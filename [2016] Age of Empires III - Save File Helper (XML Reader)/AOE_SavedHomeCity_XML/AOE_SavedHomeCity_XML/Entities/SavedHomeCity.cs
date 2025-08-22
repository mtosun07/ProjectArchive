using AOE3_HomeCity.Entities.ValueTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace AOE3_HomeCity.Entities
{
    public class SavedHomeCity
    {
        public SavedHomeCity()
        {
            Version = null;
            DefaultDirectoryID = null;
            DefaultFileName = string.Empty;
            Civilization = string.Empty;
            HomeCityType = string.Empty;
            Name = string.Empty;
            HeroName = string.Empty;
            HeroDogName = string.Empty;
            ShipName = string.Empty;
            HomeCityID = null;
            Respec_Exists = true;
            Respec = null;
            Level = null;
            XP = null;
            SkillPoints = null;
            XPPercentage = null;
            NumPropUnlocksEarned = null;
            Decks = new Deck[0];
            ActiveTechs = new Tech[0];
            ActiveProps = new Prop[0];
        }


        public uint? Version { get; set; }
        public uint? DefaultDirectoryID { get; set; }
        public string DefaultFileName { get; set; }
        public string Civilization { get; set; }
        public string HomeCityType { get; set; }
        public string Name { get; set; }
        public string HeroName { get; set; }
        public string HeroDogName { get; set; }
        public string ShipName { get; set; }
        public uint? HomeCityID { get; set; }
        public bool Respec_Exists { get; set; }
        public ByteBoolean? Respec { get; set; }
        public uint? Level { get; set; }
        public uint? XP { get; set; }
        public uint? SkillPoints { get; set; }
        public UDecimal6? XPPercentage { get; set; }
        public uint? NumPropUnlocksEarned { get; set; }
        public Deck[] Decks { get; set; }
        public Tech[] ActiveTechs { get; set; }
        public Prop[] ActiveProps { get; set; }



        public string ToXml()//bool isBackup = false)
        {
            var xml_string = new StringBuilder();
            XmlWriter writer = null;
            try
            {
                writer = XmlWriter.Create(xml_string, new XmlWriterSettings() { CloseOutput = false, Indent = true, Encoding = Encoding.Unicode });

                var xeDecks = new XElement("decks");
                if (Decks != null)
                    foreach (var deck in Decks)
                    {
                        var xeCards = new XElement("cards");
                        if (deck.Cards != null)
                            foreach (var card in deck.Cards)
                                xeCards.Add(
                                    new XElement("card", new XAttribute("dbid", card.DBID.HasValue ? card.DBID.Value.ToString() : string.Empty), card.Name));
                        var xeDeck = new XElement("deck", new XElement("name", deck.Name));
                        if (deck.GameID_Exists)
                            xeDeck.Add(new XElement("gameid", deck.GameID.HasValue ? deck.GameID.Value.ToString() : string.Empty));
                        xeDeck.Add(xeCards);
                        xeDecks.Add(xeDeck);
                    }
                var xeActiveTechs = new XElement("activetechs");
                if (ActiveTechs != null)
                    foreach (var tech in ActiveTechs)
                        xeActiveTechs.Add(new XElement("tech", new XAttribute("dbid", tech.DBID.HasValue ? tech.DBID.Value.ToString() : string.Empty), tech.Name));
                var xeActiveProps = new XElement("activeprops");
                if (ActiveProps != null)
                    foreach (var prop in ActiveProps)
                        xeActiveProps.Add(
                            new XElement("prop", new XAttribute("enabled", prop.Enabled.HasValue ? prop.Enabled.Value.ToString() : string.Empty), prop.Name));
                var xe = new XElement("savedhomecity", new XAttribute("version", Version.HasValue ? Version.Value.ToString() : string.Empty),
                    new XElement("defaultdirectoryid", DefaultDirectoryID.HasValue ? DefaultDirectoryID.Value.ToString() : string.Empty),
                    new XElement("defaultfilename", DefaultFileName),
                    new XElement("civ", Civilization),
                    new XElement("hctype", HomeCityType),
                    new XElement("name", Name),
                    new XElement("heroname", HeroName),
                    new XElement("herodogname", HeroDogName),
                    new XElement("shipname", ShipName),
                    new XElement("hcid", HomeCityID.HasValue ? HomeCityID.Value.ToString() : string.Empty));
                if (Respec_Exists)
                    xe.Add(new XElement("respec", Respec.HasValue ? Respec.Value.ToString() : string.Empty));
                xe.Add(
                    new XElement("level", Level.HasValue ? Level.Value.ToString() : string.Empty),
                    new XElement("xp", XP.HasValue ? XP.Value.ToString() : string.Empty),
                    new XElement("skillpoints", SkillPoints.HasValue ? SkillPoints.Value.ToString() : string.Empty),
                    new XElement("xppercentage", XPPercentage.HasValue ? XPPercentage.Value.ToString() : string.Empty),
                    new XElement("numpropunlocksearned", NumPropUnlocksEarned.HasValue ? NumPropUnlocksEarned.Value.ToString() : string.Empty),
                    xeDecks,
                    xeActiveTechs,
                    xeActiveProps);

                (new XDocument(xe)).WriteTo(writer);
                //(new XDocument(!isBackup ? xe : new XElement("backup", new XAttribute("DateTime", DateTime.Now.ToString()), xe))).WriteTo(writer);

                writer.Flush();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                writer.Close();
                writer.Dispose();
            }
            return xml_string.ToString();
        }
        public static string ToXml(SavedHomeCity hc)//, bool isBackup = false)
        {
            if (hc == null)
                throw new ArgumentNullException("hc");
            return hc.ToXml();//isBackup);
        }
        public static SavedHomeCity FromXml(string xml_string)
        {
            if (xml_string == null)
                throw new ArgumentNullException("xml_string");
            if (xml_string == string.Empty)
                throw new ArgumentException("Argument was empty.", "xml_string");

            SavedHomeCity hc = null;
            try
            {
                var xeSavedHomeCity = XDocument.Parse(xml_string).Descendants("savedhomecity").First();

                var version = xeSavedHomeCity.Attribute("version").Value;
                var defaultDirectoryID = xeSavedHomeCity.Element("defaultdirectoryid").Value;
                var defaultFileName = xeSavedHomeCity.Element("defaultfilename").Value;
                var civilization = xeSavedHomeCity.Element("civ").Value;
                var homeCityType = xeSavedHomeCity.Element("hctype").Value;
                var name = xeSavedHomeCity.Element("name").Value;
                var heroName = xeSavedHomeCity.Element("heroname").Value;
                var heroDogName = xeSavedHomeCity.Element("herodogname").Value;
                var shipName = xeSavedHomeCity.Element("shipname").Value;
                var homeCityID = xeSavedHomeCity.Element("hcid").Value;
                var level = xeSavedHomeCity.Element("level").Value;
                var xp = xeSavedHomeCity.Element("xp").Value;
                var skillPoints = xeSavedHomeCity.Element("skillpoints").Value;
                var xpPercentage = xeSavedHomeCity.Element("xppercentage").Value;
                var numPropUnlocksEarned = xeSavedHomeCity.Element("numpropunlocksearned").Value;

                var xeRespec = xeSavedHomeCity.Element("respec");
                bool respec_Exists = false;
                string respec = "";
                if ((respec_Exists = xeRespec != null))
                    respec = xeRespec.Value;

                var decks = new List<Deck>();
                var xeDecks = xeSavedHomeCity.Element("decks");
                foreach (var xeDeck in xeDecks.Elements())
                {
                    var name_deck = xeDeck.Element("name").Value;

                    bool gameID_Exists_deck = false;
                    string gameID_deck = string.Empty;
                    var xeGameID = xeDeck.Element("gameid");
                    if (gameID_Exists_deck = xeGameID != null)
                        gameID_deck = xeGameID.Value;

                    var cards_deck = new List<Tech>();
                    var xeCards = xeDeck.Element("cards");
                    foreach (var xeCard in xeCards.Descendants())
                    {
                        var dbID_card = xeCard.Attribute("dbid").Value;
                        var name_card = xeCard.Value;
                        cards_deck.Add(new Tech()
                        {
                            DBID = string.IsNullOrEmpty(dbID_card) ? null : (uint?)uint.Parse(dbID_card),
                            Name = name_card
                        });
                    }
                    cards_deck.TrimExcess();
                    decks.Add(new Deck()
                    {
                        Name = name_deck,
                        GameID_Exists = gameID_Exists_deck,
                        GameID = string.IsNullOrEmpty(gameID_deck) ? null : (uint?)uint.Parse(gameID_deck),
                        Cards = cards_deck.ToArray()
                    });
                }
                decks.TrimExcess();

                var activeTechs = new List<Tech>();
                var xeActiveTechs = xeSavedHomeCity.Element("activetechs");
                foreach (var xeTech in xeActiveTechs.Elements())
                {
                    var dbID_tech = xeTech.Attribute("dbid").Value;
                    var name_tech = xeTech.Value;
                    activeTechs.Add(new Tech()
                    {
                        DBID = string.IsNullOrEmpty(dbID_tech) ? null : (uint?)uint.Parse(dbID_tech),
                        Name = name_tech
                    });
                }
                activeTechs.TrimExcess();

                var activeProps = new List<Prop>();
                var xeActiveProps = xeSavedHomeCity.Element("activeprops");
                foreach (var xeProp in xeActiveProps.Elements())
                {
                    var enabled_prop = xeProp.Attribute("enabled").Value;
                    var name_prop = xeProp.Value;
                    activeProps.Add(new Prop()
                    {
                        Enabled = string.IsNullOrEmpty(enabled_prop) ? null : (byte?)byte.Parse(enabled_prop),
                        Name = name_prop
                    });
                }
                activeProps.TrimExcess();

                hc = new SavedHomeCity()
                {
                    Version = string.IsNullOrEmpty(version) ? null : (uint?)uint.Parse(version),
                    DefaultDirectoryID = string.IsNullOrEmpty(defaultDirectoryID) ? null : (uint?)uint.Parse(defaultDirectoryID),
                    DefaultFileName = defaultFileName,
                    Civilization = civilization,
                    HomeCityType = homeCityType,
                    Name = name,
                    HeroName = heroName,
                    HeroDogName = heroDogName,
                    ShipName = shipName,
                    HomeCityID = string.IsNullOrEmpty(homeCityID) ? null : (uint?)uint.Parse(homeCityID),
                    Respec_Exists = respec_Exists,
                    Respec = string.IsNullOrEmpty(respec) ? null : (byte?)byte.Parse(respec),
                    Level = string.IsNullOrEmpty(level) ? null : (uint?)uint.Parse(level),
                    XP = string.IsNullOrEmpty(xp) ? null : (uint?)uint.Parse(xp),
                    SkillPoints = string.IsNullOrEmpty(skillPoints) ? null : (uint?)uint.Parse(skillPoints),
                    XPPercentage = string.IsNullOrEmpty(xpPercentage) ? null : (decimal?)decimal.Parse(xpPercentage, CultureInfo.InvariantCulture),
                    NumPropUnlocksEarned = string.IsNullOrEmpty(numPropUnlocksEarned) ? null : (uint?)uint.Parse(numPropUnlocksEarned),
                    Decks = decks.ToArray(),
                    ActiveTechs = activeTechs.ToArray(),
                    ActiveProps = activeProps.ToArray()
                };
            }
            catch (Exception ex)
            {
                hc = null;
                throw new Exception("File was corrupted.", ex);
            }
            return hc;
        }
        //public static SavedHomeCity FromXml_Backup(string xml_string, out DateTime dt)
        //{
        //    if (xml_string == null)
        //        throw new ArgumentNullException("xml_string");
        //    if (xml_string == string.Empty)
        //        throw new ArgumentException("Argument was empty.", "xml_string");

        //    dt = new DateTime();
        //    SavedHomeCity hc = null;
        //    try
        //    {
        //        var xeRoot = XDocument.Parse(xml_string).Descendants("backup").First();
        //        dt = DateTime.Parse(xeRoot.Attribute("DateTime").Value);
        //        var xeSavedHomeCity = xeRoot.Descendants("savedhomecity").First();
        //        hc = FromXml(xeSavedHomeCity.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        hc = null;
        //        dt = new DateTime();
        //        throw new Exception("File was corrupted.", ex);
        //    }
        //    return hc;
        //}

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? string.Empty : Name;
        }
    }
}