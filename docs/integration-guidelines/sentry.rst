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
                            Dsn = new Dsn("{Your-site-sentry-token-with-intranet-ip}"),
                            Release = "{Your-release-key}", // In case you want to keep track new issue for your releases
                            // You need to set FrontEndDsn in case your front-end side accesses to different Sentry endpoint with back-end side
                            FrontEndDsn = new Dsn("{Your-site-sentry-token-with-public-domain}"),
                        });


To config more Sentry Options you can refer to https://docs.sentry.io/error-reporting/configuration/?platform=csharp

You can get the Your-site-sentry-token in Sentry Admin site follow this image:

.. image:: ../_static/sentry-dsn.png

**Step 2. Change {Your-site-sentry-token} depend on your environment, this information you can config in sentry admin site**

+-------------+-------------------------------------+-------------------------------------------+
| Environment | Sentry Admin Site (Public domain)   | Intranet IP                               |                                
+=============+=====================================+===========================================+
| LOCAL       | http://sentry.nexdev.net                                                        |
+-------------+-------------------------------------+-------------------------------------------+
| UAT - PRO   | http://sentry.starific.net          | 10.40.40.47:801                           |
+-------------+-------------------------------------+-------------------------------------------+

*Notes:* 

- In order to login to above sites, please contact NexOps team to get login account