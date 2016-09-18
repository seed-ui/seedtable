using System;
using System.Collections.Generic;

namespace SeedTable {
    interface IExcelData : IDisposable {
        IEnumerable<string> SheetNames { get; }
        SeedTableBase GetSeedTable(string sheetName, int columnNamesRowIndex = 2, int dataStartRowIndex = 3, IEnumerable<string> ignoreColumnNames = null, string versionColumnName = null);
        void Save();
        void SaveAs(string file);
    }

    abstract class SeedTableBase {
        public int ColumnNamesRowIndex { get; }
        public int DataStartRowIndex { get; }
        public HashSet<string> IgnoreColumnNames { get; }
        public string VersionColumnName { get; }

        public List<Exception> Errors { get; protected set; } = new List<Exception>();

        public SeedTableBase(int columnNamesRowIndex = 2, int dataStartRowIndex = 3, IEnumerable<string> ignoreColumnNames = null, string versionColumnName = null) {
            ColumnNamesRowIndex = columnNamesRowIndex;
            DataStartRowIndex = dataStartRowIndex;
            IgnoreColumnNames = ignoreColumnNames == null ? new HashSet<string>() : new HashSet<string>(ignoreColumnNames);
            VersionColumnName = versionColumnName;
        }

        public abstract string SheetName { get; }
        public abstract DataDictionaryList ExcelToData(string requireVersion = "");
        public abstract void DataToExcel(DataDictionaryList data, bool delete = false);
    }

    class DuplicateColumnNameException : InvalidOperationException {
        public DuplicateColumnNameException(string message) : base(message) { }
    }

    class NoIdColumnException : NotSupportedException {
        public NoIdColumnException(string message) : base(message) { }
    }

    class SheetNotFoundException : InvalidOperationException {
        public SheetNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
