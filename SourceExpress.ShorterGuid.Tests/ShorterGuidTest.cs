using Xunit.Abstractions;

namespace SourceExpress.ShorterGuid.Tests;

public class ShorterGuidTest
{
    private readonly ITestOutputHelper output;

    public ShorterGuidTest(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void TestEmpty()
    {

        var shorterGuid = Guid.Empty.ToShorterString();
        Assert.Equal("AAAAAAAAAAAAAAAAAAAAAAAAAA", shorterGuid);
    }

    [Fact]
    public void TestNew()
    {
        var guid = Guid.NewGuid();
        var shorterGuid = guid.ToShorterString();
        var result = shorterGuid.FromShorterString();
        output.WriteLine(guid.ToString());
        output.WriteLine(shorterGuid);
        output.WriteLine(result.ToString());
        Assert.Equal(guid, result);
    }

    [Fact]
    public void TestCharException()
    {
        var wrongValue = "2YPHP2BWJYHUFOAUWKXUHU4UB_";
        Assert.Throws<FormatException>(() => wrongValue.FromShorterString());
    }

    [Fact]
    public void TestLengthException()
    {
        var wrongValue = "2YPHP2BWJYHUFOAUWKXUHU4UB42YPHP2BWJYHUFOAUWKXUHU4UB4";
        Assert.Throws<ArgumentOutOfRangeException>(() => wrongValue.FromShorterString());
    }
}
