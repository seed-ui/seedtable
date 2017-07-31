using System.Linq;
using System.Collections.Generic;

namespace SeedTable {
    /** レコードDictionaryのリスト */
    public class DataDictionaryList {
        /** [{id: 1, name: "foo"}, ...] */
        public IEnumerable<Dictionary<string, object>> Table { get; private set; }

        /**
         * コンストラクタ
         *
         * 渡されたレコードリストからidが空白であるものを省き、値が空白であるデータをnullに変換して受け取る
         *
         * @param table [{id: 1, name: "foo"}, ...]的なデータ
         */
        public DataDictionaryList(IEnumerable<Dictionary<string, object>> table) {
            Table = table
                .Where(rowData => rowData.ContainsKey("id") && rowData["id"] != null && (rowData["id"].ToString()).Length != 0)
                .Select(rowData => rowData.ToDictionary(pair => pair.Key, pair => pair.Value is string && ((string) pair.Value) == "" ? null : pair.Value));
        }

        /**
         * {data1: {id: 1, name: "foo"}, ...}形式で返す
         */
        public Dictionary<string, Dictionary<string, object>> ToDictionaryDictionary() {
            var dic = new Dictionary<string, Dictionary<string, object>>();
            foreach (var row in Table) {
                dic["data" + row["id"]] = row;
            }
            return dic;
        }

        /**
         * {"1": {id: 1, name: "foo"}, ...}形式で返す
         */
        public Dictionary<string, Dictionary<string, object>> IndexById() {
            var dic = new Dictionary<string, Dictionary<string, object>>();
            foreach (var row in Table) {
                dic[row["id"].ToString()] = row;
            }
            return dic;
        }

        /**
         * カットされたIDグループごとのレコードリスト
         *
         * {"data101": [{id: 10101, name: "foo"}], ...}形式で返す
         */
        public Dictionary<string, List<Dictionary<string, object>>> ToSeparated(int preCut = 0, int postCut = 0) {
            var dic = new Dictionary<string, List<Dictionary<string, object>>>();
            foreach (var row in Table) {
                var id = row["id"].ToString();
                var subdivideId = SubdivideId(id, preCut, postCut);
                if (subdivideId == null) continue; // idが空なものはスキップ
                var cutIdKey = "data" + subdivideId;
                if (!dic.ContainsKey(cutIdKey)) dic[cutIdKey] = new List<Dictionary<string, object>>();
                dic[cutIdKey].Add(row);
            }
            return dic;
        }

        /**
         * カットされたIDグループごとのレコードリスト
         *
         * {"data101": {data10101: {id: 10101, name: "foo"}}, ...}形式で返す
         */
        public Dictionary<string, Dictionary<string, Dictionary<string, object>>> ToSeparatedDictionaryDictionary(int preCut = 0, int postCut = 0) {
            var dic = new Dictionary<string, Dictionary<string, Dictionary<string, object>>>();
            foreach (var row in Table) {
                var id = row["id"].ToString();
                var subdivideId = SubdivideId(id, preCut, postCut);
                if (subdivideId == null) continue; // idが空なものはスキップ
                var cutIdKey = "data" + subdivideId;
                if (!dic.ContainsKey(cutIdKey)) dic[cutIdKey] = new Dictionary<string, Dictionary<string, object>>();
                dic[cutIdKey]["data" + id] = row;
            }
            return dic;
        }

        private string SubdivideId(string id, int preCut = 0, int postCut = 0) {
            var idLength = id.Length;
            if (idLength == 0) return null; // idが空なものはスキップ
            var useIdLength = idLength - preCut - postCut;
            return id.Substring(preCut > idLength ? idLength : preCut, useIdLength < 0 ? 0 : useIdLength);
        }
    }
}
