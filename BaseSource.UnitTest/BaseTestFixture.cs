using NUnit.Framework;

namespace BaseSource.UnitTest
{
    [TestFixture]
    public abstract class BaseTestFixture
    {
        [SetUp]
        public async Task TestSetUp()
        {
            //await ResetState();
        }
    }

}
