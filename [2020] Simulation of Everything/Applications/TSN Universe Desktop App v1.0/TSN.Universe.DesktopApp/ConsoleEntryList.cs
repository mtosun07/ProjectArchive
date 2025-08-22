using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace TSN.Universe.DesktopApp
{
    [Serializable] internal sealed class ConsoleEntryList : IList<ConsoleEntry>, IEquatable<ConsoleEntryList>, ISerializable
    {
        public ConsoleEntryList()
        {
            _list = new List<ConsoleEntry>();
            _text = string.Empty;
        }
        private ConsoleEntryList(SerializationInfo info, StreamingContext context) : base()
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            var count = info.GetInt32(Field_Count);
            var text = info.GetString(Field_Text);
            var list = new List<ConsoleEntry>(count);
            for (int i = 0; i < count; i++)
                list.Add((ConsoleEntry)info.GetValue(string.Format(FieldFormat_Item, i), typeof(ConsoleEntry)));
            _list = list;
            _text = text;
        }


        private const string Field_Count = "Count";
        private const string FieldFormat_Item = "Item[{0}]";
        private const string Field_Text = "Text";

        private readonly List<ConsoleEntry> _list;
        private string _text;

        public ConsoleEntry this[int index] { get => _list[index]; set => _list[index] = value; }
        public int Count => _list.Count;
        public bool IsReadOnly => false;
        public uint? Generation => _list.Count == 0 ? (uint?)null : _list[0].Generation;



        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(Field_Count, _list.Count);
            for (int i = 0; i < _list.Count; i++)
                info.AddValue(string.Format(FieldFormat_Item, i), _list[i], typeof(ConsoleEntry));
            info.AddValue(Field_Text, _text);
        }
        public void TrimExcess() => _list.TrimExcess();

        public override string ToString() => _text;
        public override int GetHashCode() => _text.GetHashCode();
        public override bool Equals(object obj) => Equals(obj as ConsoleEntryList);

        public bool Equals(ConsoleEntryList other) => other != null && _list.Count == other._list.Count && _text.Equals(other._text);
        public void Add(ConsoleEntry item)
        {
            if (_list.Count > 0 && _list[0].Generation != item.Generation)
                throw new ArgumentOutOfRangeException(nameof(item));
            string text = null;
            if (ConsoleEntry.IsEmpty(item) || string.IsNullOrEmpty(text = item.ToString().Trim()))
                throw new ArgumentException("Argument was having empty value.", nameof(item));
            _list.Add(item);
            _text += $"{Environment.NewLine}{text}";
        }
        public void Insert(int index, ConsoleEntry item) => throw new NotSupportedException();
        public void RemoveAt(int index) => throw new NotSupportedException();
        public bool Remove(ConsoleEntry item) => throw new NotSupportedException();
        public void Clear()
        {
            _text = null;
            _list.Clear();
            _list.TrimExcess();
        }
        public int IndexOf(ConsoleEntry item) => _list.IndexOf(item);
        public bool Contains(ConsoleEntry item) => _list.Contains(item);
        public void CopyTo(ConsoleEntry[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
        public IEnumerator<ConsoleEntry> GetEnumerator() => ((IList<ConsoleEntry>)_list).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IList<ConsoleEntry>)_list).GetEnumerator();
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            GetObjectData(info, context);
        }
    }
}