using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace TestClient
{
    class Program
    {

        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9001/";

            using (HttpClient client = new HttpClient())
            {
                Console.WriteLine("\r\nsync to sync: client waits until response received before making next request");
                for (int i = 0; i <= 2; i++)
                {
                    DateTime sentAt = DateTime.Now;
                    string msg = sentAt.ToLongTimeString();
                    Console.WriteLine("sending {0}", msg);
                    HttpResponseMessage response = client.GetAsync(baseAddress + "api/tests/sync/" + i.ToString() + "/" + msg).Result;
                    TimeSpan duration = DateTime.Now - sentAt;
                    string x = response.Content.ReadAsAsync<string>().Result;
                    Console.WriteLine("sent {0} - received {1} duration {2}", msg, x, duration.Seconds);
                }
                Console.WriteLine("Press a key to continue ...");
                Console.ReadLine();

                Console.WriteLine("\r\nsync to async: client waits until response received before making next request");
                for (int i = 0; i <= 2; i++)
                {
                    DateTime sentAt = DateTime.Now;
                    string msg = sentAt.ToLongTimeString();
                    Console.WriteLine("sending {0}", msg);
                    HttpResponseMessage response = client.GetAsync(baseAddress + "api/tests/async/" + i.ToString() + "/" + msg).Result;
                    TimeSpan duration = DateTime.Now - sentAt;
                    string x = response.Content.ReadAsAsync<string>().Result;
                    Console.WriteLine("sent {0} - received {1} duration {2}", msg, x, duration.Seconds);
                }
                Console.WriteLine("Press a key to continue ...");
                Console.ReadLine();

                Console.WriteLine("\r\nasync to sync: client does not wait for previous response, server does work on the service thread");
                for (int i = 0; i <= 9; i++)
                {
                    DateTime sentAt = DateTime.Now;
                    string msg = sentAt.ToLongTimeString();
                    Console.WriteLine("sending {0}", msg);
                    client.GetAsync(baseAddress + "api/tests/sync/" + i.ToString() +"/" + msg).ContinueWith(t => 
                    {
                        t.Result.Content.ReadAsAsync<string>().ContinueWith(s => 
                        {
                            TimeSpan duration = DateTime.Now - sentAt;
                            string x = s.Result as string;
                            Console.WriteLine("sent {0} - received {1} duration {2}", msg, x, duration.Seconds);
                        });
                    });
                }
                Console.WriteLine("Press a key to continue ...");
                Console.ReadLine();

                Console.WriteLine("\r\nasync to async: client does not wait for previous response, server does work on worker thread");
                for (int i = 0; i <= 9; i++)
                {
                    DateTime sentAt = DateTime.Now;
                    string msg = sentAt.ToLongTimeString();
                    Console.WriteLine("sending {0}", msg);
                    client.GetAsync(baseAddress + "api/tests/async/" + i.ToString() + "/" + msg).ContinueWith(t =>
                    {
                        t.Result.Content.ReadAsAsync<string>().ContinueWith(s =>
                        {
                            TimeSpan duration = DateTime.Now - sentAt;
                            string x = s.Result as string;
                            Console.WriteLine("sent {0} - received {1} duration {2}", msg, x, duration.Seconds);
                        });
                    });
                }

                Console.WriteLine("Press a key to end ...");
                Console.ReadLine();
            }


        }

        static private void DisplayResponse(HttpResponseMessage response, int x)
        {
            //response.EnsureSuccessStatusCode();
            //Console.WriteLine(response);
            Console.WriteLine(response.IsSuccessStatusCode ? "... success" : "...failed");
            Console.WriteLine("value is {0}", x);
        }


    }
}
