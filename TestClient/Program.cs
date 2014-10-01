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
                //HttpResponseMessage response;

                Console.WriteLine("\r\nsync to sync: client waits until response received before making next request");
                for (int i = 0; i <= 2; i++)
                {
                    string msg = DateTime.Now.ToLongTimeString();
                    Console.WriteLine("sending {0}", msg);
                    HttpResponseMessage response = client.GetAsync(baseAddress + "api/tests/sync/" + i.ToString() + "/" + msg).Result;
                    string x = response.Content.ReadAsAsync<string>().Result;
                    Console.WriteLine("sent {0} - received {1} at {2}", msg, x, DateTime.Now.ToLongTimeString());
                }

                Console.WriteLine("Press a key to continue ...");
                Console.ReadLine();

                Console.WriteLine("\r\nsync to async: client waits until response received before making next request");
                for (int i = 0; i <= 2; i++)
                {
                    string msg = DateTime.Now.ToLongTimeString();
                    Console.WriteLine("sending {0}", msg);
                    HttpResponseMessage response = client.GetAsync(baseAddress + "api/tests/async/" + i.ToString() + "/" + msg).Result;
                    string x = response.Content.ReadAsAsync<string>().Result;
                    Console.WriteLine("sent {0} - received {1} at {2}", msg, x, DateTime.Now.ToLongTimeString());
                }

                Console.WriteLine("Press a key to continue ...");
                Console.ReadLine();

                Console.WriteLine("\r\nasync to sync: client does not wait for previous response, but server ability to deal with requests is compromised (see lengthened response times)");
                for (int i = 0; i <= 49; i++)
                {
                    string msg =  DateTime.Now.ToLongTimeString();
                    Console.WriteLine("sending {0}", msg);
                    client.GetAsync(baseAddress + "api/tests/sync/" + i.ToString() +"/" + msg).ContinueWith(t => 
                    {
                        t.Result.Content.ReadAsAsync<string>().ContinueWith(s => 
                        {
                            string x = s.Result as string;
                            Console.WriteLine("sent {0} - received {1} at {2}", msg, x, DateTime.Now.ToLongTimeString());
                        });
                    });
                }

                Console.WriteLine("Press a key to continue ...");
                Console.ReadLine();

                Console.WriteLine("\r\nasync to async: lient does not wait for previous response and server is free to service all requests as processing is performed in background");
                for (int i = 0; i <= 49; i++)
                {
                    string msg = DateTime.Now.ToLongTimeString();
                    Console.WriteLine("sending {0}", msg);
                    client.GetAsync(baseAddress + "api/tests/async/" + i.ToString() + "/" + msg).ContinueWith(t =>
                    {
                        t.Result.Content.ReadAsAsync<string>().ContinueWith(s =>
                        {
                            string x = s.Result as string;
                            Console.WriteLine("sent {0} - received {1} at {2}", msg, x, DateTime.Now.ToLongTimeString());
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
