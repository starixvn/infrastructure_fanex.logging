namespace Fanex.Logging.Extensions.Database.V59
{
    using System;
    using Fanex.Data;

    public class ObjectDbException : Exception
    {
        private readonly IDbSetting _dbSetting;

        public ObjectDbException(string message, Exception innerException, IDbSetting dbSetting)
            : base(message, innerException)
        {
            this._dbSetting = dbSetting;
        }

        public IDbSetting DbSetting
        {
            get
            {
                return _dbSetting;
            }
        }
    }
}