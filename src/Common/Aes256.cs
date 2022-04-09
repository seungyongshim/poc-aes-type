using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Common;

public record Aes256<T>
{
    private byte[] Body { get; init; }
    private byte[] IV { get; init; }
    private byte[] Tag { get; init; }

    public static implicit operator Aes256<T>(in (T Target, AesKey Key) v) =>
        Aes256<T>.Encrypt(v.Target, v.Key);

    public static Aes256<T> Encrypt(in T target, in AesKey key)
    {
        using var aes = new AesGcm(key.Value);

        var iv = new byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(iv);

        var plaintextBytes = JsonSerializer.SerializeToUtf8Bytes(target);
        var body = new byte[plaintextBytes.Length];
        var tag = new byte[AesGcm.TagByteSizes.MaxSize];

        aes.Encrypt(iv, plaintextBytes, body, tag);

        return new Aes256<T>(body, iv, tag);
    }

    private Aes256(byte[] body, byte[] iv, byte[] tag)
    {
        Body = body;
        IV = iv;
        Tag = tag;
    }

    public T Decrypt(in AesKey key)
    {
        using var aes = new AesGcm(key.Value);

        var plaintextBytes = new byte[Body.Length];

        aes.Decrypt(IV, Body, Tag, plaintextBytes);

        return JsonSerializer.Deserialize<T>(plaintextBytes)!;
    }
}
