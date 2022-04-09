using System.Collections.Generic;

namespace Common.Tests;

public record HelloE(Aes256<string> Value, IList<Aes256<World>> Worlds);
