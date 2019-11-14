using System.Runtime.InteropServices;

namespace VpnTray.Driver.WinApi
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct RasProjectionInfo
    {
        public RASAPIVERSION version;
        public RASPROJECTION_INFO_TYPE type;

        // Based on the connectionFlags, it should use appropriate projection info 
        //union {
        public RASPPP_PROJECTION_INFO ppp;
        //    RASIKEV2_PROJECTION_INFO ikev2;
        //};
    }
    public enum RASAPIVERSION
    {
        _500 = 1,
        _501,
        _600,
        _601,
    }

    public enum RASPROJECTION_INFO_TYPE
    {
        PPP = 1,
        IKEv2,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct RASPPP_PROJECTION_INFO
    {

        // IPv4 Projection Parameters
        public int IPv4NegotiationError;
        public long IPv4Address;
        public long IPv4ServerAddress;
        public int IPv4Options;
        public int IPv4ServerOptions;

        // IPv6 Projection Parameters
        public int IPv6NegotiationError;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] InterfaceIdentifier;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] ServerInterfaceIdentifier;

        // LCP Options
        public bool Bundled;
        public bool Multilink;
        public int AuthenticationProtocol;
        public int AuthenticationData;
        public int ServerAuthenticationProtocol;
        public int ServerAuthenticationData;
        public int EapTypeId;
        public int ServerEapTypeId;
        public int LcpOptions;
        public int LcpServerOptions;

        // CCP options
        public int CcpError;
        public int CcpCompressionAlgorithm;
        public int CcpServerCompressionAlgorithm;
        public int CcpOptions;
        public int CcpServerOptions;
    }

    //    RASIKEV2_PROJECTION_INFO {

    //    // IPv4 Projection Parameters
    //    public int IPv4NegotiationError;
    //    RASIPV4ADDR ipv4Address;
    //    RASIPV4ADDR ipv4ServerAddress;

    //// IPv6 Projection Parameters
    //    public int IPv6NegotiationError;
    //    RASIPV6ADDR ipv6Address;
    //    RASIPV6ADDR ipv6ServerAddress;
    //    public int PrefixLength;

    //// AUTH
    //    public int AuthenticationProtocol;
    //    public int EapTypeId;

    //    public int Flags;
    //    public int EncryptionMethod;

    //    DWORD numIPv4ServerAddresses;
    //    RASIPV4ADDR* ipv4ServerAddresses;
    //    DWORD numIPv6ServerAddresses;
    //    RASIPV6ADDR* ipv6ServerAddresses;
    //    }
}