using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace seedtable_egui.Data.Electron {
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class SaveDialogResult {
        public bool Canceled { get; set; }
        public string FilePath { get; set; }
    }
}
