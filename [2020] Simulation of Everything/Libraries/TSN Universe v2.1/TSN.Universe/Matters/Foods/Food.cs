using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using TSN.Universe.Matters.Things;
using static TSN.Universe.Localization.Messages;

namespace TSN.Universe.Matters.Foods
{
    [Serializable] public sealed class Food : Matter
    {
        internal Food(Universe universe, Thing returnedFrom, Location location)
            : base(universe, location, returnedFrom == null)
        {
            if (returnedFrom != null)
            {
                if (returnedFrom.Id.Equals(Guid.Empty) || returnedFrom.OwnerUniverse != OwnerUniverse)
                    throw new ArgumentException(ExceptionMessages.ARGUMENTUNEXPECTEDINNER, nameof(returnedFrom));
                else if (!returnedFrom.VanishingGeneration.HasValue)
                    throw new ArgumentException(string.Format(ExceptionMessages.ARGUMENTVANISHEDNOT, THING), nameof(returnedFrom));
                _returnedFrom = returnedFrom?.Id;
            }
            else
                _returnedFrom = null;
        }
        private Food(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            var returnedFrom = (Guid?)info.GetValue(FieldReturnedFrom, typeof(Guid?));
            var eatenBy = (Guid?)info.GetValue(FieldEatenBy, typeof(Guid?));
            _returnedFrom = returnedFrom;
            _eatenBy = eatenBy;
        }


        private const string FieldReturnedFrom = "ReturnedFrom";
        private const string FieldEatenBy = "EatenBy";
        
        private readonly Guid? _returnedFrom;
        private Guid? _eatenBy;

        public bool IsReturned => _returnedFrom.HasValue;
        public bool IsEaten => _eatenBy.HasValue;
        public new DeathReasonsForFoods? DeathReason => base.DeathReason == DeathReasons.NONE ? (DeathReasonsForFoods?)null : (DeathReasonsForFoods)base.DeathReason;

        public event EventHandler<MatterEventArgs<Thing>> GettingEaten;
        public event EventHandler<MatterEventArgs<Thing>> GotEaten;



        internal void MakeEaten(Thing eatenBy)
        {
            ThrowIfVanished();
            if (eatenBy == null)
                throw new ArgumentNullException(nameof(eatenBy));
            if (eatenBy.VanishingGeneration.HasValue)
                throw new ArgumentException(string.Format(ExceptionMessages.ARGUMENTVANISHED, THING), nameof(eatenBy));
            GettingEaten?.Invoke(this, new MatterEventArgs<Thing>(eatenBy));
            _eatenBy = eatenBy.Id;
            Vanish(DeathReasons.Eaten);
            GotEaten?.Invoke(this, new MatterEventArgs<Thing>(eatenBy));
        }
        public bool TryGetReturnedFrom(out Thing returnedFrom) => (returnedFrom = _returnedFrom.HasValue ? (Thing)OwnerUniverse.AllMattersEver[_returnedFrom.Value] : null) != null;
        public bool TryGetEatenBy(out Thing eatenBy) => (eatenBy = _eatenBy.HasValue ? (Thing)OwnerUniverse.AllMattersEver[_eatenBy.Value] : null) != null;

        private protected override string TraitsToString() => $"{FOOD_ISEATEN} : {(IsEaten ? YES : NO)}, {FOOD_ISRETURNED} : {(IsReturned ? YES : NO)}";
        private protected override bool ValidateDeathReason(DeathReasons deathReason, Matter replacedTo) => Enum.TryParse<DeathReasonsForFoods>(((DeathReasonsForFoods)deathReason).ToString(), out var reason) && (replacedTo == null || (!replacedTo.Id.Equals(Guid.Empty) && !replacedTo.Id.Equals(Id) && !replacedTo.VanishingGeneration.HasValue && replacedTo.Generation == OwnerUniverse.CurrentGeneration.Value)) && (reason == DeathReasonsForFoods.DueToSimulation || (reason == DeathReasonsForFoods.Eaten && _eatenBy.HasValue) || (reason == DeathReasonsForFoods.ReplacedToThing && (replacedTo as Thing) != null));
        private protected override void RevokeChanges(uint generation)
        {
            if (VanishingGeneration == generation)
                _eatenBy = null;
        }
        private protected override void OnMoving(MovingEventArgs e) => throw new NotSupportedException();
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(FieldReturnedFrom, _returnedFrom, typeof(Guid?));
            info.AddValue(FieldEatenBy, _eatenBy, typeof(Guid?));
        }
        public override bool Equals(Matter other) => other is Food && base.Equals(other);
    }
}