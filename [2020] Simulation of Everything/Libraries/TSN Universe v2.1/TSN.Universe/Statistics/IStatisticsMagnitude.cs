using System;
using System.Runtime.Serialization;

namespace TSN.Universe.Statistics
{
    public interface IStatisticsMagnitude : ISerializable
    {
        MagnitudeTypes MagnitudeType { get; }
        IConvertible LocalMagnitudeNew { get; }
        IConvertible LocalMagnitudeAll { get; }
        IConvertible GeneralMagnitude { get; }
    }
}