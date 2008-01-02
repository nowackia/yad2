using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Yad.Utilities.Common {
    public class NetUtils : TcpClient {
        public static void SetKeepAlive(TcpClient client) {
            Socket s = client.Client;
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            SetKeepAlive(s, true, Properties.Common.Settings.Default.KeepAliveTime,
                Properties.Common.Settings.Default.KeepAliveInterval);
        }

        private static void SetKeepAlive(Socket s, bool on, uint time, uint interval) {
            /* the native structure
            struct tcp_keepalive {
            ULONG onoff;
            ULONG keepalivetime;
            ULONG keepaliveinterval;
            };
            */

            // marshal the equivalent of the native structure into a byte array
            uint dummy = 0;
            byte[] inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
            BitConverter.GetBytes((uint)(on ? 1 : 0)).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes((uint)time).CopyTo(inOptionValues, Marshal.SizeOf(dummy));
            BitConverter.GetBytes((uint)interval).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);
            // of course there are other ways to marshal up this byte array, this is just one way

            // call WSAIoctl via IOControl
            int ignore = s.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);

        }
    }
}
