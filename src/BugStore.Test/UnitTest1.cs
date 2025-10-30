namespace BugStore.Test;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Assert.True(true);
    }

    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(2, 3, 5)]
    [InlineData(10, 20, 30)]
    public void Should_AddNumbers(int a, int b, int expected)
    {
        // Act
        var result = a + b;

        // Assert
        Assert.Equal(expected, result);
    }
}
