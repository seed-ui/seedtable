using System;

namespace SeedTable {
    [Flags]
    public enum OnOperation {
        From = 1,
        To   = 1 << 1,
    }
}
