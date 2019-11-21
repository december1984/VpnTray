using System;
using System.Collections.Generic;
using System.Text;

namespace VpnTray.Driver.Cisco.AnyConnect
{
    class VpnId
    {
        public string Name { get; set; }
        public string Server { get; set; }
        public override string ToString()
        {
            return $"{Name}:{Server}";
        }

        public static VpnId Parse(string s)
        {
            var parts = s.Split(':');
            return new VpnId
            {
                Name = parts[0],
                Server = parts[1]
            };
        }
    }
}
