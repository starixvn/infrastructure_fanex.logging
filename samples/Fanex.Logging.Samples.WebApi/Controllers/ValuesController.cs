namespace Fanex.Logging.Samples.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Data;

    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            SimulateDBException();

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            SimulateException();

            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
            SimulateDBException();
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
            SimulateException();
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        private void SimulateException()
        {
            var sampleZero = 0;
            var sampleNumber = 10;
            // Simutate exception
            sampleNumber = sampleNumber / sampleZero;
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