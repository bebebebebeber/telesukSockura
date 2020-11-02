using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace TelesukIKoputcya
{
    class Program
    {
        static Socket sck;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!Оставшаяся емкость: 7145 миллиампер/ч");
            ///Console.ReadKey();
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            sck.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            var ipmy = GetIP();
            var port = "568";

            EndPoint endLocal = new IPEndPoint(IPAddress.Parse("95.214.10.36"),
                    int.Parse(port));
            sck.Bind(endLocal);
            EndPoint epRemote = new IPEndPoint(IPAddress.Any,
                int.Parse("568"));
            sck.Connect(epRemote);

            byte[] buffer = new byte[2_000_000];
            sck.BeginReceiveFrom(buffer,
                0,
                buffer.Length,
                SocketFlags.None,
                ref epRemote,
                new AsyncCallback(MessageCallBack),
                buffer
                );
            sck.Send(Encoding.ASCII.GetBytes("qqw"));
            Console.ReadKey();
        }
        private static void MessageCallBack(IAsyncResult ar)
        {
            try
            {
                    Console.WriteLine(Encoding.ASCII.GetString((byte[])ar.AsyncState));
            }
            catch (Exception ex)
            {

            }
        }
        private static string GetIP()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var item in host.AddressList)
            {
                if (item.AddressFamily == AddressFamily.InterNetwork)
                {
                    return item.ToString();
                }
            }
            return "127.0.0.1";
        }
    }
}
