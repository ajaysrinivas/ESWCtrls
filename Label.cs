using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// Unlike the asp:label that creates <span></span> this control produces <label></label>
    /// </summary>
    [DefaultProperty("Text"), ToolboxData("<{0}:label runat=server></{0}:label>"), ParseChildren(true, "Text")]
    public class Label : WebControl
    {

        /// <summary>
        /// The text for the label
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(""), PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        public string Text
        {
            get
            {
                if(ViewState["Text"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Text"];
            }
            set { ViewState["Text"] = value; }
        }

        /// <summary>
        /// The control the label is for
        /// </summary>
        [Bindable(true), Category("Behavior"), DefaultValue("")]
        public string ForControl
        {
            get
            {
                if(ViewState["ForControl"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["ForControl"];
            }
            set { ViewState["ForControl"] = value; }
        }

        /// 
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            if(!string.IsNullOrEmpty(ForControl))
            {
                Control ctrl = Util.FindControlRecursive(Page, ForControl);
                if(ctrl == null)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.For, ForControl);
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.For, ctrl.ClientID);
                }
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Label);
            writer.Write(Text);
            writer.RenderEndTag();
        }

    }
}