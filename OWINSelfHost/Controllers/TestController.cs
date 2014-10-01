using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace OWINSelfHost
{
    
    [RoutePrefix("api/tests")]
    public class TestController : ApiController
    {

        [Route("sync/{id}/{time}"), HttpGet]           
        public string GetValueSync(string id, string time)
        {
            //Console.WriteLine("from {0}:{1} at {2} sync", id, time, DateTime.Now.ToLongTimeString());
            //string res = BigJob(id, time).Result;
            string res = BigJob(id, time, "sync");
            return res;
        }

        [Route("async/{id}/{time}"), HttpGet]
        public async Task<string> GetValueAsync(string id, string time)
        {
            //Console.WriteLine("from {0}:{1} at {2} async", id, time, DateTime.Now.ToLongTimeString());
            //string res = await BigJob(id, time);

            dont think this is working
            string res = await Task.Run<string>(() => BigJob(id, time, "async"));
            return res;
        }

        private string BigJob(string id, string time, string type)
        {
            Console.WriteLine("received {0}:{1} at {2} {3}", id, time, DateTime.Now.ToLongTimeString(), type);
            for (double d = 0; d < 900000000.0; d++)
            {
                double w = d + 1;
            }
            string res = DateTime.Now.ToLongTimeString();
            Console.WriteLine("  {0}:{1} returning {2} {3}", id, time, res, type);
            return res;
        }

        //private async Task<string> BigJob(string id, string time)
        //{
        //    await Task.Delay(10000);
        //    string res = DateTime.Now.ToLongTimeString();
        //    Console.WriteLine("  from {0}:{1} returning {2} async", id, time, res);
        //    return res;
        //}


    } 

}
