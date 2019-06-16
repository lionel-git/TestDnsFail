using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace TestDnsFail
{
    public class CheckNetWork
    {

        public void TestAddress()
        {
            var ipEntry = Dns.GetHostEntry("ps4-EEA1EF");
            Console.WriteLine(ipEntry.HostName);
            Console.WriteLine(ipEntry.AddressList[0]);
        }

        public void Check(int start, int end)
        {
            for (int i=start;i<=end;i++)
            {
                var ip = $"192.168.0.{i}";
                Console.Write($"{ip} => ");
                var ping = new Ping();
                var reply = ping.Send(ip, 100);
                if (reply.Status == IPStatus.Success)
                {
                    Console.Write("Found ... ");
                    try
                    {
                        var ipEntry = Dns.GetHostEntry(ip);
                        Console.WriteLine(ipEntry.HostName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"No hostname: {e.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("N/A");
                }



            }
        }
    }
}
