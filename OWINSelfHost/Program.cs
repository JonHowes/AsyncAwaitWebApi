using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;

namespace OWINSelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string baseAddress = "http://localhost:9001/";
                IDisposable wa = WebApp.Start<Startup>(url: baseAddress);  // Start OWIN host
                Console.WriteLine("WebAPI 2 started: {0}", baseAddress);
                Console.WriteLine("Press a key to stop the service ...");
                Console.ReadLine();
                wa.Dispose();
            }
            catch
            {

            }
        }
    }
}
