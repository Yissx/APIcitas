namespace AppCitasUnitTest;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var x = 1;
        var y = x * 2;
        Assert.Equal(2, y);
    }
    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 4)] 
    public void Test2(int x, int resp)
    {
        int y = 2 * x;
        Assert.Equal(resp, y);
    }

}