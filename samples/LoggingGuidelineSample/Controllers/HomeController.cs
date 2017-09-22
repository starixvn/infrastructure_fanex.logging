namespace LoggingGuidelineSample.Controllers
{
    using System.Web.Mvc;
    using Fanex.Data;
    using Models;

    public class HomeController : Controller
    {
        public HomeController()
        {
        }
        //// GET: Home
        //// http://localhost:54426/?password=3434343&securitycode=344545
        //// {
        ////	"password":"44444",
        ////	"secuRITYCODE": "555"
        ////}
        public ActionResult Index()
        {
            SetSampleSession();

            var sampleZero = 0;
            var sampleNumber = 10;
            // Simutate exception
            sampleNumber = sampleNumber / sampleZero;

            return Content("Index View");
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
    }
}