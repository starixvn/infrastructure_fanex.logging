**************************
Sentry Integration
**************************

**Step 1. Setup SentryLogging**

- Install this package http://nuget.nexdev.net/packages/Fanex.Logging.Sentry 
- Create new SentryLogging Instance before adding it to LogManager

.. code:: csharp

       new SentryLogging(new SentryOptions
                        {
                            Dsn = new Dsn("{Your-site-sentry-token}"),
                            Release = "{Your-release-key}"
                        });


**Step 2. Change {Your-site-sentry-token} depend on your environment, this information you can config in sentry admin site**

+-------------+-----------------------------+
| Environment | Sentry Admin Site           |
+=============+=============================+
| LOCAL       | http://sentry.nexdev.net    |
+-------------+-----------------------------+
| UAT - PRO   | http://sentry.oneops.net    |
+-------------+-----------------------------+

**Notes:** In order to inquire more Sentry Documentation please go to this site https://docs.sentry.io/