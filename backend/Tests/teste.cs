using Xunit;
using backend.Models;

namespace TestProject1
{
    public class SomaTest
    {
        [Fact]
        public void TestSoma()
        {
            // Arrange
            int a = 5;
            int b = 7;
            int expectedResult = 12;

            // Act
            int result = soma.Soma(a, b);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
