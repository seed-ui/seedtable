using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xunit;

using SeedTable;

namespace seedtable_test {
    public class WildcardTest {
        public static IEnumerable<object[]> Examples() {
            yield return new object[] {
                "abc",
                new string[] { "abc" },
                new string[] { "adc", "abcd" },
            };
            yield return new object[] {
                "a?c",
                new string[] { "abc", "adc" },
                new string[] { "abcd", "ac" },
            };
            yield return new object[] {
                "a*c",
                new string[] { "abc", "abbc", "ac" },
                new string[] { "abcd", "a", "acd" },
            };
            yield return new object[] {
                "a*",
                new string[] { "abc", "a" },
                new string[] { "baa" },
            };
            yield return new object[] {
                "__*",
                new string[] { "__dummy", "__memo", "__" },
                new string[] { "_", "_a", "o" },
            };
        }

        [Theory]
        [MemberData(nameof(Examples))]
        public void Wildcard(string wildcardStr, string[] matches, string[] notMatches) {
            var wildcard = new Wildcard(wildcardStr);
            foreach (var match in matches) {
                Assert.True(wildcard.IsMatch(match));
            }
            foreach (var notMatch in notMatches) {
                Assert.False(wildcard.IsMatch(notMatch));
            }
        }
    }
}
