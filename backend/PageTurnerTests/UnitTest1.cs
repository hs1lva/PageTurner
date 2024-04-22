namespace PageTurnerTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var a = 5;
        var b = 10;
        var expectedSum = 15;

        var actualSum = a + b;

        Assert.That(actualSum, Is.EqualTo(expectedSum), "The sum of 5 and 10 should be 15");

    }
}