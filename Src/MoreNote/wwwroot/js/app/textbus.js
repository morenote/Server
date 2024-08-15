let textbusEditor;
function initTextbus(){

    textbusEditor = textbus.createEditor(document.getElementById('textbus'));
    textbusEditor.onChange.subscribe(function() {
       console.log(editor.getContents());
     })
}