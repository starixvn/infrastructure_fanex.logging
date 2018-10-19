using Fanex.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fanex.Logging.Samples.AspNetCore.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            HttpContext.Session.SetString("demo", "value");
            HttpContext.Session.SetInt32("demo1", 34343);

            SqlException();
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