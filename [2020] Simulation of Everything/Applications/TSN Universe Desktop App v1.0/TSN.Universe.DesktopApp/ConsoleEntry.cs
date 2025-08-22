using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace TSN.Universe.DesktopApp
{
    [Serializable] internal struct ConsoleEntry : IEquatable<ConsoleEntry>, IComparable<ConsoleEntry>, IComparable, ISerializable
    {
        static ConsoleEntry()
        {
            _dateTimeFormat = "o";
            _seperator = " >>\t";
        }
        public ConsoleEntry(uint generation, DateTimeOffset dateTime, string message)
        {
            _generation = generation;
            _dateTime = dateTime;
            _message = message?.Trim() ?? string.Empty;
        }
        private ConsoleEntry(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            var generation = info.GetUInt32(Field_Generation);
            var dateTime = (DateTimeOffset)info.GetValue(Field_DateTime, typeof(DateTimeOffset));
            var message = info.GetString(Field_Message);
            _generation = generation;
            _dateTime = dateTime;
            _message = message;
        }


        private const string Field_Generation = "Generation";
        private const string Field_DateTime = "DateTime";
        private const string Field_Message = "Message";


        private static string _dateTimeFormat;
        private static string _seperator;

        private readonly uint _generation;
        private readonly DateTimeOffset _dateTime;
        private readonly string _message;

        public static string DateTimeFormat
        {
            get => _dateTimeFormat;
            set => _dateTimeFormat = value ?? string.Empty;
        }
        public static string Seperator
        {
            get => _seperator;
            set => _seperator = value ?? string.Empty;
        }

        public DateTimeOffset DateTime => _dateTime;
        public string Message => _message;
        public uint Generation => _generation;



        public static bool IsEmpty(ConsoleEntry ce) => ce._dateTime.Equals(new DateTimeOffset());

        public override string ToString() => $"{_dateTime.ToString(_dateTimeFormat)} ({Field_Generation}: {_generation}){_seperator}{_message}";
        public override int GetHashCode() => _dateTime.GetHashCode();
        public override bool Equals(object obj) => obj != null && obj is ConsoleEntry ce && Equals(ce);

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(Field_Generation, _generation);
            info.AddValue(Field_DateTime, _dateTime, typeof(DateTimeOffset));
            info.AddValue(Field_Message, _message);
        }
        public bool Equals(ConsoleEntry other) => other._generation == _generation && other._dateTime.Equals(_dateTime) && other._message.Equals(_message);
        public int CompareTo(object obj) => obj == null ? throw new ArgumentNullException(nameof(obj)) : (obj is ConsoleEntry ce ? CompareTo(ce) : throw new ArgumentException("Argument was not of the type expected."));
        public int CompareTo(ConsoleEntry other)
        {
            var result = _dateTime.CompareTo(other._dateTime);
            return result != 0 ? result : _generation.CompareTo(other._generation);
        }
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            GetObjectData(info, context);
        }
    }
}