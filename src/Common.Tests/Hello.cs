using System.Collections.Generic;

namespace Common.Tests;

public record Hello(string Value, IList<World> Worlds);
