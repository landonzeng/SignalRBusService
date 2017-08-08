using Microsoft.Owin.Hosting;
using System;
using System.Windows.Forms;

namespace ServerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:10086";//设定 SignalR Hub Server 对外的接口

            using (WebApp.Start<Startup>(url))//启动 SignalR Hub Server
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }
}
