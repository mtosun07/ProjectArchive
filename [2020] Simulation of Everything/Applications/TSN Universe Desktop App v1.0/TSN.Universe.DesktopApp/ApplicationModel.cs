using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace TSN.Universe.DesktopApp
{
    [Serializable] internal sealed class ApplicationModel : ISerializable
    {
        public ApplicationModel()
        {
            lock (_locker)
            {
                _libraryInfo = Assembly.GetAssembly(typeof(Universe)).GetName();
                _universe = null;
                _entries = new Dictionary<uint, ConsoleEntryList>();
            }
        }
        private ApplicationModel(SerializationInfo info, StreamingContext context)
        {
            lock (_locker)
            {
                if (info == null)
                    throw new ArgumentNullException(nameof(info));
                var libraryInfo = (AssemblyName)info.GetValue(Field_LibraryInfo, typeof(AssemblyName));
                var universe = (Universe)info.GetValue(Field_Universe, typeof(Universe));
                var entries = (Dictionary<uint, ConsoleEntryList>)info.GetValue(Field_Entries, typeof(Dictionary<uint, ConsoleEntryList>));
                _libraryInfo = libraryInfo;
                _universe = universe;
                _entries = entries;
                if (universe != null)
                    _universeIllustrator = new Utility.UniverseIllustrator(universe);
            }
        }


        private const string Field_LibraryInfo = "LibraryInfo";
        private const string Field_Universe = "Universe";
        private const string Field_Entries = "Entries";
        public const string FileExtension_Universe = ".soeu.bin";
        public const string FileExtension_Parameters = ".soep.bin";
        public const string FileExtension_Charts = ".soec.bin";

        private static readonly object _locker = new object();

        private readonly AssemblyName _libraryInfo;
        private Universe _universe;
        private readonly Dictionary<uint, ConsoleEntryList> _entries;
        private Utility.UniverseIllustrator _universeIllustrator;

        public Universe Universe
        {
            get
            {
                lock (_locker)
                    return _universe;
            }
            set
            {
                Clear(value);
                UniverseChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public IReadOnlyDictionary<uint, ConsoleEntryList> Entries
        {
            get
            {
                lock (_locker)
                    return _entries;
            }
        }
        public Utility.UniverseIllustrator UniverseIllustrator
        {
            get
            {
                lock (_locker)
                    return _universeIllustrator;
            }
        }

        public event EventHandler<EventArgs> UniverseChanged;
        public event EventHandler<ConsoleEntryAddedEventArgs> ConsoleEntryAdded;



        private void AddEntry(ConsoleEntry entry)
        {
            if (_entries.ContainsKey(entry.Generation))
                _entries[entry.Generation].Add(entry);
            else
                _entries.Add(entry.Generation, new ConsoleEntryList { entry });
            ConsoleEntryAdded?.Invoke(this, new ConsoleEntryAddedEventArgs(entry));
        }
        
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(Field_LibraryInfo, _libraryInfo, typeof(AssemblyName));
            info.AddValue(Field_Universe, _universe, typeof(Universe));
            info.AddValue(Field_Entries, _entries, typeof(Dictionary<uint, ConsoleEntryList>));
        }
        private void Clear(Universe universe)
        {
            lock (_locker)
            {
                _entries.Clear();
                if ((_universe = universe) != null)
                {
                    _universe.FedThing += Universe_FedThing;
                    _universe.FedThings += Universe_FedThings;
                    _universe.FeedingThings += Universe_FeedingThings;
                    _universe.Initialized += Universe_Initialized;
                    _universe.Initializing += Universe_Initializing;
                    _universe.Inserted += Universe_Inserted;
                    _universe.Inserting += Universe_Inserting;
                    _universe.KilledFood += Universe_KilledFood;
                    _universe.KilledFoods += Universe_KilledFoods;
                    _universe.KilledThing += Universe_KilledThing;
                    _universe.KilledThings += Universe_KilledThings;
                    _universe.KillingFoods += Universe_KillingFoods;
                    _universe.KillingThings += Universe_KillingThings;
                    _universe.MadeThingHungry += Universe_MadeThingHungry;
                    _universe.MadeThingsHungry += Universe_MadeThingsHungry;
                    _universe.MakingThingsHungry += Universe_MakingThingsHungry;
                    _universe.ReproducedThing += Universe_ReproducedThing;
                    _universe.ReproducedThings += Universe_ReproducedThings;
                    _universe.ReproducingThings += Universe_ReproducingThings;
                    _universe.Simulated += Universe_Simulated;
                    _universe.Simulating += Universe_Simulating;
                    _universe.SpawnedFood += Universe_SpawnedFood;
                    _universe.SpawnedFoods += Universe_SpawnedFoods;
                    _universe.SpawnedThing += Universe_SpawnedThing;
                    _universe.SpawnedThings += Universe_SpawnedThings;
                    _universe.SpawningFoods += Universe_SpawningFoods;
                    _universe.SpawningThings += Universe_SpawningThings;
                    _universe.SimulationErrorOccured += Universe_SimulationErrorOccured;
                    _universe.CalculatingStatistics += Universe_CalculatingStatistics;
                    _universe.CalculatedStatistics += Universe_CalculatedStatistics;
                    _universe.RevokingChanges += Universe_RevokingChanges;
                    _universe.RevokedChanges += Universe_RevokedChanges;
                    _universe.RevokedAllChanges += Universe_RevokedAllChanges;
                    _universeIllustrator = new Utility.UniverseIllustrator(universe);
                }
            }
            GC.Collect();
        }
        public AssemblyName GetLibraryInfo() => (AssemblyName)_libraryInfo.Clone();

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            GetObjectData(info, context);
        }

        private void Universe_Simulating(object sender, SimulationEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Universe is getting simulated."));
        private void Universe_SimulationErrorOccured(object sender, SimulationErrorOccuredEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"An error occured: {e.Exception.Message}"));
        private void Universe_CalculatedStatistics(object sender, CalculatedStatisticsEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Statistics were calculated. (Action #: {Program.ApplicationModel.Entries[e.Generation].Count + 1})"));
        private void Universe_RevokedAllChanges(object sender, SimulationEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"All changes were revoked."));
        private void Universe_RevokingChanges(object sender, RevokeChangesEventArgs e) => AddEntry(new ConsoleEntry(((Universe)sender).CurrentGeneration.Value, DateTimeOffset.Now, $"Changes are getting revoked on #{e.Count}: {e.Matter}"));
        private void Universe_RevokedChanges(object sender, RevokeChangesEventArgs e) => AddEntry(new ConsoleEntry(((Universe)sender).CurrentGeneration.Value, DateTimeOffset.Now, $"Changes were revoked on #{e.Count}: {e.Matter}"));
        private void Universe_Simulated(object sender, SimulationEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Universe were simulated."));
        private void Universe_CalculatingStatistics(object sender, SimulationEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Statistics are getting calculated."));
        private void Universe_Initializing(object sender, SimulationEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Universe is getting initialized."));
        private void Universe_Initialized(object sender, SimulationEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Universe was initialized."));
        private void Universe_Inserting(object sender, InsertingEventArgs e) => AddEntry(new ConsoleEntry(((Universe)sender).CurrentGeneration.Value, DateTimeOffset.Now, $"Matter is getting inserted at location {e.Location}: {e.Matter}"));
        private void Universe_Inserted(object sender, InsertingEventArgs e) => AddEntry(new ConsoleEntry(((Universe)sender).CurrentGeneration.Value, DateTimeOffset.Now, $"Matter was inserted at location {e.Location}: {e.Matter}"));
        private void Universe_SpawningThings(object sender, SimulationEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Things are getting spawned."));
        private void Universe_SpawnedThings(object sender, SimulatedEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Things were spawned. (Count: {e.Count})"));
        private void Universe_SpawnedThing(object sender, MatterEventArgs<Thing> e) => AddEntry(new ConsoleEntry(((Universe)sender).CurrentGeneration.Value, DateTimeOffset.Now, $"Thing was spawned: {e.Matter}"));
        private void Universe_SpawningFoods(object sender, SimulationEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Foods are getting spawned."));
        private void Universe_SpawnedFoods(object sender, SimulatedEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Foods were spawned. (Count: {e.Count})"));
        private void Universe_SpawnedFood(object sender, MatterEventArgs<Food> e) => AddEntry(new ConsoleEntry(((Universe)sender).CurrentGeneration.Value, DateTimeOffset.Now, $"Food was spawned: {e.Matter}"));
        private void Universe_FeedingThings(object sender, SimulationEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Things are getting fed."));
        private void Universe_FedThings(object sender, SimulatedEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Things were fed. (Count: {e.Count})"));
        private void Universe_FedThing(object sender, FedEventArgs e) => AddEntry(new ConsoleEntry(((Universe)sender).CurrentGeneration.Value, DateTimeOffset.Now, $"Thing was fed with {e.Food}: {e.Matter}"));
        private void Universe_ReproducingThings(object sender, SimulationEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Things are getting reproduced."));
        private void Universe_ReproducedThings(object sender, SimulatedEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Things were reproduced. (Count: {e.Count})"));
        private void Universe_ReproducedThing(object sender, MatterEventArgs<Thing> e) => AddEntry(new ConsoleEntry(((Universe)sender).CurrentGeneration.Value, DateTimeOffset.Now, $"Thing was reproduced: {e.Matter}"));
        private void Universe_MakingThingsHungry(object sender, SimulationEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Things are getting made hungry."));
        private void Universe_MadeThingsHungry(object sender, SimulatedEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Things were made hungry. (Count: {e.Count})"));
        private void Universe_MadeThingHungry(object sender, MatterEventArgs<Thing> e) => AddEntry(new ConsoleEntry(((Universe)sender).CurrentGeneration.Value, DateTimeOffset.Now, $"Thing was made hungry: {e.Matter}"));
        private void Universe_KillingThings(object sender, KillingThingsEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Things are getting killed."));
        private void Universe_KilledThings(object sender, SimulatedEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Things were killed. (Count: {e.Count})"));
        private void Universe_KilledThing(object sender, MatterEventArgs<Thing> e) => AddEntry(new ConsoleEntry(((Universe)sender).CurrentGeneration.Value, DateTimeOffset.Now, $"Thing was killed: {e.Matter}"));
        private void Universe_KillingFoods(object sender, SimulationEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Foods are getting killed."));
        private void Universe_KilledFoods(object sender, SimulatedEventArgs e) => AddEntry(new ConsoleEntry(e.Generation, DateTimeOffset.Now, $"Foods were killed. (Count: {e.Count})"));
        private void Universe_KilledFood(object sender, MatterEventArgs<Food> e) => AddEntry(new ConsoleEntry(((Universe)sender).CurrentGeneration.Value, DateTimeOffset.Now, $"Food was killed: {e.Matter}"));



        public class ConsoleEntryAddedEventArgs : EventArgs
        {
            public ConsoleEntryAddedEventArgs(ConsoleEntry consoleEntry) => _consoleEntry = consoleEntry;


            private readonly ConsoleEntry _consoleEntry;

            internal ConsoleEntry ConsoleEntry => _consoleEntry;
        }
    }
}