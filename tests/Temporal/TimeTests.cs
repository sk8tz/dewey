using Dewey.Net.Temporal;
using Dewey.Net.xUnit;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Dewey.Net.Tests.Temporal
{
    public class TimeTests : TestBase
    {
        public TimeTests(ITestOutputHelper output)
            : base(output)
        {

        }

        [Fact]
        public void TestTimeFromString()
        {
            try {
                var constructorTime = new Time("01:01:01");

                var contructorResult = constructorTime.ToString();

                Assert.Equal("01:01:01 AM", contructorResult);

                Print(contructorResult);

                Time implicitTime = "01:01:01";

                var implicitResult = implicitTime.ToString();

                Assert.Equal("01:01:01 AM", implicitResult);

                Print(contructorResult);
            } catch(Exception ex) {
                Print(ex.Message);

                throw ex;
            }
        }

        [Fact]
        public void TestMidnight()
        {
            try {
                var time = Times.Midnight;

                Assert.Equal(0, time.Hour);
                Assert.Equal(0, time.Minute);
                Assert.Equal(0, time.Second);
                Assert.Equal(0, time.Millisecond);

                var result = time.ToString();

                Print(result);
            } catch (Exception ex) {
                Print(ex.Message);

                throw ex;
            }
        }

        [Fact]
        public void TestNoon()
        {
            try {
                var time = Times.Noon;

                Assert.Equal(12, time.Hour);
                Assert.Equal(0, time.Minute);
                Assert.Equal(0, time.Second);
                Assert.Equal(0, time.Millisecond);

                var result = time.ToString();

                Print(result);
            } catch (Exception ex) {
                Print(ex.Message);

                throw ex;
            }
        }
    }
}
