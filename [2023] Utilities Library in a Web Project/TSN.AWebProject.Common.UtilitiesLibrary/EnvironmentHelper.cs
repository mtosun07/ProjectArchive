using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace TSN.AWebProject.Common.UtilitiesLibrary
{
    public static class EnvironmentHelper
    {
        static EnvironmentHelper()
        {
            _keysCpu = new HashSet<string> { "UniqueId", "SerialNumber", "ProcessorId", "Name", "Caption", "Description", "Manufacturer" };
            _keysBaseBoard = new HashSet<string> { "Model", "OtherIdentifyingInfo", "Product", "SerialNumber", "Manufacturer" };
        }


        private const string _emptyString = "To Be Filled By O.E.M.";
        private const string _pathCpu = "win32_processor";
        private const string _pathBaseBoard = "Win32_BaseBoard";

        private static readonly HashSet<string> _keysCpu;
        private static readonly HashSet<string> _keysBaseBoard;




        private static IEnumerable<KeyValuePair<string, string>> GetManagementClassValues(string path)
        {
            using (var man = new ManagementClass(path))
            using (var moc = man.GetInstances())
                foreach (var obj in moc)
                {
                    var enumerator = obj.Properties.GetEnumerator();
                    if (enumerator.Current != null && !enumerator.Current.IsArray && enumerator.Current.Value is string str && !string.IsNullOrWhiteSpace(str) && !str.Equals(_emptyString))
                        yield return new KeyValuePair<string, string>(enumerator.Current.Name, str);
                }
        }

        public static IDictionary<string, string> GetCPUIdentifiers() => GetManagementClassValues(_pathCpu).Where(x => _keysCpu.Contains(x.Key)).DistinctBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        public static IDictionary<string, string> GetBaseBoardIdentifiers() => GetManagementClassValues(_pathBaseBoard).Where(x => _keysBaseBoard.Contains(x.Key)).DistinctBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
    }
}