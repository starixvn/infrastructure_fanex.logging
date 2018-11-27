**************************
Service Logging Extensions
**************************

**Step 1: Install required packages**

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

**Step 2: Compose message and write log**


.. code:: csharp
        
        var logMessage = new LogMessage(exception.Message)
                                .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                                .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                                .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
                                .ToString();

        LogService.Error(logMessage);