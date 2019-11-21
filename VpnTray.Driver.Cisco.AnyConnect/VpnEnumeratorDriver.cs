using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using VpnTray.Domain;

namespace VpnTray.Driver.Cisco.AnyConnect
{
    public class VpnEnumeratorDriver : IVpnEnumeratorDriver
    {
        private static readonly string PROFILES_PATH = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), 
            "Cisco", 
            "Cisco AnyConnect Secure Mobility Client", 
            "Profile");

        private const string XML_NAMESPACE = "http://schemas.xmlsoap.org/encoding/";

        public Task<Vpn[]> GetVpns()
        {
            var vpns = new List<Vpn>();
            var profileFiles = Directory.GetFiles(PROFILES_PATH, "*.xml");
            foreach (var file in profileFiles)
            {
                var document = XDocument.Load(file);
                var serverList = document.Root.Element(XName.Get("ServerList", XML_NAMESPACE));
                var hosts = serverList.Elements(XName.Get("HostEntry", XML_NAMESPACE));
                foreach (var entry in hosts)
                {
                    var id = new VpnId
                    {
                        Name = entry.Element(XName.Get("HostName", XML_NAMESPACE)).Value,
                        Server = entry.Element(XName.Get("HostAddress", XML_NAMESPACE)).Value
                    };
                    vpns.Add(new Vpn(id.ToString(), id.Name));
                }
            }
            return Task.FromResult(vpns.ToArray());
        }
    }
}
