namespace Fanex.Logging.Samples.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;

    public class TestController : Controller
    {
        [Route("testroute")]
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        [Route("api/5/store")]
        //[HttpGet, HttpPost]
        // GET: Test
        public ActionResult Index2()
        {
            Thread.Sleep(10 * 1000);
            return new ContentResult
            {
                Content = "test"
            };
        }
    }
}