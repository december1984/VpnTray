using System;
using System.Collections.Generic;
using System.Text;

namespace VpnTray.Driver.WinApi
{

    [Serializable]
    public class RasApi32Exception : Exception
    {
        public int ErrorCode { get; private set; }
        public RasApi32Exception(int errorCode) { ErrorCode = errorCode; }
        public RasApi32Exception(int errorCode, string message) : base(message) { ErrorCode = errorCode; }
        public RasApi32Exception(int errorCode, string message, Exception inner) : base(message, inner) { ErrorCode = errorCode; }
        protected RasApi32Exception(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
