using System.ComponentModel;
using System.Web.UI;

namespace ESWCtrls
{

    /// <summary>
    /// A Menu Item that only displays an icon and text
    /// </summary>
    public class MenuItemText : MenuItem
    {
        #region Constructors

        /// <summary>
        /// Creates a new menu item
        /// </summary>
        public MenuItemText() : base() { }

        /// <summary>
        /// Creates a new menu item
        /// </summary>
        /// <param name="ID">For identifying the item in code</param>
        /// <param name="Text">The text for the item</param>
        public MenuItemText(string ID, string Text)
            : base(ID)
        {
            this.Text = Text;
        }

        /// <summary>
        /// Creates a new menu item
        /// </summary>
        /// <param name="ID">For identifying the item in code</param>
        /// <param name="Text">The text for the item</param>
        /// <param name="Icon">The icon</param>
        public MenuItemText(string ID, string Text, string Icon)
            : this(ID, Text)
        {
            this.Icon = Icon;
        }

        #endregion

        #region Properties

        #region Appearance

        /// <summary>
        /// The text to display for the node
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(string), "string.Empty")]
        public string Text
        {
            get
            {
                if (ViewState["Text"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Text"];
            }
            set { ViewState["Text"] = value; }
        }

        /// <summary>
        /// The icon for the node
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(string), "string.Empty"), UrlProperty(), Editor("System.Web.UI.Design.ImageUrlEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor))]
        public string Icon
        {
            get
            {
                if (ViewState["Icon"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Icon"];
            }
            set { ViewState["Icon"] = value; }
        }

        #endregion

        #endregion

        #region Render

        internal override void Render(HtmlTextWriter writer, bool topLevel)
        {
            RenderStart(writer, topLevel);
            Owner.RenderBeforeItemContent(this, writer, topLevel);
            RenderIcon(writer, topLevel);
            writer.Write(Text);
            Owner.RenderAfterItemContent(this, writer, topLevel);
            RenderEnd(writer, topLevel);
        }

        /// <summary>
        /// Renders the icon part
        /// </summary>
        protected void RenderIcon(HtmlTextWriter writer, bool topLevel)
        {
            bool addEmpty = false;
            if (topLevel)
            {
                if (!Owner.TopIconSize.IsEmpty)
                {
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Width, Owner.TopIconSize.Width.ToString() + "px");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Height, Owner.TopIconSize.Height.ToString() + "px");
                    addEmpty = true;
                }
            }
            else if (!Owner.IconSize.IsEmpty)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, Owner.IconSize.Width.ToString() + "px");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Height, Owner.IconSize.Height.ToString() + "px");
                addEmpty = true;
            }

            if (!string.IsNullOrEmpty(Icon))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, Owner.Page.ResolveUrl(Icon));
                writer.AddAttribute(HtmlTextWriterAttribute.Alt, Text + " Icon");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
            }
            else if (addEmpty)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "inline-block");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Overflow, "hidden");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write("&nbsp;");
                writer.RenderEndTag();
            }
        }

        #endregion
    }
}
