using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zeroconf;

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

            var culture = new CultureInfo("en");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            var connection = new TcpClient("toto", 123);
            //connection.Connect();
            Console.WriteLine("Connected");


            //  throw new Exception("Error");
        }

        // Cf https://stackoverflow.com/questions/34423129/how-to-get-win32exception-in-english
        static void TestMessage()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            string msg2 = new SocketException((int)SocketError.HostNotFound).Message;
            Console.WriteLine($"msg2= {msg2}");

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr-FR");
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
            string msg3 = new SocketException((int)SocketError.HostNotFound).Message;
            Console.WriteLine($"msg3= {msg3}");



        }

        static void TestNetWork()
        {
            var checkNetwork = new CheckNetWork();
            checkNetwork.TestAddress();
            checkNetwork.Check(10, 50);
        }
        static void TestNetBios()
        {
            var netBios = new NetworkBrowser();
            var names = netBios.getNetworkComputers();
            foreach (var name in names)
            {
                Console.WriteLine(name);
            }        
        }

        static void TestZeroConf()
        {
            var t=EnumerateAllServicesFromAllHosts();
            t.Wait();
        }


        public async Task ProbeForNetworkPrinters()
        {
            IReadOnlyList<IZeroconfHost> results = await
                ZeroconfResolver.ResolveAsync("_printer._tcp.local.");
        }

        public static async Task EnumerateAllServicesFromAllHosts()
        {
            ILookup<string, string> domains = await ZeroconfResolver.BrowseDomainsAsync();
            var responses = await ZeroconfResolver.ResolveAsync(domains.Select(g => g.Key));
            foreach (var resp in responses)
                Console.WriteLine(resp);
        }


        static void Main(string[] args)
        {
            try
            {
                TestZeroConf();
                //TestNetBios();
                //TestNetWork();
                //TestMessage();
                //TestDnsError();
                //TestPing();
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
