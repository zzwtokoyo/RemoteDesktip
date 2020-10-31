using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDesktip
{
    class ErrorData
    {
        public const int disconnectReasonAtClientWinsockFDCLOSE = 0x904;
        public const int disconnectReasonByServer = 0x3;
        public const int disconnectReasonClientDecompressionError = 0xC08;
        public const int disconnectReasonConnectionTimedOut = 0x108;
    }
}
