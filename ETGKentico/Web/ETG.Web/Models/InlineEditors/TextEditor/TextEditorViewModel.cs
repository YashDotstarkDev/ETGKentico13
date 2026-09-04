using ETG.Web.Models.InlineEditors.TextEditor;
using Kentico.Forms.Web.Mvc;
using Kentico.Components.Web.Mvc.FormComponents;

/*[assembly: RegisterFormComponent("ETG.TextEditor",  typeof(TextEditorViewModel), "TextEditor",ViewName = "InlineEditors/_TextEditor")]
*/
namespace ETG.Web.Models.InlineEditors.TextEditor
{
    public class TextEditorViewModel : InlineEditorViewModel
    {
        public string Text { get; set; }
        public bool EnableFormatting { get; set; } = true;
    }
}