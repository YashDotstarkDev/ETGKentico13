using ETG.Web.Models.FormComponents;
using Kentico.Forms.Web.Mvc;
[assembly: RegisterFormComponent("CKEditorComponent", typeof(CKEditorComponent), "CKEditorComponent", Description = "CKEditor Component")]
namespace ETG.Web.Models.FormComponents
{
    public class CKEditorComponent : FormComponent<CKEditorProperties, string>
    {
        public const string IDENTIFIER = "CKEditorComponent";
        [BindableProperty]
        // Used to store the value of the input field of the component
        public string Text { get; set; } = "";

        public override string GetValue()
        {
            if (Text == null)
            {
                return string.Empty;
            }
            return Text;
        }

        public override void SetValue(string value)
        {
            Text = value;
        }
    }
}