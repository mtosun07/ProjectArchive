using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using static TSN.Universe.Localization.Messages;

namespace TSN.Universe.Matters.Things
{
    [Serializable] internal struct Match : IEquatable<Match>, ISerializable
    {
        internal Match(Thing spouse1, Thing spouse2)
        {
            if (spouse1 == null)
                throw new ArgumentNullException(nameof(spouse1));
            if (spouse2 == null)
                throw new ArgumentNullException(nameof(spouse2));
            if (spouse1.Id.Equals(spouse2.Id))
                throw new ArgumentException(ExceptionMessages.ARGUMENTSPOUSESEQUAL);
            if (Gender.IsFeminine(spouse1.Gender))
            {
                _feminine = spouse1.Id;
                _masculine = spouse2.Id;
            }
            else
            {
                _feminine = spouse2.Id;
                _masculine = spouse1.Id;
            }
        }
        private Match(Guid spouse1, Guid spouse2)
        {
            if (spouse1.Equals(spouse2))
                throw new ArgumentException(ExceptionMessages.ARGUMENTSPOUSESEQUAL);
            _feminine = spouse1;
            _masculine = spouse2;
        }
        private Match(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            var spouse1 = (Guid)info.GetValue(FieldFeminine, typeof(Guid));
            var spouse2 = (Guid)info.GetValue(FieldMasculine, typeof(Guid));
            _feminine = spouse1;
            _masculine = spouse2;
        }


        private const string FieldFeminine = "Feminine";
        private const string FieldMasculine = "Masculine";

        private readonly Guid _feminine;
        private readonly Guid _masculine;

        public Guid Feminine => _feminine;
        public Guid Masculine => _masculine;
        public bool IsEmpty => _feminine.Equals(Guid.Empty) || _masculine.Equals(Guid.Empty);



        internal bool IsASpouse(Guid id) => _feminine.Equals(id) || _masculine.Equals(id);
        internal bool TryGetOtherSpouse(Guid knownSpouse, out Guid otherSpouse)
        {
            if (_feminine.Equals(knownSpouse))
            {
                otherSpouse = _masculine;
                return true;
            }
            if (_masculine.Equals(knownSpouse))
            {
                otherSpouse = _feminine;
                return true;
            }
            otherSpouse = Guid.Empty;
            return false;
        }

        public override string ToString() => $"({_feminine}, {_masculine})";
        public override int GetHashCode()
        {
            unchecked
            {
                return _feminine.GetHashCode() + _masculine.GetHashCode();
            }
        }
        public override bool Equals(object obj) => obj is Match match && Equals(match);

        public bool Equals(Match other) => (_feminine.Equals(other._feminine) && _masculine.Equals(other._masculine)) || (_feminine.Equals(other._masculine) && _masculine.Equals(other._feminine));

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            info.AddValue(FieldFeminine, _feminine, typeof(Guid));
            info.AddValue(FieldMasculine, _masculine, typeof(Guid));
        }

        public static implicit operator Match((Guid, Guid) obj) => new Match(obj.Item1, obj.Item2);
        public static implicit operator (Guid, Guid)(Match obj) => (obj.Feminine, obj.Masculine);

        public static Guid? operator -(Match match, Guid id) => match.Feminine.Equals(id) ? match.Masculine : (match.Masculine.Equals(id) ? match.Feminine : (Guid?)null);
    }
}