using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace Common.Tests;

public record Hello(string Value);

public record HelloE(Aes256<string> Value);

public class Aes256Spec
{
    [Theory, AutoData]
    public void Success(Hello msg, AesKey key)
    {
        var sut = new HelloE
        (
            (msg.Value, key)
        );

        sut.Value.Decrypt(key).Should().Be(msg.Value);
    }
}
