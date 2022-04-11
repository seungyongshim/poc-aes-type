using System.Linq;
using System.Text.Json;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Common.Tests;

public class Aes256Spec
{
    public Aes256Spec(ITestOutputHelper output) => Output = output;

    public ITestOutputHelper Output { get; }

    [Theory, AutoData]
    public void SuccessThruJsonSerialize(Hello msg, AesKey key)
    {
        var sut = new HelloE
        (
            (msg.Value, key),
            msg.Worlds.Select(x => Aes256.Encrypt(x, key))
                      .ToList()
        );

        var payload = JsonSerializer.Serialize(sut);

        Output.WriteLine($"{key}");
        Output.WriteLine($"{payload}");

        var receive = JsonSerializer.Deserialize<HelloE>(payload)!;

        var ret = new Hello
        (
            receive.Value.Decrypt(key),
            receive.Worlds.Select(x => x.Decrypt(key)).ToList()
        );

        ret.Should().BeEquivalentTo(msg);
    }
}
