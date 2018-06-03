using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Sockets;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(1302);
            listener.Start();

            while (true)
            {
                Console.WriteLine("Waiting For a connection...");
                TcpClient client = listener.AcceptTcpClient();

                StreamReader sr = new StreamReader(client.GetStream());
                StreamWriter sw = new StreamWriter(client.GetStream());

                try
                {
                    //client request
                    string request = sr.ReadLine();
                    Console.WriteLine(request);
                    string[] tokens = request.Split(' ');
                    string page = tokens[1];
                    if(page =="/")
                    {
                        page = "/Page1.html";
                    }
                    //find the file

                    StreamReader file = new StreamReader("../../web/other" + page);
                    sw.WriteLine("HTTP/1.0 200 OK\n");

                    //Send the file
                    string data = file.ReadLine();
                    while(data != null)
                    {
                        sw.WriteLine(data);
                        sw.Flush();
                        data = file.ReadLine();
                    }
                    
                }
                catch (Exception)
                {
                    //error
                    sw.WriteLine("HTTP/1.0 404 OK\n");
                    sw.WriteLine("<h1>Sorry we couldn't find your file !!</h1>");
                    sw.Flush();
                }
                client.Close();
            }

        }

    }
}
