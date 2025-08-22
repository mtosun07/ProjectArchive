using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using static TSN.Universe.Localization.Messages;

namespace TSN.Universe.Localization
{
    internal sealed class Message : IReadOnlyDictionary<CultureInfo, string>
    {
        public Message(string cultureName, string message, params (string CultureName, string Message)[] languages)
        {
            if (cultureName?.Equals(string.Empty) ?? throw new ArgumentNullException(nameof(cultureName)))
                throw new ArgumentException(ExceptionMessages.ARGUMENTEMPTY, nameof(cultureName));
            if (message?.Equals(string.Empty) ?? throw new ArgumentNullException(nameof(message)))
                throw new ArgumentException(ExceptionMessages.ARGUMENTEMPTY, nameof(message));
            if (languages?.Any(x => x.CultureName.Equals(string.Empty) || x.Message.Equals(string.Empty)) ?? false)
                throw new ArgumentException(ExceptionMessages.ARGUMENTEMPTYINNER, nameof(languages));
            var culture = CultureInfo.GetCultureInfo(cultureName);
            _invariantCultureName = culture.Name;
            var raw = new (CultureInfo Culture, string Message)[] { (CultureInfo.InvariantCulture, message), (culture, message) };
            if (languages != null)
                raw = raw.Concat(languages.Select(x => (CultureInfo.GetCultureInfo(x.CultureName), x.Message))).ToArray();
            _messages = new ReadOnlyDictionary<CultureInfo, string>(raw.ToDictionary(x => x.Culture, x => x.Message));
        }



        private static CultureInfo _currentCulture = CultureInfo.CurrentCulture;

        private readonly string _invariantCultureName;
        private readonly ReadOnlyDictionary<CultureInfo, string> _messages;

        public static CultureInfo CurrentCulture
        {
            get => _currentCulture;
            set
            {
                if (value != null)
                    _currentCulture = value;
            }
        }
        
        public string this[CultureInfo key] => _messages.TryGetValue(key, out var value) ? value : MessageInvariant;

        public string InvariantCultureName => _invariantCultureName;
        public string MessageInvariant => _messages[CultureInfo.InvariantCulture];
        public int Count => _messages.Count;
        public IEnumerable<CultureInfo> Keys => _messages.Keys;
        public IEnumerable<string> Values => _messages.Values;

        public bool ContainsKey(CultureInfo key) => _messages.ContainsKey(key);
        public bool TryGetValue(CultureInfo key, out string value) => _messages.TryGetValue(key, out value);
        public IEnumerator<KeyValuePair<CultureInfo, string>> GetEnumerator() => _messages.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _messages.GetEnumerator();

        public static implicit operator string(Message em) => em._messages[_currentCulture];
    }
}