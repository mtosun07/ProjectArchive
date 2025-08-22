using System;
using System.Linq;

namespace AOE3_HomeCity.Entities
{
    public class Deck : HomeCityComponent<Deck>
    {
        public Deck()
            : base()
        {
            GameID_Exists = true;
            GameID = null;
            Cards = new Tech[0];
        }


        public string Name { get; set; }
        public bool GameID_Exists { get; set; }
        public uint? GameID { get; set; }
        public Tech[] Cards { get; set; }

        
        public override bool IsEmpty
        {
            get { return string.IsNullOrEmpty(Name) && !GameID.HasValue && (Cards == null || Cards.Length == 0); }
        }
        protected override int HashCode
        {
            get { return 31 * Name.GetHashCode() + 17 * (GameID_Exists.GetHashCode() + 13 * (GameID.GetHashCode() + Cards.GetHashCode())); }
        }

        public override bool UniqueIdEquals(Deck other)
        {
            if (other == null)
                throw new ArgumentNullException("other");
            return string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(other.Name) ? false : Name.Equals(other.Name);
        }
        public override bool ExactEquals(Deck other)
        {
            if (other == null)
                throw new ArgumentNullException("other");
            if ((GameID_Exists == other.GameID_Exists && ((GameID.HasValue && other.GameID.HasValue && GameID.Value.Equals(other.GameID.Value)) ||
                (!GameID.HasValue && !other.GameID.HasValue))) && 
                ((Name == null && other.Name == null) || (Name != null && other.Name != null && Name.Equals(other.Name))))
            {
                if (Cards == null && other.Cards == null)
                    return true;
                if (Cards != null && other.Cards != null && Cards.Length.Equals(other.Cards.Length))
                {
                    bool eq = true;
                    foreach (var tech in Cards)
                        if (!other.Cards.Contains(tech))
                        {
                            eq = false;
                            break;
                        }
                    return eq;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? string.Empty : Name;
        }
    }
}