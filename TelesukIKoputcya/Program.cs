﻿using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace TelesukIKoputcya
{
    class Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public override string ToString()
        {
            return $"{X} {Y} {Z}";
        }
    }
    enum CommandPlayEnum
    {
        Connect=0,
        Playing=1
    }
    class Play
    {
        public string Name { get; set; }
        public bool IsOne { get; set; }
        public Vector3 Position { get; set; }
        public CommandPlayEnum Command { get; set; }
        public override string ToString()
        {
            return $"Name: {Name}\t IsONe: {IsOne}\t " +
                $"Position: {Position}\t Command: {Command}";
        }
    }
    class Program
    {
        static Play PlayOne = null;
        static Play PlayTwo = null;
        static Socket sck;
        static void Main(string[] args)
        {
            Socket s = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.IP);
            IPAddress ip = IPAddress.Parse("95.214.10.36");//IPAddress.Parse("91.204.84.93"); 
            IPEndPoint ep = new IPEndPoint(ip, 560);
            Console.WriteLine("Server " + ep.ToString());
            s.Bind(ep); //Наш сокет звязаний з даною адресою
            s.Listen(10);
            try
            {
                while (true)
                {
                    Socket ns = s.Accept();
                    string data = null;
                    // Мы дождались клиента, пытающегося с нами соединиться

                    byte[] bytes = new byte[1024];
                    int bytesRec = ns.Receive(bytes);

                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    Console.WriteLine("Нам прислали: " + data);
                    try
                    {
                        var player = JsonConvert.DeserializeObject<Play>(data);
                        Console.WriteLine("Нам прислали: " + player);
                    }
                    catch { Console.WriteLine("Bad data!!!"); }
                    
                    Console.WriteLine(ns.RemoteEndPoint.ToString());

                    ns.Send(Encoding.ASCII.GetBytes($"Vova server {DateTime.Now}"));
                    ns.Shutdown(SocketShutdown.Both);
                    ns.Close();

                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Socket problem: " + ex.Message);
            }
        }
    }
}
