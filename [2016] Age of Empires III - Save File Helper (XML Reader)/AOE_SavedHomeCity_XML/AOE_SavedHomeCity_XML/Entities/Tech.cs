using System;

namespace AOE3_HomeCity.Entities
{
    public class Tech : HomeCityComponent<Tech>
    {
        public Tech()
            :base()
        {
            DBID = null;
            Name = string.Empty;
        }


        public uint? DBID { get; set; }
        public string Name { get; set; }

        public static Tech Empty
        {
            get { return new Tech() { DBID = null, Name = string.Empty }; }
        }
        
        public override bool IsEmpty
        {
            get { return Equals(Empty); }
        }
        protected override int HashCode
        {
            get { return 31 * Name.GetHashCode() + DBID.GetHashCode(); }
        }

        public override bool UniqueIdEquals(Tech other)
        {
            if (other == null)
                throw new ArgumentNullException("other");
            return DBID.HasValue && other.DBID.HasValue && DBID.Equals(other.DBID);
        }
        public override bool ExactEquals(Tech other)
        {
            if (other == null)
                throw new ArgumentNullException("other");
            return 
                ((!DBID.HasValue && !other.DBID.HasValue) || (DBID.HasValue && other.DBID.HasValue && DBID.Value.Equals(other.DBID.Value))) && 
                ((Name == null && other.Name == null) || (Name != null && other.Name != null && Name.Equals(other.Name)));
        }
        public bool Equals(Tech other)
        {
            return ExactEquals(other);
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? string.Empty : Name;
        }
    }
}