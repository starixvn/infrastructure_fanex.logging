using System.Web.Mvc;
using Fanex.Data;

namespace Fanex.Logging.Samples.WebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult TestLogging()
        {
            SimulateDBException();

            return View();
        }

        private void SimulateDBException()
        {
            using (var db = ObjectDbFactory.CreateInstance("Logging_UpdateMachine"))
            {
                db.ExecuteNonQuery(new
                {
                    MachineIP = "45454",
                    Timezone = "GMT8"
                });
            }
        }
    }
}