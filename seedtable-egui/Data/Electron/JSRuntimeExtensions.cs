using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace seedtable_egui.Data.Electron {
    public static class JSRuntimeExtensions {
        public static ValueTask<OpenDialogResult> ShowOpenDialog(this IJSRuntime js, OpenDialogOption option) {
            return js.InvokeAsync<OpenDialogResult>("showOpenDialog", option);
        }

        public static ValueTask<SaveDialogResult> ShowSaveDialog(this IJSRuntime js, SaveDialogOption option) {
            return js.InvokeAsync<SaveDialogResult>("showSaveDialog", option);
        }

        public static ValueTask ShowErrorBox(this IJSRuntime js, string content = null, string title = "エラー") {
            return js.InvokeVoidAsync("showErrorBox", title, content);
        }
    }
}
