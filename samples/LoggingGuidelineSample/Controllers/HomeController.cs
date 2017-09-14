namespace LoggingGuidelineSample.Controllers
{
    using System.Web.Mvc;
    using Fanex.Data;

    public class HomeController : Controller
    {
        //// GET: Home
        //// http://localhost:54426/?password=3434343&securitycode=344545
        //// {
        ////	"password":"44444",
        ////	"secuRITYCODE": "555"
        ////}
        public ActionResult Index()
        {
            var sampleZero = 0;
            var sampleNumber = 10;
            // Simutate exception
            sampleNumber = sampleNumber / sampleZero;

            return Content("Index View");
        }

        public ActionResult SqlException()
        {
            using (var db = new ObjectDb("Logging_UpdateMachine"))
            {
                db.ExecuteNonQuery();
            }

            return Content("SQL View");
        }
    }
}