namespace Fanex.Logging.Samples.Web.Controllers
{
    using System.Threading;
    using System.Web.Mvc;
    using Fanex.Data;
    using Models;

    public class HomeController : Controller
    {
        [HttpGet, HttpPost]
        public ActionResult Index()
        {
            //SetSampleSession();
            //Thread.Sleep(10000);
            //var sampleZero = 0;
            //var sampleNumber = 10;
            ////// Simutate exception
            //sampleNumber = sampleNumber / sampleZero;

            //return Content("Index View");
            //LogService.Error("Ricky test on wweb 2");
            return View();
        }

        public ActionResult ViewError()
        {
            return View();
        }

        public ActionResult SqlException()
        {
            SetSampleSession();

            using (var db = ObjectDbFactory.CreateInstance("Logging_UpdateMachine"))
            {
                db.ExecuteNonQuery(new
                {
                    MachineIP = "45454",
                    Timezone = "GMT8"
                });
            }

            return Content("SQL View");
        }

        private void SetSampleSession()
        {
            Session["Session1"] = new SampleModel
            {
                SampleProperty1 = "SampleProperty1",
                SampleProperty2 = 2
            };

            Session["Session2"] = "Session2";
        }

        [Route("test")]
        public ActionResult SentryEndPointSimulator()
        {
            //Thread.Sleep(10000);
            
            return View();
        }
    }
}