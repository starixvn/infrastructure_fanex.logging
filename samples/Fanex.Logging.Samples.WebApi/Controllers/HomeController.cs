using System;
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

        public ActionResult TestDBException()
        {
            SimulateDBException();

            return View();
        }

        public ActionResult TestLogging()
        {
            var logMessage = $"This is Fanex Logging testing messsage at {DateTime.Now}";

            LogService.Error(logMessage);
            LogService.Warn(logMessage);
            LogService.Debug(logMessage);
            LogService.Critical(logMessage);
            LogService.Info(logMessage);

            return new EmptyResult();
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