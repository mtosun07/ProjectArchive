using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using TSN.Hashing;
using TSN.Utility.Entities;
using TSN.Utility.EqualityComparers;
using TSN.Utility.Extensions;

namespace TSN.Utility.WebHelper
{
    [Serializable()] [NativeHashable()]
    public sealed class ReCaptchaV2 : EntityBase<ReCaptchaV2>
    {
        public ReCaptchaV2(string token, string encodedResponse, string remoteIp)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));
            if (string.IsNullOrEmpty(encodedResponse))
                throw new ArgumentNullException(nameof(encodedResponse));
            if (string.IsNullOrEmpty(remoteIp))
                throw new ArgumentNullException(remoteIp);
            _token = token;
            _encodedResponse = encodedResponse;
            _remoteIp = remoteIp;
            string data = null;
            var request = (HttpWebRequest)WebRequest.Create(string.Format(_verificationUriFormat, token, encodedResponse, remoteIp));
            request.Method = "GET";
            using (var response = (HttpWebResponse)(request.GetResponse()))
            {
                if (response.StatusCode == HttpStatusCode.OK && response.ContentType == "application/json; charset=utf-8")
                    using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        data = reader.ReadToEnd();
                        reader.Close();
                    }
                response.Close();
            }
            if (!string.IsNullOrEmpty(data))
            {
                var json = JObject.Parse(data);
                _isSuccess = (bool)json["success"];
                _challenge = DateTime.Parse(json["challenge_ts"].ToString());
                _hostName = (string)json["hostname"];
                _errorCodes = (json["errorcodes"]?.Select(x =>
                {
                    var s = (string)x;
                    switch (s)
                    {
                        case "missing-input-secret":
                            return ErrorCodeValues.MissingInputSecret;
                        case "invalid-input-secret":
                            return ErrorCodeValues.InvalidInputSecret;
                        case "missing-input-response":
                            return ErrorCodeValues.MissingInputResponse;
                        case "invalid-input-response":
                            return ErrorCodeValues.InvalidInputResponse;
                        case "bad-request":
                            return ErrorCodeValues.BadRequest;
                        default:
                            throw new InvalidOperationException();
                    }
                }).ToList() ?? new List<ErrorCodeValues>(0)).AsReadOnly();
            }
            else
                throw new InvalidOperationException();
        }
        private ReCaptchaV2(ReCaptchaV2 otherInstance)
        {
            if (otherInstance == null)
                throw new ArgumentNullException(nameof(otherInstance));
            _token = otherInstance._token.Clone<string>();
            _encodedResponse = otherInstance._encodedResponse.Clone<string>();
            _remoteIp = otherInstance._remoteIp.Clone<string>();
            _isSuccess = otherInstance._isSuccess;
            _challenge = otherInstance._challenge;
            _hostName = otherInstance._hostName.Clone<string>();
            _errorCodes = _errorCodes.CloneStructSequence().ToList().AsReadOnly();
        }
        private ReCaptchaV2(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            var token = info.GetString(TokenField);
            var encodedResponse = info.GetString(EncodedResponseField);
            var remoteIp = info.GetString(RemoteIpField);
            var isSuccess = info.GetBoolean(IsSuccessField);
            var challenge = info.GetDateTime(ChallengeField);
            var hostName = info.GetString(HostNameField);
            var errorCodes = info.GetCollectionValues<ErrorCodeValues>(ErrorCodesField).ToList().AsReadOnly();
            _token = token;
            _encodedResponse = encodedResponse;
            _remoteIp = remoteIp;
            _isSuccess = isSuccess;
            _challenge = challenge;
            _hostName = hostName;
            _errorCodes = errorCodes;
        }


        private const string _verificationUriFormat = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}&remoteip={1}";
        private const string TokenField = "Token";
        private const string EncodedResponseField = "EncodedResponse";
        private const string RemoteIpField = "RemoteIp";
        private const string IsSuccessField = "IsSuccess";
        private const string ChallengeField = "Challenge";
        private const string HostNameField = "HostName";
        private const string ErrorCodesField = "ErrorCodes";

        private readonly string _token;
        private readonly string _encodedResponse;
        private readonly string _remoteIp;
        private readonly bool _isSuccess;
        private readonly DateTime _challenge;
        private readonly string _hostName;
        private readonly ReadOnlyCollection<ErrorCodeValues> _errorCodes;

        public string Token => _token;
        public string EncodedResponse => _encodedResponse;
        public string RemoteIp => _remoteIp;
        public bool IsSuccess => _isSuccess;
        public DateTime Challenge => _challenge;
        public string HostName => _hostName;
        public IReadOnlyCollection<ErrorCodeValues> ErrorCodes => _errorCodes;


        protected sealed override object[] GetHashCodesOf()
        {
            return new object[] { _token, _encodedResponse, _remoteIp, _isSuccess, _challenge, _hostName, CollectionEqualityComparer<ErrorCodeValues>.DefaultByFNV.GetHashCode(_errorCodes) };
        }
        protected sealed override bool EqualsMemberwise(ReCaptchaV2 other)
        {
            return _token.Equals(other._token) && _encodedResponse.Equals(other._encodedResponse) && _remoteIp.Equals(other._remoteIp) && _isSuccess == other._isSuccess && _challenge.Equals(other._challenge) && _hostName.Equals(other._hostName) && CollectionEqualityComparer<ErrorCodeValues>.DefaultByFNV.Equals(_errorCodes, other._errorCodes);
        }
        public sealed override string ToString()
        {
            return JsonConvert.SerializeObject(new { Token, EncodedResponse, RemoteIp, IsSuccess, Challenge, HostName, ErrorCodes });
        }
        public sealed override object Clone()
        {
            return new ReCaptchaV2(this);
        }
        public sealed override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue<string>(TokenField, _token);
            info.AddValue<string>(EncodedResponseField, _encodedResponse);
            info.AddValue<string>(RemoteIpField, _remoteIp);
            info.AddValue(IsSuccessField, _isSuccess);
            info.AddValue(ChallengeField, _challenge);
            info.AddValue<string>(HostNameField, _hostName);
            info.AddCollectionValues(ErrorCodesField, _errorCodes);
        }



        public enum ErrorCodeValues : byte
        {
            MissingInputSecret = 1,
            InvalidInputSecret = 2,
            MissingInputResponse = 3,
            InvalidInputResponse = 4,
            BadRequest = 5
        }
    }
}