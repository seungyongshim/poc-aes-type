using System.Security.Cryptography;
using System.Text.Json;

namespace Common;

public static class Aes256
{
    public static Aes256<T> Encrypt<T>(in T target, in AesKey key)
    {
        using var aes = new AesGcm(key.Value);

        var plaintextBytes = JsonSerializer.SerializeToUtf8Bytes(target);
        var body = new byte[plaintextBytes.Length];
        var tag = new byte[AesGcm.TagByteSizes.MaxSize];
        var iv = RandomNumberGenerator.GetBytes(AesGcm.NonceByteSizes.MaxSize);

        aes.Encrypt(iv, plaintextBytes, body, tag);

        return new Aes256<T>(body, iv, tag);
    }
}

public record Aes256<T>(byte[] Body, byte[] IV, byte[] Tag)
{
    public static implicit operator Aes256<T>(in (T Target, AesKey Key) v) =>
        Aes256.Encrypt(v.Target, v.Key);

    public T Decrypt(in AesKey key)
    {
        using var aes = new AesGcm(key.Value);

        var plaintextBytes = new byte[Body.Length];
        aes.Decrypt(IV, Body, Tag, plaintextBytes);

        return JsonSerializer.Deserialize<T>(plaintextBytes)!;
    }
}
