using System.ComponentModel;
using System.Web.UI;

namespace ESWCtrls
{
    /// <summary>
    /// menu item for template controls
    /// </summary>
    public class MenuItemTemplate : MenuItem
    {
        /// <summary>
        /// Creates a new menu item template
        /// </summary>
        public MenuItemTemplate() : base() { }

        #region Data

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
                if (_contentTemplate != null)
                    CreateContents();
            }
        }

        #endregion

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

        private ITemplate _contentTemplate;
        private Control _contentTemplateContainer;

        internal override Menu Owner
        {
            get { return base.Owner; }
            set
            {
                if (Owner == null && value != null && _contentTemplateContainer != null)
                    value.Controls.Add(_contentTemplateContainer);
                base.Owner = value;
            }
        }

        internal override void Render(HtmlTextWriter writer, bool topLevel)
        {
            RenderStart(writer, topLevel);
            Owner.RenderBeforeItemContent(this, writer, topLevel);
            ContentTemplateContainer.RenderControl(writer);
            Owner.RenderAfterItemContent(this, writer, topLevel);
            RenderEnd(writer, topLevel);
        }
    }
}
