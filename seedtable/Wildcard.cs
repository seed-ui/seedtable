using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace SeedTable {
    public class Wildcards<T> : List<T> where T : Wildcard {
        public Wildcards() : base() { }

        public Wildcards(IEnumerable<T> wildcards) : base(
            wildcards.Where(
                wildcard => wildcard.MatchType == WildcardMatchType.Exact
            ).Concat(
                wildcards.Where(
                    wildcard => wildcard.MatchType == WildcardMatchType.Wildcard
                )
            ).Concat(
                wildcards.Where(
                    wildcard => wildcard.MatchType == WildcardMatchType.All
                ).Take(1)
            )
        ) { }

        public T Find(string name) {
            return Find(matchName => matchName.IsMatch(name));
        }

        public bool Contains(string name) {
            return Find(name) != null;
        }
    }

    public class Wildcard {
        public string Name { get; }
        public WildcardMatchType MatchType { get; }
        private Regex NameMatcher { get; }

        public Wildcard(string name) {
            Name = name;
            if (Name == "*") {
                MatchType = WildcardMatchType.All;
            } else if (Name.Contains("*") || Name.Contains("?")) {
                NameMatcher = new Regex("^" + Regex.Escape(name).Replace(@"\*", ".*").Replace(@"\?", ".") + "$");
                MatchType = WildcardMatchType.Wildcard;
            } else {
                MatchType = WildcardMatchType.Exact;
            }
        }

        public bool IsMatch(string name) {
            switch (MatchType) {
                case WildcardMatchType.Exact:
                    return name == Name;
                case WildcardMatchType.Wildcard:
                    return NameMatcher.IsMatch(name);
                default:
                    return true;
            }
        }
    }

    public enum WildcardMatchType {
        All = 0,
        Wildcard,
        Exact,
    }
}
