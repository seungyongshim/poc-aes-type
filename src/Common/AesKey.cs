using System.ComponentModel.DataAnnotations;

namespace Common;

public readonly record struct AesKey([property: MinLength(32), MaxLength(32)] byte[] Value);
