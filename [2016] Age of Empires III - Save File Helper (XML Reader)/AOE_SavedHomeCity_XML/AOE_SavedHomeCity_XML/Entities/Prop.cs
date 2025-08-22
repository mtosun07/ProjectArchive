using AOE3_HomeCity.Entities.ValueTypes;
using System;

namespace AOE3_HomeCity.Entities
{
    public class Prop : HomeCityComponent<Prop>
    {
        public Prop()
            :base()
        {
            Enabled = true;
            Name = string.Empty;
        }


        public ByteBoolean? Enabled { get; set; }
        public string Name { get; set; }

        public static Prop Empty
        {
            get { return new Prop() { Enabled = null, Name = string.Empty }; }
        }
        
        public override bool IsEmpty
        {
            get
            { return Equals(Empty); }
        }
        protected override int HashCode
        {
            get { return 31 * Name.GetHashCode() + Enabled.GetHashCode(); }
        }

        public override bool UniqueIdEquals(Prop other)
        {
            if (other == null)
                throw new ArgumentNullException("other");
            return string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(other.Name) ? false : Name.Equals(other.Name);
        }
        public override bool ExactEquals(Prop other)
        {
            if (other == null)
                throw new ArgumentNullException("other");
            return ((Name == null && other.Name == null) || (Name != null && other.Name != null && Name.Equals(other.Name))) &&
                ((!Enabled.HasValue && !other.Enabled.HasValue) || (Enabled.HasValue && other.Enabled.HasValue && Enabled.Equals(other.Enabled)));
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? string.Empty : Name;
        }
    }
}