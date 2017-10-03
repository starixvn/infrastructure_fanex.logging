namespace Fanex.Logging.Extensions.Database.V59
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Data;

    public class ObjectDbException : Exception
    {
        private readonly IDbSetting _dbSetting;

        public ObjectDbException()
        {
        }

        public ObjectDbException(string message)
            : base(message)
        {
        }

        public ObjectDbException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public ObjectDbException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ObjectDbException(string message, Exception innerException, IDbSetting dbSetting)
            : base(message, innerException)
        {
            this._dbSetting = dbSetting;
        }

        public ObjectDbException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException)
        {
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
