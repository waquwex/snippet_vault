// Auto resize SnippetDescription textarea when adding new content

window.addEventListener("load", () => {
    setupSnippedDescriptionTextArea();
    setupCodeMirror();
});

let setupSnippedDescriptionTextArea = () => {
    let sd = document.getElementsByClassName("snippet-description")[0];

    sd.setAttribute("style", "height:" + sd.scrollHeight + "px;overflow-y:hidden;");

    sd.oninput = (event) => {
        sd.style.height = 0;
        sd.style.height = sd.scrollHeight + "px";
    }
}

let setupCodeMirror = () => {
    let editor = CodeMirror.fromTextArea(document.getElementsByClassName("snippet-body")[0], {
        mode: null,
        styleActiveLine: true,
        lineNumbers: true,
        lineWrapping: false,
        indentUnit: 4,
        viewportMargin: Infinity,
        spellCheck: false,
        autocorrect: false
    });

    editor.setSize(null, "100%");

    let mac = CodeMirror.keyMap.default == CodeMirror.keyMap.macDefault;
    CodeMirror.keyMap.default[(mac ? "Cmd" : "Ctrl") + "-Space"] = "autocomplete";
    CodeMirror.modeURL = "/scripts/libs/codemirror5/mode/%N/%N.js";
    let snippetFileName = document.getElementsByClassName("snippet-file-name")[0];
    snippetFileName.oninput = (event) => {
        changeMode(event.target.value);
    }

    function changeMode(fileName) {
        let mode, spec;
        if (m = /.+\.([^.]+)$/.exec(fileName)) {
            let extension = m[1];
            let info = CodeMirror.findModeByExtension(extension);
            if (info) {
                mode = info.mode;
                spec = info.mime;
            }
        }
        if (mode) {
            editor.setOption("mode", spec);
            CodeMirror.autoLoadMode(editor, mode);
        }
    }
}