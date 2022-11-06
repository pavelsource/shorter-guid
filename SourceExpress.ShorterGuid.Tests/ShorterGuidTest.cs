using System.Diagnostics;
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
        Assert.Equal("00000000000000000000000000", shorterGuid);
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
        var wrongValue = "g0h2o50ibvk4b2g2rqz5k3psg_";
        Assert.Throws<FormatException>(() => wrongValue.FromShorterString());
    }

    [Fact]
    public void TestLengthException()
    {
        var wrongValue = "g0h2o50ibvk4b2g2rqz5k3psgsg0h2o50ibvk4b2g2rqz5k3psgs";
        Assert.Throws<ArgumentOutOfRangeException>(() => wrongValue.FromShorterString());
    }
}
