using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestDnsFail
{
    public class TestArp
    {

        [DllImport("Ws2_32.dll")]
        private static extern Int32 inet_addr(string ip);

        [DllImport("Iphlpapi.dll")]
        private static extern int SendARP(Int32 dest, Int32 host, ref Int64 mac, ref Int32 len);

        public static Int64 getRemoteMAC(string localIP, string remoteIP)
        {
            Int32 ldest = inet_addr(remoteIP);
            Int32 lhost = inet_addr(localIP);

            Int64 macinfo = 0;
            Int32 len = 6;
            int res = SendARP(ldest, 0, ref macinfo, ref len);
            if (res != 0)
                throw new Exception($"A pb occured: ret code = {res}");
            return macinfo;
        }

        public static string formatMac(Int64 mac, char separator=':')
        {
            if (mac <= 0)
                return "-";

            char[] oldmac = Convert.ToString(mac, 16).PadLeft(12, '0').ToCharArray();

            System.Text.StringBuilder newMac = new System.Text.StringBuilder(17);

            if (oldmac.Length < 12)
                return "00-00-00-00-00-00";

            newMac.Append(oldmac[10]);
            newMac.Append(oldmac[11]);
            newMac.Append(separator);
            newMac.Append(oldmac[8]);
            newMac.Append(oldmac[9]);
            newMac.Append(separator);
            newMac.Append(oldmac[6]);
            newMac.Append(oldmac[7]);
            newMac.Append(separator);
            newMac.Append(oldmac[4]);
            newMac.Append(oldmac[5]);
            newMac.Append(separator);
            newMac.Append(oldmac[2]);
            newMac.Append(oldmac[3]);
            newMac.Append(separator);
            newMac.Append(oldmac[0]);
            newMac.Append(oldmac[1]);

            return newMac.ToString();

        }
    }
}
