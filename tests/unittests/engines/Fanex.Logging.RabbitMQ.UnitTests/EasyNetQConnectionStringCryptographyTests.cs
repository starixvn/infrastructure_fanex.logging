namespace Fanex.Logging.RabbitMQ.UnitTests
{
    using Fanex.Logging.RabbitMQ.Helpers;
    using Xunit;

    [Trait("Unit Tests", "RabbitMQ/EasyNetQConnectionStringCryptography")]
    public class EasyNetQConnectionStringCryptographyTests
    {
        [Theory]
        [InlineData(
            "host=10.18.200.103;virtualHost=/Fanex_Logging;username=fanexlogging;password=1234aa",
            "host=10.18.200.103;virtualHost=/Fanex_Logging;username=fanexlogging;password=YKQrMtla7oYZGlP8apPtSw==")]
        [InlineData(
            "host=10.40.40.47:5673;virtualHost=/fanex_logging;username=fanexlogging;password=fanexlogging123!@#",
            "host=10.40.40.47:5673;virtualHost=/fanex_logging;username=fanexlogging;password=VKREKUBBd2T3DAtosbDPojl4YQFDnDp7Yilb8JGcLQ8=")]
        public void Encrypt_Always_ReturnEncryptedConnectionString(string connetionString, string expected)
        {
            Assert.Equal(expected, EasyNetQConnectionStringCryptography.Encrypt(connetionString));
        }
    }
}