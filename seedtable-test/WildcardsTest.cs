using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xunit;

using SeedTable;

namespace seedtable_test {
    public class WildcardsTest {
        public static IEnumerable<object[]> Examples() {
            yield return new object[] {
                new string[] { "abc", "efg" },
                new string[] { "abc", "efg" },
                new string[] { "adc", "" },
            };
            yield return new object[] {
                new string[] { "a?c", "abcd" },
                new string[] { "abc", "adc", "abcd" },
                new string[] { "ac" },
            };
            yield return new object[] {
                new string[] { "a*c", "e?g" },
                new string[] { "abc", "abbc", "efg" },
                new string[] { "abcd", "a", "effg" },
            };
        }

        [Theory]
        [MemberData(nameof(Examples))]
        public void Wildcards(string[] wildcardStrs, string[] matches, string[] notMatches) {
            var wildcards = new Wildcards<Wildcard>(wildcardStrs.Select(wildcardStr => new Wildcard(wildcardStr)));
            foreach (var match in matches) {
                Assert.True(wildcards.Contains(match));
            }
            foreach (var notMatch in notMatches) {
                Assert.False(wildcards.Contains(notMatch));
            }
        }
    }
}
