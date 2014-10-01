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
            string res = BigFileIOJob(id, time, "sync");
            return res;
        }

        [Route("async/{id}/{time}"), HttpGet]
        public async Task<string> GetValueAsync(string id, string time)
        {
            string res = await Task.Run<string>(() => BigFileIOJob(id, time, "async"));
            return res;
        }

        private string BigFileIOJob(string id, string time, string type)
        {
            string text = "whats the difference between roast chicken and pea soup?  You can roast chicken but ...";
            for (int i = 0; i < 2000; i++)
            {
                string fp = @"c:\temp\" + id + i.ToString() + ".txt";
                System.IO.File.WriteAllText(fp, text);
            }
            for (int i = 0; i < 2000; i++)
            {
                string fp = @"c:\temp\" + id + i.ToString() + ".txt";
                System.IO.File.Delete(fp);
            }
            string res = DateTime.Now.ToLongTimeString();
            res = string.Format("{0}:{1} returning {2} {3}", id, time, res, type);
            return res;
        }

        private string BigCpuJob(string id, string time, string type)
        {
            for (double d = 0; d < 900000000.0; d++)
            {
                double w = d + 1;
            }
            string res = DateTime.Now.ToLongTimeString();
            res = string.Format("{0}:{1} returning {2} {3}", id, time, res, type);
            return res;
        }

        private async Task<string> BigJob(string id, string time)
        {
            await Task.Delay(10000);
            string res = DateTime.Now.ToLongTimeString();
            Console.WriteLine("  from {0}:{1} returning {2} async", id, time, res);
            return res;
        }


    } 

}
