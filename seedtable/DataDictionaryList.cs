using System.Collections.Generic;

namespace SeedTable {
    class DataDictionaryList {
        public IEnumerable<Dictionary<string, string>> table { get; private set; }
        public DataDictionaryList(IEnumerable<Dictionary<string, string>> table) {
            this.table = table;
        }
        public Dictionary<string, Dictionary<string, string>> ToDictionaryDictionary() {
            var dic = new Dictionary<string, Dictionary<string, string>>();
            foreach (var row in this.table) {
                dic["data" + row["id"]] = row;
            }
            return dic;
        }
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> ToSeparatedDictionaryDictionary(int pre_cut = 0, int post_cut = 0) {
            var dic = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
            foreach (var row in this.table) {
                var id = row["id"];
                if (id == null) continue;
                var id_len = id.Length;
                if (id_len == 0) continue; // idが空行なものはスキップ
                var use_id_len = id_len - pre_cut - post_cut;
                var cut_id_key = "data" + id.Substring(pre_cut, use_id_len < 0 ? 0 : use_id_len);
                if (!dic.ContainsKey(cut_id_key)) dic[cut_id_key] = new Dictionary<string, Dictionary<string, string>>();
                dic[cut_id_key]["data" + id] = row;
            }
            return dic;
        }
    }
}