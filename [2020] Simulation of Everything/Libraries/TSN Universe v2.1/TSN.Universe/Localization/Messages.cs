using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace TSN.Universe.Localization
{
    internal static class Messages
    {
        static Messages()
        {
            _allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            var messagesDefault = GetMessages(Properties.Resources.Languages_Default).ToArray();
            var languageDefault = messagesDefault.First().CultureName;
            _dictionary = new ReadOnlyDictionary<string, Message>(messagesDefault.Concat(GetMessages(Properties.Resources.Languages).Where(x => !x.CultureName.Equals(languageDefault)).GroupBy(x => x.CultureName).Select(x => (x.Key, Items: x.ToArray())).Where(x => x.Items.Length == x.Items.Select(y => y.MessageName).Distinct().Count()).SelectMany(x => x.Items)).GroupBy(x => x.MessageName).ToDictionary(x => x.Key, x => new Message(languageDefault, x.Single(y => y.CultureName.Equals(languageDefault)).Message, x.Where(y => !y.CultureName.Equals(languageDefault)).Select(y => (y.CultureName, y.Message)).ToArray())));
            _ALGORITHM = new Lazy<Message>(() => _dictionary["ALGORITHM"]);
            _YES = new Lazy<Message>(() => _dictionary["YES"]);
            _NO = new Lazy<Message>(() => _dictionary["NO"]);
            _GENERATION = new Lazy<Message>(() => _dictionary["GENERATION"]);
            _GENERATIONS = new Lazy<Message>(() => _dictionary["GENERATIONS"]);
            _LOCATION = new Lazy<Message>(() => _dictionary["LOCATION"]);
            _MATTER = new Lazy<Message>(() => _dictionary["MATTER"]);
            _MATTER_ID = new Lazy<Message>(() => _dictionary["MATTER_ID"]);
            _MATTER_VANISHING = new Lazy<Message>(() => _dictionary["MATTER_VANISHING"]);
            _FOOD = new Lazy<Message>(() => _dictionary["FOOD"]);
            _FOOD_ISEATEN = new Lazy<Message>(() => _dictionary["FOOD_ISEATEN"]);
            _FOOD_ISRETURNED = new Lazy<Message>(() => _dictionary["FOOD_ISRETURNED"]);
            _THING = new Lazy<Message>(() => _dictionary["THING"]);
            _GENDER = new Lazy<Message>(() => _dictionary["GENDER"]);
            _GENDER_NEUTRAL = new Lazy<Message>(() => _dictionary["GENDER_NEUTRAL"]);
            _GENDER_FEMININE = new Lazy<Message>(() => _dictionary["GENDER_FEMININE"]);
            _GENDER_MASCULINE = new Lazy<Message>(() => _dictionary["GENDER_MASCULINE"]);
            _CHILDRENCOUNT = new Lazy<Message>(() => _dictionary["CHILDRENCOUNT"]);
            _OTHERPARENT = new Lazy<Message>(() => _dictionary["OTHERPARENT"]);
            _SPOUSES = new Lazy<Message>(() => _dictionary["SPOUSES"]);
            _THING_SENSE = new Lazy<Message>(() => _dictionary["THING_SENSE"]);
            _THING_DEATHRATE = new Lazy<Message>(() => _dictionary["THING_DEATHRATE"]);
            _PARAMETERS_SPAWNRATE_THING = new Lazy<Message>(() => _dictionary["PARAMETERS_SPAWNRATE_THING"]);
            _PARAMETERS_SPAWNRATE_FOOD = new Lazy<Message>(() => _dictionary["PARAMETERS_SPAWNRATE_FOOD"]);
            _PARAMETERS_FEMININERATE = new Lazy<Message>(() => _dictionary["PARAMETERS_FEMININERATE"]);
            _PARAMETERS_REPRODUCERATE_MIN = new Lazy<Message>(() => _dictionary["PARAMETERS_REPRODUCERATE_MIN"]);
            _PARAMETERS_REPRODUCERATE_MAX = new Lazy<Message>(() => _dictionary["PARAMETERS_REPRODUCERATE_MAX"]);
            _PARAMETERS_DEATHRATE_MIN = new Lazy<Message>(() => _dictionary["PARAMETERS_DEATHRATE_MIN"]);
            _PARAMETERS_DEATHRATE_MAX = new Lazy<Message>(() => _dictionary["PARAMETERS_DEATHRATE_MAX"]);
            _PARAMETERS_SENSE_MIN = new Lazy<Message>(() => _dictionary["PARAMETERS_SENSE_MIN"]);
            _PARAMETERS_SENSE_MAX = new Lazy<Message>(() => _dictionary["PARAMETERS_SENSE_MAX"]);
            _OPTIONS = new Lazy<Message>(() => _dictionary["OPTIONS"]);
            _ALLMATTERS = new Lazy<Message>(() => _dictionary["ALLMATTERS"]);
            _THINGS = new Lazy<Message>(() => _dictionary["THINGS"]);
            _FOODS = new Lazy<Message>(() => _dictionary["FOODS"]);
            _ALIVE = new Lazy<Message>(() => _dictionary["ALIVE"]);
            _DEAD = new Lazy<Message>(() => _dictionary["DEAD"]);
            _ALIVEDEAD = new Lazy<Message>(() => _dictionary["ALIVEDEAD"]);
            _REPRODUCERATE = new Lazy<Message>(() => _dictionary["REPRODUCERATE"]);
            _POPULATION = new Lazy<Message>(() => _dictionary["POPULATION"]);
            _LIFELENGTH = new Lazy<Message>(() => _dictionary["LIFELENGTH"]);
            _SPAWNED = new Lazy<Message>(() => _dictionary["SPAWNED"]);
            _REPRODUCED = new Lazy<Message>(() => _dictionary["REPRODUCED"]);
            _OLDGENERATION = new Lazy<Message>(() => _dictionary["OLDGENERATION"]);
            _HUNGRY = new Lazy<Message>(() => _dictionary["HUNGRY"]);
            _ATEAFTERBIRTH = new Lazy<Message>(() => _dictionary["ATEAFTERBIRTH"]);
            _GOTFED = new Lazy<Message>(() => _dictionary["GOTFED"]);
            _MOVED = new Lazy<Message>(() => _dictionary["MOVED"]);
            _MOVECOUNT = new Lazy<Message>(() => _dictionary["MOVECOUNT"]);
            _PARENT = new Lazy<Message>(() => _dictionary["PARENT"]);
            _REPRODUCEMENTCOUNT = new Lazy<Message>(() => _dictionary["REPRODUCEMENTCOUNT"]);
            _MULTIREPRODUCED = new Lazy<Message>(() => _dictionary["MULTIREPRODUCED"]);
            _MULTIREPRODUCEMENTCOUNT = new Lazy<Message>(() => _dictionary["MULTIREPRODUCEMENTCOUNT"]);
            _HUNGRYREPRODUCED = new Lazy<Message>(() => _dictionary["HUNGRYREPRODUCED"]);
            _HUNGRYREPRODUCEMENTCOUNT = new Lazy<Message>(() => _dictionary["HUNGRYREPRODUCEMENTCOUNT"]);
            _HUNGRYMULTIREPRODUCED = new Lazy<Message>(() => _dictionary["HUNGRYMULTIREPRODUCED"]);
            _HUNGRYMULTIREPRODUCEMENTCOUNT = new Lazy<Message>(() => _dictionary["HUNGRYMULTIREPRODUCEMENTCOUNT"]);
            _MATCHCOUNT = new Lazy<Message>(() => _dictionary["MATCHCOUNT"]);
            _REPRODUCEMENTPERMATCH = new Lazy<Message>(() => _dictionary["REPRODUCEMENTPERMATCH"]);
            _DIEDBYCHANCE = new Lazy<Message>(() => _dictionary["DIEDBYCHANCE"]);
            _DIEDOFSTARVING = new Lazy<Message>(() => _dictionary["DIEDOFSTARVING"]);
            _DIEDOFREPLACINGT = new Lazy<Message>(() => _dictionary["DIEDOFREPLACINGT"]);
            _DIEDOFREPLACINGF = new Lazy<Message>(() => _dictionary["DIEDOFREPLACINGF"]);
            _RETURNED = new Lazy<Message>(() => _dictionary["RETURNED"]);
            _RETURNEDHUNGRY = new Lazy<Message>(() => _dictionary["RETURNEDHUNGRY"]);
            _DIEDOFEATEN = new Lazy<Message>(() => _dictionary["DIEDOFEATEN"]);
        }



        private static readonly CultureInfo[] _allCultures;
        private static readonly ReadOnlyDictionary<string, Message> _dictionary;
        private static readonly Lazy<Message> _ALGORITHM;
        private static readonly Lazy<Message> _YES;
        private static readonly Lazy<Message> _NO;
        private static readonly Lazy<Message> _GENERATION;
        private static readonly Lazy<Message> _GENERATIONS;
        private static readonly Lazy<Message> _MATTER;
        private static readonly Lazy<Message> _MATTER_ID;
        private static readonly Lazy<Message> _MATTER_VANISHING;
        private static readonly Lazy<Message> _LOCATION;
        private static readonly Lazy<Message> _FOOD;
        private static readonly Lazy<Message> _FOOD_ISEATEN;
        private static readonly Lazy<Message> _FOOD_ISRETURNED;
        private static readonly Lazy<Message> _THING;
        private static readonly Lazy<Message> _GENDER;
        private static readonly Lazy<Message> _GENDER_NEUTRAL;
        private static readonly Lazy<Message> _GENDER_FEMININE;
        private static readonly Lazy<Message> _GENDER_MASCULINE;
        private static readonly Lazy<Message> _CHILDRENCOUNT;
        private static readonly Lazy<Message> _OTHERPARENT;
        private static readonly Lazy<Message> _SPOUSES;
        private static readonly Lazy<Message> _THING_SENSE;
        private static readonly Lazy<Message> _THING_DEATHRATE;
        private static readonly Lazy<Message> _PARAMETERS_SPAWNRATE_THING;
        private static readonly Lazy<Message> _PARAMETERS_SPAWNRATE_FOOD;
        private static readonly Lazy<Message> _PARAMETERS_FEMININERATE;
        private static readonly Lazy<Message> _PARAMETERS_REPRODUCERATE_MIN;
        private static readonly Lazy<Message> _PARAMETERS_REPRODUCERATE_MAX;
        private static readonly Lazy<Message> _PARAMETERS_DEATHRATE_MIN;
        private static readonly Lazy<Message> _PARAMETERS_DEATHRATE_MAX;
        private static readonly Lazy<Message> _PARAMETERS_SENSE_MIN;
        private static readonly Lazy<Message> _PARAMETERS_SENSE_MAX;
        private static readonly Lazy<Message> _OPTIONS;
        private static readonly Lazy<Message> _ALLMATTERS;
        private static readonly Lazy<Message> _THINGS;
        private static readonly Lazy<Message> _FOODS;
        private static readonly Lazy<Message> _ALIVE;
        private static readonly Lazy<Message> _DEAD;
        private static readonly Lazy<Message> _ALIVEDEAD;
        private static readonly Lazy<Message> _REPRODUCERATE;
        private static readonly Lazy<Message> _POPULATION;
        private static readonly Lazy<Message> _LIFELENGTH;
        private static readonly Lazy<Message> _SPAWNED;
        private static readonly Lazy<Message> _REPRODUCED;
        private static readonly Lazy<Message> _OLDGENERATION;
        private static readonly Lazy<Message> _HUNGRY;
        private static readonly Lazy<Message> _ATEAFTERBIRTH;
        private static readonly Lazy<Message> _GOTFED;
        private static readonly Lazy<Message> _MOVED;
        private static readonly Lazy<Message> _MOVECOUNT;
        private static readonly Lazy<Message> _PARENT;
        private static readonly Lazy<Message> _REPRODUCEMENTCOUNT;
        private static readonly Lazy<Message> _MULTIREPRODUCED;
        private static readonly Lazy<Message> _MULTIREPRODUCEMENTCOUNT;
        private static readonly Lazy<Message> _HUNGRYREPRODUCED;
        private static readonly Lazy<Message> _HUNGRYREPRODUCEMENTCOUNT;
        private static readonly Lazy<Message> _HUNGRYMULTIREPRODUCED;
        private static readonly Lazy<Message> _HUNGRYMULTIREPRODUCEMENTCOUNT;
        private static readonly Lazy<Message> _MATCHCOUNT;
        private static readonly Lazy<Message> _REPRODUCEMENTPERMATCH;
        private static readonly Lazy<Message> _DIEDBYCHANCE;
        private static readonly Lazy<Message> _DIEDOFSTARVING;
        private static readonly Lazy<Message> _DIEDOFREPLACINGT;
        private static readonly Lazy<Message> _DIEDOFREPLACINGF;
        private static readonly Lazy<Message> _RETURNED;
        private static readonly Lazy<Message> _RETURNEDHUNGRY;
        private static readonly Lazy<Message> _DIEDOFEATEN;

        public static IReadOnlyDictionary<string, Message> Dictionary = _dictionary;
        public static Message ALGORITHM => _ALGORITHM.Value;
        public static Message YES => _YES.Value;
        public static Message NO => _NO.Value;
        public static Message LOCATION => _LOCATION.Value;
        public static Message MATTER => _MATTER.Value;
        public static Message MATTER_ID => _MATTER_ID.Value;
        public static Message MATTER_VANISHING => _MATTER_VANISHING.Value;
        public static Message FOOD => _FOOD.Value;
        public static Message GENERATION => _GENERATION.Value;
        public static Message GENERATIONS => _GENERATIONS.Value;
        public static Message FOOD_ISEATEN => _FOOD_ISEATEN.Value;
        public static Message FOOD_ISRETURNED => _FOOD_ISRETURNED.Value;
        public static Message THING => _THING.Value;
        public static Message GENDER => _GENDER.Value;
        public static Message GENDER_NEUTRAL => _GENDER_NEUTRAL.Value;
        public static Message GENDER_FEMININE => _GENDER_FEMININE.Value;
        public static Message GENDER_MASCULINE => _GENDER_MASCULINE.Value;
        public static Message CHILDRENCOUNT => _CHILDRENCOUNT.Value;
        public static Message OTHERPARENT => _OTHERPARENT.Value;
        public static Message SPOUSES => _SPOUSES.Value;
        public static Message THING_SENSE => _THING_SENSE.Value;
        public static Message THING_DEATHRATE => _THING_DEATHRATE.Value;
        public static Message PARAMETERS_SPAWNRATE_THING => _PARAMETERS_SPAWNRATE_THING.Value;
        public static Message PARAMETERS_SPAWNRATE_FOOD => _PARAMETERS_SPAWNRATE_FOOD.Value;
        public static Message PARAMETERS_FEMININERATE => _PARAMETERS_FEMININERATE.Value;
        public static Message PARAMETERS_REPRODUCERATE_MIN => _PARAMETERS_REPRODUCERATE_MIN.Value;
        public static Message PARAMETERS_REPRODUCERATE_MAX => _PARAMETERS_REPRODUCERATE_MAX.Value;
        public static Message PARAMETERS_DEATHRATE_MIN => _PARAMETERS_DEATHRATE_MIN.Value;
        public static Message PARAMETERS_DEATHRATE_MAX => _PARAMETERS_DEATHRATE_MAX.Value;
        public static Message PARAMETERS_SENSE_MIN => _PARAMETERS_SENSE_MIN.Value;
        public static Message PARAMETERS_SENSE_MAX => _PARAMETERS_SENSE_MAX.Value;
        public static Message OPTIONS => _OPTIONS.Value;
        public static Message ALLMATTERS => _ALLMATTERS.Value;
        public static Message THINGS => _THINGS.Value;
        public static Message FOODS => _FOODS.Value;
        public static Message ALIVE => _ALIVE.Value;
        public static Message DEAD => _DEAD.Value;
        public static Message ALIVEDEAD => _ALIVEDEAD.Value;
        public static Message REPRODUCERATE => _REPRODUCERATE.Value;
        public static Message POPULATION => _POPULATION.Value;
        public static Message LIFELENGTH => _LIFELENGTH.Value;
        public static Message SPAWNED => _SPAWNED.Value;
        public static Message REPRODUCED => _REPRODUCED.Value;
        public static Message OLDGENERATION => _OLDGENERATION.Value;
        public static Message HUNGRY => _HUNGRY.Value;
        public static Message ATEAFTERBIRTH => _ATEAFTERBIRTH.Value;
        public static Message GOTFED => _GOTFED.Value;
        public static Message MOVED => _MOVED.Value;
        public static Message MOVECOUNT => _MOVECOUNT.Value;
        public static Message PARENT => _PARENT.Value;
        public static Message REPRODUCEMENTCOUNT => _REPRODUCEMENTCOUNT.Value;
        public static Message MULTIREPRODUCED => _MULTIREPRODUCED.Value;
        public static Message MULTIREPRODUCEMENTCOUNT => _MULTIREPRODUCEMENTCOUNT.Value;
        public static Message HUNGRYREPRODUCED => _HUNGRYREPRODUCED.Value;
        public static Message HUNGRYREPRODUCEMENTCOUNT => _HUNGRYREPRODUCEMENTCOUNT.Value;
        public static Message HUNGRYMULTIREPRODUCED => _HUNGRYMULTIREPRODUCED.Value;
        public static Message HUNGRYMULTIREPRODUCEMENTCOUNT => _HUNGRYMULTIREPRODUCEMENTCOUNT.Value;
        public static Message MATCHCOUNT => _MATCHCOUNT.Value;
        public static Message REPRODUCEMENTPERMATCH => _REPRODUCEMENTPERMATCH.Value;
        public static Message DIEDBYCHANCE => _DIEDBYCHANCE.Value;
        public static Message DIEDOFSTARVING => _DIEDOFSTARVING.Value;
        public static Message DIEDOFREPLACINGT => _DIEDOFREPLACINGT.Value;
        public static Message DIEDOFREPLACINGF => _DIEDOFREPLACINGF.Value;
        public static Message RETURNED => _RETURNED.Value;
        public static Message RETURNEDHUNGRY => _RETURNEDHUNGRY.Value;
        public static Message DIEDOFEATEN => _DIEDOFEATEN.Value;



        private static IEnumerable<(string CultureName, string MessageName, string Message)> GetMessages(string text)
        {
            return from line in text?.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                   where line.Length > 8
                   let sep1 = line.IndexOf("|")
                   where sep1 >= 5
                   let cultureName = line.Substring(0, sep1)
                   let culture = _allCultures.FirstOrDefault(y => y.Name.Equals(cultureName))
                   where culture != null
                   let sep2 = line.IndexOf(":", sep1 + 1)
                   where sep2 >= 0
                   let message = line.Substring(sep2 + 1).Trim()
                   where message.Length > 0
                   let name = line.Substring(6, line.Length - culture.Name.Length - message.Length - 2).Trim()
                   where name.Length > 0
                   select (culture.Name, name, message);
        }



        public static class ExceptionMessages
        {
            static ExceptionMessages()
            {
                _ARGUMENTTYPEINVALID = new Lazy<Message>(() => _dictionary["EXCEPTION_ARGUMENTTYPEINVALID"]);
                _ARGUMENTEMPTY = new Lazy<Message>(() => _dictionary["EXCEPTION_ARGUMENTEMPTY"]);
                _ARGUMENTEMPTYINNER = new Lazy<Message>(() => _dictionary["EXCEPTION_ARGUMENTEMPTYINNER"]);
                _ARGUMENTUNEXPECTEDINNER = new Lazy<Message>(() => _dictionary["EXCEPTION_ARGUMENTUNEXPECTEDINNER"]);
                _ARGUMENTVANISHED = new Lazy<Message>(() => _dictionary["EXCEPTION_ARGUMENTVANISHED"]);
                _ARGUMENTVANISHEDNOT = new Lazy<Message>(() => _dictionary["EXCEPTION_ARGUMENTVANISHEDNOT"]);
                _ARGUMENTEATEN = new Lazy<Message>(() => _dictionary["EXCEPTION_ARGUMENTEATEN"]);
                _ARGUMENTSPOUSESEQUAL = new Lazy<Message>(() => _dictionary["EXCEPTION_ARGUMENTSPOUSESEQUAL"]);
                _ARGUMENTPARENTNOT = new Lazy<Message>(() => _dictionary["EXCEPTION_ARGUMENTPARENTNOT"]);
                _ARGUMENTALREADYINSERTED = new Lazy<Message>(() => _dictionary["EXCEPTION_ARGUMENTALREADYINSERTED"]);
                _ARGUMENTGREATER = new Lazy<Message>(() => _dictionary["EXCEPTION_ARGUMENTGREATER"]);
                _ARGUMENTGREATERSUM = new Lazy<Message>(() => _dictionary["EXCEPTION_ARGUMENTGREATERSUM"]);
                _ARGUMENTUNIVERSEDIFFERENT = new Lazy<Message>(() => _dictionary["EXCEPTION_ARGUMENTUNIVERSEDIFFERENT"]);
                _ARGUMENTCANTREPRODUCE = new Lazy<Message>(() => _dictionary["EXCEPTION_ARGUMENTCANTREPRODUCE"]);
                _ARGUMENTHASSTATISTICS = new Lazy<Message>(() => _dictionary["EXCEPTION_ARGUMENTHASSTATISTICS"]);
                _NOTINITIALIZED = new Lazy<Message>(() => _dictionary["EXCEPTION_NOTINITIALIZED"]);
                _ALREADYATE = new Lazy<Message>(() => _dictionary["EXCEPTION_ALREADYATE"]);
                _ALREADYREPRODUCED = new Lazy<Message>(() => _dictionary["EXCEPTION_ALREADYREPRODUCED"]);
                _REFERENCESNOTSAME = new Lazy<Message>(() => _dictionary["EXCEPTION_REFERENCESNOTSAME"]);
                _TARGETOCUPPYING = new Lazy<Message>(() => _dictionary["EXCEPTION_TARGETOCUPPYING"]);
                _NOTRETURNED = new Lazy<Message>(() => _dictionary["EXCEPTION_NOTRETURNED"]);
                _VANISHED = new Lazy<Message>(() => _dictionary["EXCEPTION_VANISHED"]);
                _NOTEXISTS = new Lazy<Message>(() => _dictionary["EXCEPTION_NOTEXISTS"]);
            }


            private static readonly Lazy<Message> _ARGUMENTTYPEINVALID;
            private static readonly Lazy<Message> _ARGUMENTEMPTY;
            private static readonly Lazy<Message> _ARGUMENTEMPTYINNER;
            private static readonly Lazy<Message> _ARGUMENTUNEXPECTEDINNER;
            private static readonly Lazy<Message> _ARGUMENTVANISHED;
            private static readonly Lazy<Message> _ARGUMENTVANISHEDNOT;
            private static readonly Lazy<Message> _ARGUMENTEATEN;
            private static readonly Lazy<Message> _ARGUMENTSPOUSESEQUAL;
            private static readonly Lazy<Message> _ARGUMENTPARENTNOT;
            private static readonly Lazy<Message> _ARGUMENTALREADYINSERTED;
            private static readonly Lazy<Message> _ARGUMENTGREATER;
            private static readonly Lazy<Message> _ARGUMENTGREATERSUM;
            private static readonly Lazy<Message> _ARGUMENTUNIVERSEDIFFERENT;
            private static readonly Lazy<Message> _ARGUMENTCANTREPRODUCE;
            private static readonly Lazy<Message> _ARGUMENTHASSTATISTICS;
            private static readonly Lazy<Message> _NOTINITIALIZED;
            private static readonly Lazy<Message> _ALREADYATE;
            private static readonly Lazy<Message> _ALREADYREPRODUCED;
            private static readonly Lazy<Message> _REFERENCESNOTSAME;
            private static readonly Lazy<Message> _TARGETOCUPPYING;
            private static readonly Lazy<Message> _NOTRETURNED;
            private static readonly Lazy<Message> _VANISHED;
            private static readonly Lazy<Message> _NOTEXISTS;

            public static Message ARGUMENTTYPEINVALID => _ARGUMENTTYPEINVALID.Value;
            public static Message ARGUMENTEMPTY => _ARGUMENTEMPTY.Value;
            public static Message ARGUMENTEMPTYINNER => _ARGUMENTEMPTYINNER.Value;
            public static Message ARGUMENTUNEXPECTEDINNER => _ARGUMENTUNEXPECTEDINNER.Value;
            public static Message ARGUMENTVANISHED => _ARGUMENTVANISHED.Value;
            public static Message ARGUMENTVANISHEDNOT => _ARGUMENTVANISHEDNOT.Value;
            public static Message ARGUMENTEATEN => _ARGUMENTEATEN.Value;
            public static Message ARGUMENTSPOUSESEQUAL => _ARGUMENTSPOUSESEQUAL.Value;
            public static Message ARGUMENTPARENTNOT => _ARGUMENTPARENTNOT.Value;
            public static Message ARGUMENTALREADYINSERTED => _ARGUMENTALREADYINSERTED.Value;
            public static Message ARGUMENTGREATER => _ARGUMENTGREATER.Value;
            public static Message ARGUMENTGREATERSUM => _ARGUMENTGREATERSUM.Value;
            public static Message ARGUMENTUNIVERSEDIFFERENT => _ARGUMENTUNIVERSEDIFFERENT.Value;
            public static Message ARGUMENTCANTREPRODUCE => _ARGUMENTCANTREPRODUCE.Value;
            public static Message ARGUMENTHASSTATISTICS => _ARGUMENTHASSTATISTICS.Value;
            public static Message NOTINITIALIZED => _NOTINITIALIZED.Value;
            public static Message ALREADYATE => _ALREADYATE.Value;
            public static Message ALREADYREPRODUCED => _ALREADYREPRODUCED.Value;
            public static Message REFERENCESNOTSAME => _REFERENCESNOTSAME.Value;
            public static Message TARGETOCUPPYING => _TARGETOCUPPYING.Value;
            public static Message NOTRETURNED => _NOTRETURNED.Value;
            public static Message VANISHED => _VANISHED.Value;
            public static Message NOTEXISTS => _NOTEXISTS.Value;
        }
    }
}