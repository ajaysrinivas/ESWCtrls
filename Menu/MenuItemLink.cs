using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{

    /// <summary>
    /// The individual items in a menu
    /// </summary>
    [ParseChildren(true), PersistChildren(false)]
    public class MenuItemLink : MenuItemText
    {
        #region Constructors

        /// <summary>
        /// An empty menu item (Default Constructor)
        /// </summary>
        public MenuItemLink() : base() { }

        /// <summary>
        /// Creates a new menu item
        /// </summary>
        /// <param name="ID">For identifying the item in code</param>
        /// <param name="Text">The text for the item</param>
        /// <param name="Link">The url to redirect to when clicked</param>
        public MenuItemLink(string ID, string Text, string Link)
            : base(ID, Text)
        {
            this.Link = Link;
        }

        /// <summary>
        /// Creates a new menu item
        /// </summary>
        /// <param name="ID">For identifying the item in code</param>
        /// <param name="Text">The text for the item</param>
        /// <param name="Icon">The icon</param>
        /// <param name="Link">The url to redirect to when clicked</param>
        public MenuItemLink(string ID, string Text, string Icon, string Link)
            : this(ID, Text, Link)
        {
            this.Icon = Icon;
        }

        /// <summary>
        /// Creates a new menu item
        /// </summary>
        /// <param name="ID">For identifying the item in code</param>
        /// <param name="Text">The text for the item</param>
        /// <param name="CommandName">The command name to use when clicked</param>
        /// <param name="CommandArgument">The command argument to use</param>
        public MenuItemLink(string ID, string Text, string CommandName, object CommandArgument)
            : base(ID, Text)
        {
            this.CommandName = CommandName;
            this.CommandArgument = CommandArgument;
        }

        /// <summary>
        /// Creates a new menu item
        /// </summary>
        /// <param name="ID">For identifying the item in code</param>
        /// <param name="Text">The text for the item</param>
        /// <param name="Icon">The icon</param>
        /// <param name="CommandName">The command name to use when clicked</param>
        /// <param name="CommandArgument">The command argument to use</param>
        public MenuItemLink(string ID, string Text, string Icon, string CommandName, object CommandArgument)
            : this(ID, Text, CommandName, CommandArgument)
        {
            this.Icon = Icon;
        }

        #endregion

        #region Properties

        #region Appearance

        /// <summary>
        /// The tooltip to display for the node
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(string), "string.Empty")]
        public string ToolTip
        {
            get
            {
                if (ViewState["ToolTip"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["ToolTip"];
            }
            set { ViewState["ToolTip"] = value; }
        }

        #endregion

        #region Behaviour

        /// <summary>
        /// A url link to  redirect to when the node is clicked
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(string), "string.Empty"), UrlProperty()]
        public string Link
        {
            get
            {
                if (ViewState["Link"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Link"];
            }
            set { ViewState["Link"] = value; }
        }

        /// <summary>
        /// Command name to pass when the node is clicked
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(string), "string.Empty")]
        public string CommandName
        {
            get
            {
                if (ViewState["CommandName"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["CommandName"];
            }
            set { ViewState["CommandName"] = value; }
        }

        /// <summary>
        /// Command argument to pass when the node is clicked
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(object), null)]
        public object CommandArgument
        {
            get { return ViewState["CommandArgument"]; }
            set { ViewState["CommandArgument"] = value; }
        }

        /// <summary>
        /// The client script to envoke when clicked
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(string), "string.Empty")]
        public string OnClientClick
        {
            get
            {
                if (ViewState["OnClientClick"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["OnClientClick"];
            }
            set { ViewState["OnClientClick"] = value; }
        }

        #endregion

        #endregion

        #region Internal

        internal CommandEventArgs EventArgs
        {
            get { return new CommandEventArgs(CommandName, CommandArgument); }
        }

        /// <summary>
        /// Does the rendering of the item
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        /// <param name="topLevel">True if this is a top level item</param>
        internal override void Render(HtmlTextWriter writer, bool topLevel)
        {
            RenderStart(writer, topLevel);

            if (Owner.Enabled)
            {

                if (Enabled)
                {
                    if (!string.IsNullOrEmpty(Link))
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, Owner.Page.ResolveUrl(Link));
                    else if (!string.IsNullOrEmpty(CommandName))
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, Owner.Page.ClientScript.GetPostBackClientHyperlink(Owner, InternalID));
                    else
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");

                    if (!string.IsNullOrEmpty(OnClientClick))
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, OnClientClick);
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "return false");
                }
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "return false");
            }

            if (!topLevel)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "block");

            writer.RenderBeginTag(HtmlTextWriterTag.A);

            Owner.RenderBeforeItemContent(this, writer, topLevel);
            RenderIcon(writer, topLevel);
            writer.Write(Text);
            Owner.RenderAfterItemContent(this, writer, topLevel);

            writer.RenderEndTag();

            RenderEnd(writer, topLevel);
        }

        #endregion
    }
}