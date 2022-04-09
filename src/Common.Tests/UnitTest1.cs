using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Common.Tests;

public record Hello(string Value);

public record HelloE(Aes256<string> Value);

public class Aes256Spec
{
    public Aes256Spec(ITestOutputHelper output)
    {
        Output = output;
    }

    public ITestOutputHelper Output { get; }

    [Theory, AutoData]
    public void Success(Hello msg, AesKey key)
    {
        var sut = new HelloE
        (
            (msg.Value, key)
        );

        Output.WriteLine($"{sut}");

        sut.Value.Decrypt(key).Should().Be(msg.Value);
    }
}
