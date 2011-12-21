using System.ComponentModel;
using System.Web.UI;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// The individual pages for a tab page class
    /// </summary>
    [ParseChildren(true), PersistChildren(false),DefaultProperty("Content")]
    public class TabPage : ViewStateBase
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Bindable(true),DefaultValue(null)]
        public string Id
        {
            get
            {
                if(ViewState["Id"] != null)
                    return (string)ViewState["Id"];
                else
                    return string.Empty;
            }
            set { ViewState["Id"] = value; }
        }


        /// <summary>
        /// The title of the page
        /// </summary>
        [Bindable(true),DefaultValue(null)]
        public string Title
        {
            get
            {
                if (ViewState["Title"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Title"];
            }
            set { ViewState["Title"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TabPage"/> is visible.
        /// </summary>
        [Bindable(true),DefaultValue(true)]
        public bool Visible
        {
            get
            {
                if(ViewState["Visible"] != null)
                    return (bool)ViewState["Visible"];
                else
                    return true;
            }
            set { ViewState["Visible"] = value; }
        }

        #region Data Properties

        /// <summary>
        /// The contents of the box
        /// </summary>
        [Browsable(false), PersistenceMode(PersistenceMode.InnerDefaultProperty), TemplateInstance(TemplateInstance.Single)]
        public ITemplate Content
        {
            get { return _contentTemplate; }
            set
            {
                _contentTemplate = value;
                if (_contentTemplate != null)
                    CreateContents();
            }
        }

        #endregion

        #region Protected

        /// <summary>
        /// The template container control as the parent of the templated controls
        /// </summary>
        [Browsable(false)]
        protected Control ContentTemplateContainer
        {
            get
            {
                if (_contentTemplateContainer == null)
                {
                    _contentTemplateContainer = new Control();
                    if (Owner != null)
                        Owner.Controls.Add(_contentTemplateContainer);
                }

                return _contentTemplateContainer;
            }
        }

        #endregion

        #region Internal

        internal TabControl Owner
        {
            get { return _owner; }
            set
            {
                if (_owner == null && value != null && _contentTemplateContainer != null)
                    value.Controls.Add(_contentTemplateContainer);
                _owner = value;
            }
        }

        internal void SetDirty()
        {
            ViewState.SetDirty(true);
        }

        internal void PreRender()
        {
            if(!Visible)
                ContentTemplateContainer.Visible = false;
            else
            {
                int idx = _owner.Pages.IndexOf(this);
                if(_owner.ActivePageIndex != idx)
                {
                    if(_owner.TabStyle == TabStyle.ServerSide)
                        ContentTemplateContainer.Visible = false;
                    else
                        ContentTemplateContainer.Visible = true;
                }
                else
                    ContentTemplateContainer.Visible = true;
            }
        }

        internal void RenderButton(HtmlTextWriter writer, int idx)
        {
            if(!Visible)
                return;

            if (_owner.UsejQueryStyle || _owner.TabStyle == TabStyle.jQuery)
            {
                if (_owner.ActivePageIndex == idx)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-state-default ui-corner-top ui-tabs-selected ui-state-active");
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#" + _owner.ClientID + "_page_" + idx.ToString());
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-state-default ui-corner-top");
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    if (_owner.TabStyle != TabStyle.ServerSide)
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "#" + _owner.ClientID + "_page_" + idx.ToString());
                    else
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, _owner.Page.ClientScript.GetPostBackClientHyperlink(_owner, idx.ToString()));
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Id, _owner.ClientID + "_btn_" + idx.ToString());
                writer.AddAttribute("oa", idx.ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write(Title);
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
            else if (string.IsNullOrEmpty(_owner.ActiveTabStyle.BackImage))
            {
                if (_owner.ActivePageIndex == idx)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, _owner.ActiveTabStyle.RenderClass);
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#" + _owner.ClientID + "_page_" + idx.ToString());
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, _owner.InactiveTabStyle.RenderClass);
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    if (_owner.TabStyle != TabStyle.ServerSide)
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, "#" + _owner.ClientID + "_page_" + idx.ToString());
                    else
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, _owner.Page.ClientScript.GetPostBackClientHyperlink(_owner, idx.ToString()));
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Id, _owner.ClientID + "_btn_" + idx.ToString());
                writer.AddAttribute("oa", idx.ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write(Title);
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
            else
            {
                if (_owner.ActivePageIndex == idx)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, _owner.ActiveTabStyle.RenderClass);
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, _owner._leftGfxAct.ImageSize.ToString() + "px");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, _owner._leftGfxAct.FileName + Graphic.WebGfx.Extension);
                    writer.AddStyleAttribute("background-repeat", "no-repeat");
                    writer.AddStyleAttribute("float", "left");
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, _owner.ClientID + "_ld_" + idx.ToString());
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);

                    writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingRight, _owner._rightGfxAct.ImageSize.ToString() + "px");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, _owner._rightGfxAct.FileName + Graphic.WebGfx.Extension);
                    writer.AddStyleAttribute("background-position", "right top");
                    writer.AddStyleAttribute("background-repeat", "no-repeat");
                    writer.AddStyleAttribute("float", "left");
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, _owner.ClientID + "_rd_" + idx.ToString());
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);

                    writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, _owner._midGfxAct.FileName + Graphic.WebGfx.Extension);
                    writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingTop, _owner.ActiveTabStyle.ImageTopSize.ToString() + "px");
                    writer.AddStyleAttribute("float", "left");
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, _owner.InactiveTabStyle.RenderClass);
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, _owner._leftGfxInact.ImageSize.ToString() + "px");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, _owner._leftGfxInact.FileName + Graphic.WebGfx.Extension);
                    writer.AddStyleAttribute("background-repeat", "no-repeat");
                    writer.AddStyleAttribute("float", "left");
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, _owner.ClientID + "_ld_" + idx.ToString());
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);

                    writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingRight, _owner._rightGfxInact.ImageSize.ToString() + "px");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, _owner._rightGfxInact.FileName + Graphic.WebGfx.Extension);
                    writer.AddStyleAttribute("background-repeat", "no-repeat");
                    writer.AddStyleAttribute("background-position", "right top");
                    writer.AddStyleAttribute("float", "left");
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, _owner.ClientID + "_rd_" + idx.ToString());
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);

                    writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, _owner._midGfxInact.FileName + Graphic.WebGfx.Extension);
                    writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingTop, _owner.InactiveTabStyle.ImageTopSize.ToString() + "px");
                    writer.AddStyleAttribute("float", "left");
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, _owner.Page.ClientScript.GetPostBackClientHyperlink(_owner, idx.ToString()));
                }

                writer.AddAttribute(HtmlTextWriterAttribute.Id, _owner.ClientID + "_btn_" + idx.ToString());
                writer.AddAttribute("oa", idx.ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write(Title);
                writer.RenderEndTag();

                writer.RenderEndTag();
                writer.RenderEndTag();

                writer.RenderEndTag();
            }
        }

        internal void RenderPage(HtmlTextWriter writer, int idx)
        {
            if (ContentTemplateContainer.Visible)
            {
                if(_owner.ActivePageIndex != idx && _owner.TabStyle == TabStyle.ClientSide)
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");

                if (_owner.UsejQueryStyle || _owner.TabStyle == TabStyle.jQuery)
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-tabs-panel ui-widget-content ui-corner-bottom");

                writer.AddAttribute(HtmlTextWriterAttribute.Id, _owner.ClientID + "_page_" + idx.ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
            }

            ContentTemplateContainer.RenderControl(writer);

            if (ContentTemplateContainer.Visible)
                writer.RenderEndTag();
        }

        #endregion

        #region Private

        private void ClearContent()
        {
            ContentTemplateContainer.Controls.Clear();
            if (Owner != null)
                Owner.Controls.Remove(_contentTemplateContainer);
            _contentTemplateContainer = null;
        }

        private void CreateContents()
        {
            if (_contentTemplateContainer == null)
            {
                _contentTemplateContainer = new Control();
                if (_contentTemplate != null)
                    _contentTemplate.InstantiateIn(_contentTemplateContainer);
                if (Owner != null)
                    Owner.Controls.Add(_contentTemplateContainer);
            }
            else if (_contentTemplate != null)
                _contentTemplate.InstantiateIn(_contentTemplateContainer);
        }

        private TabControl _owner;
        private ITemplate _contentTemplate;
        private Control _contentTemplateContainer;

        #endregion
    }
}
