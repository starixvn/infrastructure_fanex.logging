**************************
Entlibs with RabbitMQ Integration
**************************

**Step 1. Setup RabbitMQLogging**

- Install this package http://nuget.nexdev.net/packages/Fanex.Logging.RabbitMQ 
- Create new RabbitMQLogging Instance before adding it to LogManager

.. code:: csharp

       new RabbitMQLogging("{RabbitMQ-ConnectionString}");


**Step 2. Change {RabbitMQ-ConnectionString} depend on your environment**

+-------------+----------------------------------------------------------------------------------------------------------------------------------------+
| Environment | RabbitMQ-ConnectionString                                                                                                              |
+=============+========================================================================================================================================+
| LOCAL       | host=10.18.200.103;virtualHost=/Fanex_Logging;username=fanexlogging;password=YKQrMtla7oYZGlP8apPtSw==                                  |
+-------------+----------------------------------------------------------------------------------------------------------------------------------------+
| UAT         | host=10.99.99.101:5672;virtualHost=/fanex_logging;username=fanexlogging;password=VKREKUBBd2T3DAtosbDPojl4YQFDnDp7Yilb8JGcLQ8=           |
+-------------+----------------------------------------------------------------------------------------------------------------------------------------+
| SIT         | host=172.16.4.106,172.16.4.107;virtualHost=/fanex_logging;username=fanexlogging;password=VKREKUBBd2T3DAtosbDPojl4YQFDnDp7Yilb8JGcLQ8=  |
+-------------+----------------------------------------------------------------------------------------------------------------------------------------+
| PRO         | host=10.99.99.37;virtualHost=/fanex_logging;username=fanexlogging;password=VKREKUBBd2T3DAtosbDPojl4YQFDnDp7Yilb8JGcLQ8=                |
+-------------+----------------------------------------------------------------------------------------------------------------------------------------+
| MNL         | host=10.40.40.202;virtualHost=/fanex_logging;username=fanexlogging;password=VKREKUBBd2T3DAtosbDPojl4YQFDnDp7Yilb8JGcLQ8=               |
+-------------+----------------------------------------------------------------------------------------------------------------------------------------+

**Notes:**

- To view log, you can visit MSite


