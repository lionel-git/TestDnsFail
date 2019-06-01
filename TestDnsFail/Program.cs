using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestDnsFail
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello world");
            try
            {
                var connection = new TcpClient("toto", 123);
                //connection.Connect();
                Console.WriteLine("Connected");
                }
            catch (SocketException e)
            {
                Console.WriteLine($"Sock exception: {e.Message} => ec:{e.SocketErrorCode}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
          //  throw new Exception("Error");
        }
    }
}
