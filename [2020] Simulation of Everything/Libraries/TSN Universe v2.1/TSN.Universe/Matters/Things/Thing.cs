using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using TSN.Universe.Matters.Foods;
using static TSN.Universe.Localization.Messages;

namespace TSN.Universe.Matters.Things
{
    [Serializable] public sealed class Thing : Matter
    {
        internal Thing(Universe universe, Match? parents, Gender gender, ushort sense, double deathRate, FoodReturningOptions returnFoodAfterDeath, Location location)
            : base(universe, location, !parents.HasValue)
        {
            if (!Enum.TryParse<FoodReturningOptions>(returnFoodAfterDeath.ToString(), out _))
                throw new ArgumentOutOfRangeException(nameof(returnFoodAfterDeath));
            _parents = parents;
            _gender = gender;
            _sense = sense;
            _deathRate = deathRate;
            _returnsFoodAfterDeath = returnFoodAfterDeath;
            _eatenFoods = new Dictionary<uint, Guid>();
            _reproducements = new HashSet<Reproducement>();
            _isHungry = true;
            _returnedFood = null;
            _childrenCount = 0;
        }
        private Thing(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            var returnFoodAfterDeath = (FoodReturningOptions)info.GetUInt32(FieldReturnsFoodAfterDeath);
            if (!Enum.TryParse<FoodReturningOptions>(returnFoodAfterDeath.ToString(), out _))
                throw new ArgumentOutOfRangeException(nameof(returnFoodAfterDeath));
            var parents = (Match?)info.GetValue(FieldParents, typeof(Match?));
            var gender = (Gender)info.GetValue(FieldGender, typeof(Gender));
            var sense = info.GetUInt16(FieldSense);
            var deathRate = info.GetDouble(FieldDeathRate);
            var isHungry = info.GetBoolean(FieldIsHungry);
            var returnedFood = (Guid?)info.GetValue(FieldReturnedFood, typeof(Guid?));
            var eatenFoods = (Dictionary<uint, Guid>)info.GetValue(FieldEatenFoods, typeof(Dictionary<uint, Guid>));
            var reproducments = (HashSet<Reproducement>)info.GetValue(FieldReproducements, typeof(HashSet<Reproducement>));
            _parents = parents;
            _gender = gender;
            _sense = sense;
            _deathRate = deathRate;
            _returnsFoodAfterDeath = returnFoodAfterDeath;
            _eatenFoods = eatenFoods;
            _reproducements = reproducments;
            _isHungry = isHungry;
            _returnedFood = returnedFood;
        }


        private const string FieldParents = "Parents";
        private const string FieldGender = "Gender";
        private const string FieldSense = "Sense";
        private const string FieldDeathRate = "DeathRate";
        private const string FieldReturnsFoodAfterDeath = "ReturnsFoodAfterDeath";
        private const string FieldEatenFoods = "EatenFoods";
        private const string FieldReproducements = "Reproducement";
        private const string FieldIsHungry = "IsHungry";
        private const string FieldReturnedFood = "ReturnedFood";

        private readonly Match? _parents;
        private readonly Gender _gender;
        private readonly ushort _sense;
        private readonly double _deathRate;
        private readonly FoodReturningOptions _returnsFoodAfterDeath;
        private readonly Dictionary<uint, Guid> _eatenFoods;
        private readonly HashSet<Reproducement> _reproducements;
        private bool _isHungry;
        private Guid? _returnedFood;
        [NonSerialized] private int _childrenCount;

        public Gender Gender => _gender * Math.Pow(.99, _childrenCount);
        public ushort Sense => _sense;
        public double DeathRate => _deathRate;
        public FoodReturningOptions ReturnsFoodAfterDeath => _returnsFoodAfterDeath;
        public new DeathReasonsForThings? DeathReason => base.DeathReason == DeathReasons.NONE ? (DeathReasonsForThings?)null : (DeathReasonsForThings)base.DeathReason;
        public bool IsHungry => _isHungry;
        public bool HasParents => _parents.HasValue && !_parents.Value.IsEmpty;
        public int EatenFoodsCount => _eatenFoods.Count;
        public IReadOnlyCollection<uint> EatenFoodGenerations => _eatenFoods.Keys;
        public bool IsParent => _childrenCount > 0;
        public int ChildrenCount => _childrenCount;

        public event EventHandler<MatterEventArgs<Food>> EatingFood;
        public event EventHandler<MatterEventArgs<Food>> AteFood;
        public event EventHandler<EventArgs> GettingHungry;
        public event EventHandler<EventArgs> GotHungry;



        internal static void AddReproducement(Thing child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));
            IReadOnlyCollection<Thing> parents = null;
            if (child.IsSpawned || child.VanishingGeneration.HasValue || child.Generation != child.OwnerUniverse.CurrentGeneration || (parents = child.GetParents()).Count != 2 || parents.Any(x => x.VanishingGeneration.HasValue))
                throw new ArgumentException(ExceptionMessages.ARGUMENTUNEXPECTEDINNER, nameof(child));
            var parent1 = parents.First();
            var parent2 = parents.Last();
            var match = new Match(parent1, parent2);
            if (parent1._reproducements.SelectMany(x => x.GetChildren()).Contains(child.Id) || parent2._reproducements.SelectMany(x => x.GetChildren()).Contains(child.Id))
                throw new ArgumentException(ExceptionMessages.ARGUMENTALREADYINSERTED, nameof(child));
            if (parent1._reproducements.Any(x => !x.Spouses.Equals(match) && x.Generations.Contains(child.Generation)) || parent2._reproducements.Any(x => !x.Spouses.Equals(match) && x.Generations.Contains(child.Generation)))
                throw new InvalidOperationException(ExceptionMessages.ALREADYREPRODUCED);
            var reproducement = parent1._reproducements.SingleOrDefault(x => x.Spouses.Equals(match));
            var reproducement_ = parent2._reproducements.SingleOrDefault(x => x.Spouses.Equals(match));
            if (!(reproducement?.Equals(reproducement_) ?? reproducement_ == null))
                throw new InvalidOperationException(ExceptionMessages.REFERENCESNOTSAME);
            if (reproducement != null)
            {
                reproducement.AddChild(child.Generation, child.Id);
                if (!ReferenceEquals(reproducement, reproducement_))
                    reproducement_.AddChild(child.Generation, child.Id);
            }
            else
            {
                reproducement = new Reproducement(match);
                reproducement.AddChild(child.Generation, child.Id);
                parent1._reproducements.Add(reproducement);
                parent2._reproducements.Add(reproducement);
            }
            parent1._childrenCount++;
            parent2._childrenCount++;
        }

        internal IEnumerable<Location> GetSpots()
        {
            ThrowIfVanished();
            var location = Location;
            int iMin = location.X - _sense;
            iMin = iMin < 0 ? 0 : iMin;
            int jMin = location.Y - _sense;
            jMin = jMin < 0 ? 0 : jMin;
            int iMax = location.X + _sense;
            iMax = iMax >= OwnerUniverse.M ? (OwnerUniverse.M - 1) : iMax;
            int jMax = location.Y + _sense;
            jMax = jMax >= OwnerUniverse.N ? (OwnerUniverse.N - 1) : jMax;
            for (int i = iMin; i <= iMax; i++)
                for (int j = jMin; j <= jMax; j++)
                    if (location.X != i || location.Y != j)
                        yield return new Location((ushort)i, (ushort)j);
        }
        internal void Eat(Food food, bool move)
        {
            ThrowIfVanished();
            if (food == null)
                throw new ArgumentNullException(nameof(food));
            if (food.VanishingGeneration.HasValue)
                throw new ArgumentException(string.Format(ExceptionMessages.ARGUMENTVANISHED, FOOD), nameof(food));
            if (_eatenFoods.ContainsValue(food.Id))
                throw new ArgumentException(ExceptionMessages.ARGUMENTEATEN, nameof(food));
            var generation = OwnerUniverse.CurrentGeneration.Value;
            if (_eatenFoods.ContainsKey(generation))
                throw new InvalidOperationException(ExceptionMessages.ALREADYATE);
            var location = food.Location;
            EatingFood?.Invoke(this, new MatterEventArgs<Food>(food));
            food.MakeEaten(this);
            _eatenFoods.Add(generation, food.Id);
            _isHungry = false;
            if (move)
                ChangeLocation(location);
            AteFood?.Invoke(this, new MatterEventArgs<Food>(food));
        }
        internal void MakeHungry()
        {
            ThrowIfVanished();
            GettingHungry?.Invoke(this, EventArgs.Empty);
            _isHungry = true;
            GotHungry?.Invoke(this, EventArgs.Empty);
        }
        public bool TryGetReturnedFood(out Food returned)
        {
            if (_returnedFood.HasValue)
            {
                returned = (Food)OwnerUniverse.AllMattersEver[_returnedFood.Value];
                return true;
            }
            returned = null;
            return false;
        }
        public IEnumerable<Food> GetEatenFoods() => _eatenFoods.Values.Select(x => (Food)OwnerUniverse.AllMattersEver[x]);
        public bool TryGetEatenFoodAt(uint generation, out Food eatenFood)
        {
            if (_eatenFoods.TryGetValue(generation, out var eatenFoodId))
            {
                eatenFood = (Food)OwnerUniverse.AllMattersEver[eatenFoodId];
                return true;
            }
            eatenFood = null;
            return false;
        }
        public IReadOnlyCollection<Thing> GetParents() => new ReadOnlyCollection<Thing>(!HasParents ? new Thing[0] : new[] { (Thing)OwnerUniverse.AllMattersEver[_parents.Value.Feminine], (Thing)OwnerUniverse.AllMattersEver[_parents.Value.Masculine] });
        public IEnumerable<Thing> GetAscendants() => GetParents().SelectMany(x => x.GetAscendants().Concat(new[] { x })).OrderByDescending(x => x.Generation);
        public int GetChildrenCountAt(uint generation) => _reproducements.Where(x => x.Generations.Contains(generation)).Sum(x => x.GetChildrenCountAt(generation));
        public int GetChildrenCount(Predicate<int> predicateChildrenCount) => _reproducements.SelectMany(x => x.GetChildren(predicateChildrenCount).SelectMany(y => y.Children)).Count();
        public IEnumerable<Thing> GetChildren() => _reproducements.SelectMany(x => x.GetChildren()).Select(x => (Thing)OwnerUniverse.AllMattersEver[x]);
        public IEnumerable<Thing> GetChildrenAt(uint generation)
        {
            foreach (var rep in _reproducements)
                if (rep.TryGetChildrenAt(generation, out var ids))
                    return ids.Select(x => (Thing)OwnerUniverse.AllMattersEver[x]);
            return new Thing[0];
        }
        public IEnumerable<Thing> GetChildren(Predicate<int> predicateChildrenCount) => _reproducements.SelectMany(x => x.GetChildren(predicateChildrenCount).SelectMany(y => y.Children)).Select(x => (Thing)OwnerUniverse.AllMattersEver[x]);
        public IEnumerable<Thing> GetDescendants() => GetChildren().SelectMany(x => x.GetDescendants().Concat(new[] { x })).OrderBy(x => x.Generation);
        public IEnumerable<MatchInfo> GetMatches(Predicate<int> predicateChildrenCount = null)
        {
            var predicate = predicateChildrenCount ?? new Predicate<int>(x => true);
            foreach (var rep in _reproducements)
                foreach (var c in rep.GetChildren(predicate))
                    yield return new MatchInfo(c.Generation, (Thing)OwnerUniverse.AllMattersEver[(rep.Spouses - Id).Value], c.Children.Count());
        }
        public IEnumerable<MatchInfo> GetMatches(uint generation, Predicate<int> predicateChildrenCount = null)
        {
            if (!OwnerUniverse.CurrentGeneration.HasValue || (VanishingGeneration.HasValue && generation > VanishingGeneration.Value) || generation > OwnerUniverse.CurrentGeneration.Value)
                throw new ArgumentOutOfRangeException(nameof(generation));
            var predicate = predicateChildrenCount ?? new Predicate<int>(x => true);
            foreach (var rep in _reproducements.Where(x => x.Generations.Contains(generation)))
                foreach (var c in rep.GetChildren(predicate))
                    if (c.Generation == generation)
                        yield return new MatchInfo(c.Generation, (Thing)OwnerUniverse.AllMattersEver[(rep.Spouses - Id).Value], c.Children.Count());
        }
        public IEnumerable<MatchInfo> GetMatches(Thing spouse)
        {
            if (spouse == null)
                throw new ArgumentNullException(nameof(spouse));
            if (spouse.Id.Equals(Guid.Empty) || spouse.Id.Equals(Id))
                throw new ArgumentOutOfRangeException(nameof(spouse));
            if (spouse._reproducements.Count == 0)
                return new MatchInfo[0];
            return _reproducements.Where(x => x.Spouses.IsASpouse(spouse.Id)).SelectMany(x => x.Generations.Select(y => new MatchInfo(y, spouse, x.GetChildrenCountAt(y))));
        }
        public IEnumerable<MatchInfo> GetFirstMatches(uint startingGeneration)
        {
            if (!OwnerUniverse.CurrentGeneration.HasValue || (VanishingGeneration.HasValue && startingGeneration > VanishingGeneration.Value) || startingGeneration > OwnerUniverse.CurrentGeneration.Value)
                throw new ArgumentOutOfRangeException(nameof(startingGeneration));
            if (_reproducements.Count == 0)
                return new MatchInfo[0];
            var generation = _reproducements.SelectMany(x => x.Generations).Where(x => x > startingGeneration).Min();
            return _reproducements.Where(x => x.Generations.Contains(generation)).Select(x => new MatchInfo(generation, (Thing)OwnerUniverse.AllMattersEver[(x.Spouses - Id).Value], x.GetChildrenCountAt(generation)));
        }
        public IEnumerable<uint> GetReproducementGenerations() => _reproducements.SelectMany(x => x.Generations).Distinct();
        public IEnumerable<uint> GetReproducementGenerations(Predicate<int> predicateChildrenCount) => predicateChildrenCount == null ? throw new ArgumentNullException(nameof(predicateChildrenCount)) : _reproducements.SelectMany(x => x.GetChildren(predicateChildrenCount).Select(y => y.Generation)).Distinct();
        public IEnumerable<Thing> GetSpousesAt(uint generation) => _reproducements.Where(x => x.Generations.Contains(generation)).Select(x => (x.Spouses - Id).Value).Distinct().Select(x => (Thing)OwnerUniverse.AllMattersEver[x]);
        public int GetSpouseCountDistinct() => _reproducements.Count();
        public IEnumerable<Thing> GetSpousesDistinct() => _reproducements.Select(x => (x.Spouses - Id).Value).Select(x => (Thing)OwnerUniverse.AllMattersEver[x]);
        public IReadOnlyDictionary<uint, IReadOnlyCollection<Thing>> GetSpousesByGenerations()
        {
            var dic = new Dictionary<uint, HashSet<Guid>>();
            foreach (var rep in _reproducements)
            {
                var spouse = (rep.Spouses - Id).Value;
                foreach (var gen in rep.Generations)
                    if (dic.ContainsKey(gen))
                        dic[gen].Add(spouse);
                    else
                    {
                        var hashset = new HashSet<Guid>() { spouse };
                        dic.Add(gen, hashset);
                    }
            }
            return new ReadOnlyDictionary<uint, IReadOnlyCollection<Thing>>(dic.ToDictionary(x => x.Key, x => (IReadOnlyCollection<Thing>)x.Value.Select(y => (Thing)OwnerUniverse.AllMattersEver[y]).ToList().AsReadOnly()));
        }
        public int GetMatchCount() => _reproducements.SelectMany(x => x.Generations).Count();
        public int GetMatchCountAt(uint generation) => _reproducements.Where(x => x.Generations.Contains(generation)).Select(x => (x.Spouses - Id).Value).Distinct().Count();

        private protected override string TraitsToString() => $"{GENDER}: {_gender}, {THING_SENSE}: {_sense}, {THING_DEATHRATE}: {_deathRate}";
        private protected override bool ValidateDeathReason(DeathReasons deathReason, Matter replacedTo) => Enum.TryParse<DeathReasonsForThings>(((DeathReasonsForThings)deathReason).ToString(), out var reason) && (replacedTo == null || (!replacedTo.Id.Equals(Guid.Empty) && !replacedTo.Id.Equals(Id) && !replacedTo.VanishingGeneration.HasValue && replacedTo.Generation == OwnerUniverse.CurrentGeneration.Value)) && (reason == DeathReasonsForThings.ByChance || (reason == DeathReasonsForThings.Starved && _isHungry) || (reason == DeathReasonsForThings.ReplacedToThing && (replacedTo as Thing) != null) || (reason == DeathReasonsForThings.ReplacedToFood && (replacedTo as Food) != null));
        private protected override void RevokeChanges(uint generation)
        {
            _childrenCount -= GetChildrenCountAt(generation);
            _reproducements.RemoveWhere(x => !x.RevokeChangesAt(generation));
            _eatenFoods.Remove(generation);
            if (OwnerUniverse.CurrentGeneration == generation)
            {
                _isHungry = generation == 0 || !_eatenFoods.ContainsKey(generation - 1);
                _returnedFood = null;
            }
            else if (VanishingGeneration == generation)
                _returnedFood = null;
        }
        private protected override void OnMoving(MovingEventArgs e)
        {
            Guid? id = OwnerUniverse.GetMatterId(e.TargetLocation);
            if (id.HasValue && !(OwnerUniverse.AllMattersEver[id.Value] is Food))
                throw new InvalidOperationException(ExceptionMessages.TARGETOCUPPYING);
            base.OnMoving(e);
        }
        private protected override void OnVanished(VanishingEventArgs e)
        {
            base.OnVanished(e);
            if (_returnsFoodAfterDeath == FoodReturningOptions.ReturnAlways || (_returnsFoodAfterDeath == FoodReturningOptions.ReturnIfNotHungry && !_isHungry))
            {
                var food = new Food(OwnerUniverse, this, Location);
                if (OwnerUniverse.Insert(food, null))
                    _returnedFood = food.Id;
                else
                    throw new InvalidOperationException(ExceptionMessages.NOTRETURNED);
            }
        }
        public override bool Equals(Matter other) => other is Thing thing && base.Equals(thing) && _parents.Equals(thing._parents) && _gender.Equals(thing._gender) && _sense == thing._sense && _deathRate == thing._deathRate;
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(FieldParents, _parents, typeof(Match?));
            info.AddValue(FieldGender, _gender, typeof(Gender));
            info.AddValue(FieldSense, _sense);
            info.AddValue(FieldDeathRate, _deathRate);
            info.AddValue(FieldReturnsFoodAfterDeath, (uint)_returnsFoodAfterDeath);
            info.AddValue(FieldIsHungry, _isHungry);
            info.AddValue(FieldReturnedFood, _returnedFood, typeof(Guid?));
            info.AddValue(FieldEatenFoods, _eatenFoods, typeof(Dictionary<uint, Guid>));
            info.AddValue(FieldReproducements, _reproducements, typeof(HashSet<Reproducement>));
        }
        private protected override void OnDeserialization(object sender)
        {
            base.OnDeserialization(sender);
            _eatenFoods.OnDeserialization(sender);
            _reproducements.OnDeserialization(sender);
            _childrenCount = _reproducements.Sum(x => x.GetChildrenCount());
        }
    }
}