window.addEventListener("load", () => {
    setupCodeMirror();
});

let setupCodeMirror = () => {
    const snippets = document.getElementsByClassName("snippet");

    for (let i = 0; i < snippets.length; i++) {
        let editor = CodeMirror.fromTextArea(snippets[i].getElementsByClassName("snippet-body-read")[0], {
            mode: null,
            lineNumbers: true,
            lineWrapping: false,
            indentUnit: 4,
            spellCheck: false,
            autocorrect: false,
            readOnly: true
        });

        console.log(editor);

        let totalLines = editor.doc.size;
        editor.setSize(null, totalLines * 15 + 11);

        let mac = CodeMirror.keyMap.default == CodeMirror.keyMap.macDefault;
        CodeMirror.keyMap.default[(mac ? "Cmd" : "Ctrl") + "-Space"] = "autocomplete";
        CodeMirror.modeURL = "/scripts/libs/codemirror5/mode/%N/%N.js";
        let snippetFileName = snippets[i].getElementsByClassName("snippet-file-name-read")[0].innerText;

        changeMode(editor, snippetFileName);
    }
}

function changeMode(editor, fileName) {
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