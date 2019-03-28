using System;
using System.Diagnostics;
using Fanex.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fanex.Logging.Samples.AspNetCore.Pages
{
    public class IndexModel : PageModel
    {
        public string CompletedTime1 = string.Empty;

        public string CompletedTime2 = string.Empty;

        public void OnGet()
        {
            HttpContext.Session.SetString("demo", "value");
            HttpContext.Session.SetInt32("demo1", 34343);

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            LogService.Error("test message 1");
            stopwatch.Stop();

            CompletedTime1 = stopwatch.ElapsedMilliseconds.ToString();

            stopwatch = new Stopwatch();
            stopwatch.Start();
            LogService.Error("test message 2");
            stopwatch.Stop();

            CompletedTime2 = stopwatch.ElapsedMilliseconds.ToString();


        }

        public void OnPost()
        {
            HttpContext.Session.SetString("demo", "value");
            HttpContext.Session.SetInt32("demo1", 34343);

            SqlException();
        }

        public void NormalException()
        {
            var sampleZero = 0;
            var sampleNumber = 10;
            // Simutate exception
            sampleNumber = sampleNumber / sampleZero;
        }

        public void SqlException()
        {
            using (var db = ObjectDbFactory.CreateInstance("InsertUser"))
            {
                db.ExecuteNonQuery(new
                {
                    SubAccountId = "343433434",
                    MachineIP = "45454"
                });
            }
        }
    }
}