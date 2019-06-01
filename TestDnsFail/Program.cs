using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestDnsFail
{
    class Program
    {

        static void TestPing()
        {
            TestPing("www.google.com");
            TestPing("www.free.fr");
            TestPing("192.168.0.254");
            // TestPing("toto");
        }

        static void TestPing(string host)
        {
            var ping = new Ping();
            var reply = ping.Send(host);
            if (reply.Status == IPStatus.Success)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < reply.Address.GetAddressBytes().Length; i++)
                {
                    var b = reply.Address.GetAddressBytes()[i];
                    switch (reply.Address.AddressFamily)
                    {
                        case AddressFamily.InterNetwork:
                            sb.Append($"{b}");
                            if (i < 3)
                                sb.Append($".");
                            break;
                        case AddressFamily.InterNetworkV6:
                            sb.Append($"{b:X2}");
                            if (i == 7)
                                sb.Append($" : ");
                            else if (i < 15 && i % 2 == 1)
                                sb.Append($":");
                            break;
                    }
                }
                Console.WriteLine($"{host} : {reply.Status} | {reply.RoundtripTime} ms | {reply.Address.AddressFamily} | {sb.ToString()}");
            }
            else
                Console.WriteLine($"{host} : Failed: {reply.Status}");
        }

        static void TestDnsError()
        {
            Console.WriteLine("Hello world");

            var connection = new TcpClient("toto", 123);
            //connection.Connect();
            Console.WriteLine("Connected");


            //  throw new Exception("Error");
        }

        static void Main(string[] args)
        {
            try
            {
                // TestDnsError();
                TestPing();
            }
            catch (SocketException e)
            {
                Console.WriteLine($"Sock exception: {e.Message} => ec:{e.SocketErrorCode}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
}
