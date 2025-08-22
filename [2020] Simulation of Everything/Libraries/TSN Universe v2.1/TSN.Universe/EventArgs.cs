using System;
using TSN.Universe.Matters;
using TSN.Universe.Matters.Foods;
using TSN.Universe.Statistics;
using TSN.Universe.Matters.Things;

namespace TSN.Universe
{
    public class SimulationEventArgs : EventArgs
    {
        internal SimulationEventArgs(uint generation)
        {
            _generation = generation;
        }


        private readonly uint _generation;

        public uint Generation => _generation;
    }
    public class SimulatedEventArgs : SimulationEventArgs
    {
        internal SimulatedEventArgs(uint generation, int count) :
            base(generation)
        {
            _count = count;
        }


        private readonly int _count;

        public int Count => _count;
    }
    public class CalculatedStatisticsEventArgs : SimulationEventArgs
    {
        public CalculatedStatisticsEventArgs(uint generation, StatisticsEntity statistics) :
            base(generation)
        {
            if (statistics == null)
                throw new ArgumentNullException(nameof(statistics));
            _statistics = statistics;
        }


        private readonly StatisticsEntity _statistics;

        public StatisticsEntity Statistics => _statistics;
    }
    public class SimulationErrorOccuredEventArgs : SimulationEventArgs
    {
        public SimulationErrorOccuredEventArgs(uint generation, Exception exception, byte rank) :
            base(generation)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));
            _exception = exception;
            _rank = rank;
            CancelThrow = false;
        }


        private readonly Exception _exception;
        private readonly byte _rank;

        public Exception Exception => _exception;
        public byte Rank => _rank;
        public bool CancelThrow { get; set; }
    }
    public class InsertingEventArgs : MatterEventArgs<Matter>
    {
        internal InsertingEventArgs(Matter matter, Location location) :
            base(matter)
        {
            _location = location;
        }


        private readonly Location _location;

        public Location Location => _location;
    }
    public class KillingThingsEventArgs : SimulationEventArgs
    {
        internal KillingThingsEventArgs(uint generation, bool exceptNewBorns) :
            base(generation)
        {
            _exceptNewBorns = exceptNewBorns;
        }


        private readonly bool _exceptNewBorns;

        public bool ExceptNewBorns => _exceptNewBorns;
    }
    public class KilledThingsEventArgs : SimulatedEventArgs
    {
        internal KilledThingsEventArgs(uint generation, bool exceptNewBorns, int count) :
            base(generation, count)
        {
            _exceptNewBorns = exceptNewBorns;
        }


        private readonly bool _exceptNewBorns;

        public bool ExceptNewBorns => _exceptNewBorns;
    }
    public class VanishingEventArgs : EventArgs
    {
        internal VanishingEventArgs(uint? vanishingGeneration)
        {
            _vanishingGeneration = vanishingGeneration;
        }


        private readonly uint? _vanishingGeneration;

        public uint? VanishingGeneration => _vanishingGeneration;
    }
    public class MovingEventArgs : EventArgs
    {
        internal MovingEventArgs(Location initialLocation, Location targetLocation)
        {
            if (initialLocation.Equals(targetLocation))
                throw new ArgumentException();
            _initialLocation = initialLocation;
            _targetLocation = targetLocation;
        }


        private readonly Location _initialLocation;
        private readonly Location _targetLocation;

        public Location InitialLocation => _initialLocation;
        public Location TargetLocation => _targetLocation;
    }
    public class MatterEventArgs<TMatter> : EventArgs
        where TMatter : Matter
    {
        internal MatterEventArgs(TMatter matter)
        {
            _matter = matter ?? throw new ArgumentNullException(nameof(matter));
        }


        private readonly TMatter _matter;

        public TMatter Matter => _matter;
    }
    public class FedEventArgs : MatterEventArgs<Thing>
    {
        internal FedEventArgs(Thing thing, Food food) :
            base(thing)
        {
            _food = food ?? throw new ArgumentNullException(nameof(_food));
        }


        private readonly Food _food;

        public Food Food => _food;
    }
    public class RevokeChangesEventArgs : MatterEventArgs<Matter>
    {
        public RevokeChangesEventArgs(Matter matter, uint generation, uint count)
            : base(matter)
        {
            _generation = generation;
            _count = count;
        }


        private readonly uint _generation;
        private readonly double _count;

        public uint Generation => _generation;
        public double Count => _count;
    }
}