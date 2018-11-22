**************************
WebSite/WebAPI Logging Extensions
**************************

Fanex Logging WebSite/WebAPI Extensions will help you collect useful information when error occurs, do these steps to use it.

**Step 1: Install required packages**

- For .Net Framework: http://nuget.nexdev.net/packages/Fanex.Logging.Extensions.Web
- For .Net Core: http://nuget.nexdev.net/packages/Fanex.Logging.Extensions.AspNetCore
- If your site use Fanex.Data, please follow table below

+----------------------+-------------------------------------------------------------------------------+
| Fanex.Data version   | Packages                                                                      |
+======================+===============================================================================+
| >= 0.59 && < 0.62    | http://nuget.nexdev.net/packages/Fanex.Logging.Extensions.Database.V59/       |
+----------------------+-------------------------------------------------------------------------------+
| 0.62,0.63            | http://nuget.nexdev.net/packages/Fanex.Logging.Extensions.Database.V62/       |
+----------------------+-------------------------------------------------------------------------------+
| >= 2.0 && < 3.0      | http://nuget.nexdev.net/packages/Fanex.Logging.Extensions.Database/1.0.2      |
+----------------------+-------------------------------------------------------------------------------+
| >= 3.0               | http://nuget.nexdev.net/packages/Fanex.Logging.Extensions.Database/           |
+----------------------+-------------------------------------------------------------------------------+


**Step 2: Setup Exception Handler**

.. code:: csharp

        ExceptionHandler
            // To use custom config
            .UseConfig(new LoggingConfiguration()
            // To ignore exceptions
            //.IgnoreException<DivideByZeroException>()
            // To hide sensitive information
            //.HideSensitiveInfo(
                    //new List<string>
                    //{
                    //    "(?:password=)(?<paramValue>[^&|^\\s]+)",
                    //   "(\"password\":)(?<paramValue>[^,|^\r]+)",
                    //    "(?:seccode=)(?<paramValue>[^&|^\\s]+)",
                    //    "(\"seccode\":)(?<paramValue>[^,|^\r]+)",
                    //    "(?:securitycode=)(?<paramValue>[^&|^\\s]+)",
                    //    "(\"securitycode\":)(?<paramValue>[^,|^\r]+)"
                    //})
            // To Enable Log Http Header
            //.GetHttpHeaderInfo()
            // To Enable Log Http Session
            //.GetHttpSessionInfo())
            // To Log Database Information
            .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);

**Step 3: Log exception when error occurs**

In **WebSite/WebAPI .Net Framework**, you can catch all exception in Global Application_Error

.. code:: csharp

    ExceptionHandler.Handle(
        Server.GetLastError()
        // To Write Custom Data
        // new Dictionary<string, object> { ["user id"] = 1111111 });

In **Asp.Net Core**, you can catch all exception in Middleware then use ExceptionHandler handles it.



