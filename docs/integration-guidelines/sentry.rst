**************************
Sentry Integration
**************************

Sentry is Open-source error tracking that helps developers monitor and fix crashes in real time. Iterate continuously. Boost efficiency. Improve user experience. 
You can study more at https://docs.sentry.io/


**Step 1. Setup SentryLogging**

- Install this package http://nuget.nexdev.net/packages/Fanex.Logging.Sentry 
- Create new SentryLogging Instance before adding it to LogManager

.. code:: csharp

       new SentryLogging(new SentryEngineOptions
                        {
                            Dsn = new Dsn("{Your-site-sentry-token}"),
                            Release = "{Your-release-key}",
                            // You need to set FrontEndDns in case your front-end side access to different Sentry endpoint with back-end side
                            FrontEndDns = new Dsn("{Your-site-sentry-token}"),
                        });


To config more Sentry Options you can refer to https://docs.sentry.io/error-reporting/configuration/?platform=csharp


**Step 2. Change {Your-site-sentry-token} depend on your environment, this information you can config in sentry admin site**

+-------------+-----------------------------+
| Environment | Sentry Admin Site           |
+=============+=============================+
| LOCAL       | http://sentry.nexdev.net    |
+-------------+-----------------------------+
| UAT - PRO   | http://sentry.oneops.net    |
+-------------+-----------------------------+