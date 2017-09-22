namespace Fanex.Logging.Extensions.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class AppExceptionHandler
    {
        List<Type> skippedExceptionTypes = new List<Type>();
        Exception AppException;

        public AppExceptionHandler Handle(Exception exception)
        {
            if (!skippedExceptionTypes.Contains(exception.GetType()))
            {
                AppException = exception;
            }          

            return this;
        }
        public AppExceptionHandler SkipExceptionTypes(params Type[] exceptionTypes)
        {
            skippedExceptionTypes.AddRange(exceptionTypes);

            return this;
        }
       

    }
}
