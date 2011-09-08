using System.ComponentModel;
using System.Web.UI;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// The individual sections for an Accordion
    /// </summary>
    public class AccordionSection : ViewStateBase
    {
        /// <summary>
        /// The title of the page
        /// </summary>
        public string Title
        {
            get
            {
                if(ViewState["Title"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Title"];
            }
            set { ViewState["Title"] = value; }
        }

        #region Data Properties

        /// <summary>
        /// The contents of the box
        /// </summary>
        [Browsable(false), PersistenceMode(PersistenceMode.InnerProperty), TemplateInstance(TemplateInstance.Single)]
        public ITemplate Content
        {
            get { return _contentTemplate; }
            set
            {
                _contentTemplate = value;
                if(_contentTemplate != null)
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
                if(_contentTemplateContainer == null)
                {
                    _contentTemplateContainer = new Control();
                    if(Owner != null)
                        Owner.Controls.Add(_contentTemplateContainer);
                }

                return _contentTemplateContainer;
            }
        }

        #endregion

        #region Internal

        internal Accordion Owner
        {
            get { return _owner; }
            set
            {
                if(_owner == null && value != null && _contentTemplateContainer != null)
                    value.Controls.Add(_contentTemplateContainer);
                _owner = value;
            }
        }

        internal void SetDirty()
        {
            ViewState.SetDirty(true);
        }

        internal void Render(HtmlTextWriter writer, int idx)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Id, _owner.ClientID + "_head_" + idx.ToString());
            writer.AddAttribute("oa", idx.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.H3);
            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#" + _owner.ClientID + "_sect_" + idx.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(Title);
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Id, _owner.ClientID + "_sect_" + idx.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            ContentTemplateContainer.RenderControl(writer);
            writer.RenderEndTag();

            writer.RenderEndTag();
        }

        #endregion

        #region Private

        private void ClearContent()
        {
            ContentTemplateContainer.Controls.Clear();
            if(Owner != null)
                Owner.Controls.Remove(_contentTemplateContainer);
            _contentTemplateContainer = null;
        }

        private void CreateContents()
        {
            if(_contentTemplateContainer == null)
            {
                _contentTemplateContainer = new Control();
                if(_contentTemplate != null)
                    _contentTemplate.InstantiateIn(_contentTemplateContainer);
                if(Owner != null)
                    Owner.Controls.Add(_contentTemplateContainer);
            }
            else if(_contentTemplate != null)
                _contentTemplate.InstantiateIn(_contentTemplateContainer);
        }

        private ITemplate _contentTemplate;
        private Control _contentTemplateContainer;
        private Accordion _owner;

        #endregion
    }
}
