namespace Fanex.Logging.Extensions.AspNetCore.UnitTests
{
    using Newtonsoft.Json;
    using Xunit;

    public static class AssertExtensions
    {
        public static void Equal<T>(T expexted, T actual)
            => Assert.Equal(JsonConvert.SerializeObject(expexted), JsonConvert.SerializeObject(actual));
    }
}