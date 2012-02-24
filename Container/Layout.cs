using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// The layout control
    /// </summary>
    [ToolboxData("<{0}:layout runat=\"server\"></{0}:layout>"), ParseChildren(true), PersistChildren(false)]
    public class Layout : WebControl
    {
        #region Properties

        /// <summary>
        /// The style for the resize bars
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(PaddingStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public AdvStyle BarStyle
        {
            get
            {
                if(_barStyle == null)
                {
                    _barStyle = new AdvStyle();
                    if(IsTrackingViewState)
                        ((IStateManager)_barStyle).TrackViewState();
                }

                return _barStyle;
            }
        }

        /// <summary>
        /// The style for the central content
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(PaddingStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public AdvStyle ContentStyle
        {
            get
            {
                if(_contentStyle == null)
                {
                    _contentStyle = new AdvStyle();
                    if(IsTrackingViewState)
                        ((IStateManager)_contentStyle).TrackViewState();
                }

                return _contentStyle;
            }
        }

        /// <summary>
        /// Whether to use ghosting on the resizing bars
        /// </summary>
        [Category("Appearance"), DefaultValue(false)]
        public bool GhostDragging
        {
            get
            {
                if(ViewState["Ghost"] != null)
                    return (bool)ViewState["Ghost"];
                else
                    return false;
            }
            set { ViewState["Ghost"] = value; }
        }

        /// <summary>
        /// The opacity to use on the ghosted bars
        /// </summary>
        [Category("Appearance"), DefaultValue(0.5)]
        public double GhostOpacity
        {
            get
            {
                if(ViewState["GhostOpacity"] != null)
                    return (double)ViewState["GhostOpacity"];
                else
                    return 0.5;
            }
            set { ViewState["GhostOpacity"] = value; }
        }

        #region Panels

        /// <summary>
        /// The contents of the centre panel
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

        /// <summary>
        /// The Left panel of the payout
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(typeof(LayoutPanel), null), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public LayoutPanel LeftPanel
        {
            get
            {
                if(_left == null)
                {
                    _left = new LayoutPanel(this, "left");
                    if(IsTrackingViewState) ((IStateManager)_left).TrackViewState();
                }

                return _left;
            }
        }

        /// <summary>
        /// The South panel of the payout
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(typeof(LayoutPanel), null), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public LayoutPanel RightPanel
        {
            get
            {
                if(_right == null)
                {
                    _right = new LayoutPanel(this,"right");
                    if(IsTrackingViewState) ((IStateManager)_right).TrackViewState();
                }

                return _right;
            }
        }

        #endregion

        #endregion

        #region Control Events

        ///
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if(!string.IsNullOrEmpty(Page.Request.Params[ClientID + "_store"]))
            {
                string[] parts = Page.Request.Params[ClientID + "_store"].Split(";".ToCharArray());
                foreach(string part in parts)
                {
                    if(part.StartsWith("left"))
                        LeftPanel.OnLoad(part.Replace("left", "").Trim("{}".ToCharArray()));
                    else if(part.StartsWith("right"))
                        RightPanel.OnLoad(part.Replace("right", "").Trim("{}".ToCharArray()));
                }
            }
        }

        ///
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            // Nothing to do so abandon rendering
            if(_contentTemplate == null)
                return;

            if(!ContentStyle.IsEmpty)
                Page.Header.StyleSheet.RegisterStyle(ContentStyle, this);

            if((_left == null || !_left.Visible) && (_right == null || !_right.Visible))
                return;

            if(!BarStyle.IsEmpty)
                Page.Header.StyleSheet.RegisterStyle(BarStyle,this);

            Script.AddResourceScript(Page, "jquery.panels.js");

            List<string> opts = new List<string>();

            opts.Add(string.Format("centrePanel:\"#{0}_centre\"", ClientID));
            opts.Add(string.Format("store:\"#{0}_store\"", ClientID));

            if(!BarStyle.IsEmpty)
                opts.Add(string.Format("barClass:\"{0}\"", BarStyle.RenderClass));

            if(GhostDragging)
                opts.Add("ghostDrag:true");

            if(GhostOpacity != 0.5)
                opts.Add(string.Format("ghostOpacity:{0}", GhostOpacity));

            if(_left != null)
                _left.OnPreRender(opts);
            if(_right != null)
                _right.OnPreRender(opts);

            Script.AddStartupScript(this, ClientID, "ls_panels", opts);
            ScriptManager.RegisterHiddenField(this, ClientID + "_store", "");
        }

        ///
        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            // Nothing to do so abandon rendering the contents
            if(_contentTemplate == null)
            {
                writer.RenderEndTag();
                return;
            }

            if(_left != null)
                _left.Render(writer);

            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_centre");
            if(!ContentStyle.IsEmpty)
                writer.AddAttribute(HtmlTextWriterAttribute.Class, ContentStyle.RenderClass);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            ContentTemplateContainer.RenderControl(writer);
            writer.RenderEndTag();

            if(_right != null)
                _right.Render(writer);

            writer.RenderEndTag();
        }

        #endregion

        #region ViewState

        ///
        protected override void TrackViewState()
        {
            base.TrackViewState();
            if(_left != null) _left.TrackViewState();
            if(_right != null) _right.TrackViewState();
            if(_barStyle != null) ((IStateManager)_barStyle).TrackViewState();
            if(_contentStyle != null) ((IStateManager)_contentStyle).TrackViewState();
        }

        ///
        protected override object SaveViewState()
        {
            object[] state = new object[5];
            state[0] = base.SaveViewState();
            if(_left != null) state[1] = _left.SaveViewState();
            if(_right != null) state[2] = _right.SaveViewState();
            if(_barStyle != null) state[3] = ((IStateManager)_barStyle).SaveViewState();
            if(_contentStyle != null) state[4] = ((IStateManager)_contentStyle).SaveViewState();
            return state;
        }

        ///
        protected override void LoadViewState(object savedState)
        {
           object[] state = (object[])savedState;
           if(state != null)
           {
               if(state[0] != null) base.LoadViewState(state[0]);
               if(state[1] != null) LeftPanel.LoadViewState(state[1]);
               if(state[2] != null) RightPanel.LoadViewState(state[2]);
               if(state[3] != null) ((IStateManager)BarStyle).LoadViewState(state[3]);
               if(state[4] != null) ((IStateManager)ContentStyle).LoadViewState(state[3]);
           }
        }

        #endregion

        #region Template

        ///
        protected Control ContentTemplateContainer
        {
            get
            {
                if(_contentTemplateContainer == null)
                {
                    _contentTemplateContainer = new Control();
                    Controls.Add(_contentTemplateContainer);
                }

                return _contentTemplateContainer;
            }
        }

        private void ClearContent()
        {
            ContentTemplateContainer.Controls.Clear();
            Controls.Remove(_contentTemplateContainer);
            _contentTemplateContainer = null;
        }

        private void CreateContents()
        {
            if(_contentTemplateContainer == null)
            {
                _contentTemplateContainer = new Control();
                if(_contentTemplate != null)
                    _contentTemplate.InstantiateIn(_contentTemplateContainer);
                Controls.Add(_contentTemplateContainer);
            }
            else if(_contentTemplate != null)
                _contentTemplate.InstantiateIn(_contentTemplateContainer);
        }

        #endregion

        #region Private

        private ITemplate _contentTemplate;
        private Control _contentTemplateContainer;
        private LayoutPanel _left;
        private LayoutPanel _right;

        private AdvStyle _barStyle;
        private AdvStyle _contentStyle;

        #endregion
    }

    /// <summary>
    /// The panel for parts of a layout
    /// </summary>
    public class LayoutPanel : ViewStateBase
    {
        /// <summary>
        /// Creates the panel
        /// </summary>
        /// <param name="owner">The owning Layout</param>
        /// <param name="position">The position this panel takes</param>
        internal LayoutPanel(Layout owner, string position)
        {
            _owner = owner;
            _position = position;
        }

        #region Properties

        /// <summary>
        /// Whether the panel is visible
        /// </summary>
        [Category("Appearance"), DefaultValue(true)]
        public bool Visible
        {
            get
            {
                if(ViewState["Visible"] != null)
                    return (bool)ViewState["Visible"];
                else
                    return true;
            }
            set 
            { 
                ViewState["Visible"] = value;
                if(_contentTemplateContainer != null)
                    _contentTemplateContainer.Visible = false;
            }
        }

        /// <summary>
        /// When open, can the panel be resized
        /// </summary>
        [Category("Behaviour"), DefaultValue(true)]
        public bool Resizable
        {
            get
            {
                if(ViewState["Resizable"] != null)
                    return (bool)ViewState["Resizable"];
                else
                    return true;
            }
            set { ViewState["Resizable"] = value; }
        }

        /// <summary>
        /// When closed, can a panel slide open over adjacent panels
        /// </summary>
        [Category("Behaviour"), DefaultValue(false)]
        public bool AutoHide
        {
            get
            {
                if(ViewState["AutoHide"] != null)
                    return (bool)ViewState["AutoHide"];
                else
                    return false;
            }
            set { ViewState["AutoHide"] = value; }
        }

        /// <summary>
        /// The minimum size of the panel
        /// </summary>
        [Category("Appearance"), DefaultValue(0)]
        public int MinimumSize
        {
            get
            {
                if(ViewState["minSize"] != null)
                    return (int)ViewState["minSize"];
                else
                    return 0;
            }
            set { ViewState["minSize"] = value; }
        }

        /// <summary>
        /// The maximum size of the panel
        /// </summary>
        [Category("Appearance"), DefaultValue(0)]
        public int MaximumSize
        {
            get
            {
                if(ViewState["maxSize"] != null)
                    return (int)ViewState["maxSize"];
                else
                    return 0;
            }
            set { ViewState["maxSize"] = value; }
        }

        /// <summary>
        /// The pin element selector, to pick the element to act as pin within the panel
        /// </summary>
        [Category("Behaviour"), DefaultValue(null)]
        public string PinElementSelector
        {
            get
            {
                if(ViewState["PinElement"] != null)
                    return (string)ViewState["PinElement"];
                else
                    return null;
            }
            set { ViewState["PinElement"] = value; }
        }

        /// <summary>
        /// The class to use to toggle the element on and off
        /// </summary>
        public string PinOnClass
        {
            get
            {
                if(ViewState["PinClass"] != null)
                    return (string)ViewState["PinClass"];
                else
                    return null;
            }
            set { ViewState["PinClass"] = value; }
        }

        /// <summary>
        /// The style for the resize bar
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(PaddingStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public AdvStyle BarStyle
        {
            get
            {
                if(_barStyle == null)
                {
                    _barStyle = new AdvStyle();
                    if(IsTrackingViewState)
                        ((IStateManager)_barStyle).TrackViewState();
                }

                return _barStyle;
            }
        }

        /// <summary>
        /// The style for the content
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(PaddingStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public AdvStyle ContentStyle
        {
            get
            {
                if(_contentStyle == null)
                {
                    _contentStyle = new AdvStyle();
                    if(IsTrackingViewState)
                        ((IStateManager)_contentStyle).TrackViewState();
                }

                return _contentStyle;
            }
        }

        /// <summary>
        /// The contents of the panel
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

        #region Template

        ///
        protected Control ContentTemplateContainer
        {
            get
            {
                if(_contentTemplateContainer == null)
                {
                    _contentTemplateContainer = new Control();
                    if(_owner != null)
                        _owner.Controls.Add(_contentTemplateContainer);
                    _contentTemplateContainer.Visible = Visible;
                }

                return _contentTemplateContainer;
            }
        }

        private void ClearContent()
        {
            ContentTemplateContainer.Controls.Clear();
            if(_owner != null)
                _owner.Controls.Remove(_contentTemplateContainer);
            _contentTemplateContainer = null;
        }

        private void CreateContents()
        {
            if(_contentTemplateContainer == null)
            {
                _contentTemplateContainer = new Control();
                if(_contentTemplate != null)
                    _contentTemplate.InstantiateIn(_contentTemplateContainer);
                if(_owner != null)
                    _owner.Controls.Add(_contentTemplateContainer);
                _contentTemplateContainer.Visible = Visible;
            }
            else if(_contentTemplate != null)
                _contentTemplate.InstantiateIn(_contentTemplateContainer);
        }

        #endregion

        #region Control Events

        internal void OnLoad(string postback)
        {
            string[] parts = postback.Split(",".ToCharArray());
            foreach(string part in parts)
            {
                string[] vals = part.Split(":".ToCharArray());
                if(vals.Length == 2)
                {
                    switch(vals[0])
                    {
                        case "width":
                            ContentStyle.Width = new Unit(int.Parse(vals[1]), UnitType.Pixel);
                            break;
                        case "autoHide":
                            AutoHide = bool.Parse(vals[1]);
                            break;
                    }
                }
            }
        }

        internal void OnPreRender(List<string> opts)
        {
            if(!Visible)
                return;

            if(!BarStyle.IsEmpty)
                _owner.Page.Header.StyleSheet.RegisterStyle(BarStyle, _owner);
            if(!ContentStyle.IsEmpty)
                _owner.Page.Header.StyleSheet.RegisterStyle(ContentStyle, _owner);

            List<string> panOpts = new List<string>();

            panOpts.Add(string.Format("panel:\"#{0}_{1}\"", _owner.ClientID, _position));

            if(!Resizable)
                panOpts.Add("resizeable:false");

            if(MinimumSize != 0)
                panOpts.Add(string.Format("minSize:{0}", MinimumSize));

            if(MaximumSize != 0)
                panOpts.Add(string.Format("maxSize:{0}", MaximumSize));

            if(!BarStyle.IsEmpty)
                panOpts.Add(string.Format("barClass:\"{0}\"", BarStyle.RenderClass));

            if(AutoHide)
                panOpts.Add("autoHide:true");

            if(!string.IsNullOrEmpty(PinElementSelector))
            {
                if(PinElementSelector.StartsWith("#") || !PinElementSelector.StartsWith("."))
                {
                    Control ctrl = Util.FindControlRecursive(_contentTemplateContainer, PinElementSelector);
                    if(ctrl != null)
                        panOpts.Add(string.Format("pinElem:\"#{0}\"", ctrl.ClientID));
                    else
                        panOpts.Add(string.Format("pinElem:\"{0}\"", PinElementSelector));
                }
                else
                    panOpts.Add(string.Format("pinElem:\"{0}\"", PinElementSelector));
            }
            if(!string.IsNullOrEmpty(PinOnClass))
                panOpts.Add(string.Format("pinElemClass:\"{0}\"", PinOnClass));

            opts.Add(string.Format("{0}:{{{1}}}", _position, string.Join(",", panOpts)));
        }

        internal void Render(HtmlTextWriter writer)
        {
            if(!Visible)
                return;

            writer.AddAttribute(HtmlTextWriterAttribute.Id,  _owner.ClientID + "_" + _position);
            if(!ContentStyle.IsEmpty)
                writer.AddAttribute(HtmlTextWriterAttribute.Class, ContentStyle.RenderClass);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            ContentTemplateContainer.RenderControl(writer);
            writer.RenderEndTag();
        }

        #endregion

        #region ViewState

        ///
        protected internal override void TrackViewState()
        {
            base.TrackViewState();
            if(_barStyle != null) ((IStateManager)_barStyle).TrackViewState();
            if(_contentStyle != null) ((IStateManager)_contentStyle).TrackViewState();
        }

        ///
        protected internal override object SaveViewState()
        {
            object[] state = new object[3];
            state[0] = base.SaveViewState();
            if(_barStyle != null) state[1] = ((IStateManager)_barStyle).SaveViewState();
            if(_contentStyle != null) state[2] = ((IStateManager)_contentStyle).SaveViewState();
            return state;
        }

        ///
        protected internal override void LoadViewState(object savedState)
        {
            object[] state = (object[])savedState;
            if(state != null)
            {
                if(state[0] != null) base.LoadViewState(state[0]);
                if(state[1] != null) ((IStateManager)BarStyle).LoadViewState(state[1]);
                if(state[2] != null) ((IStateManager)ContentStyle).LoadViewState(state[2]);
            }
        }

        #endregion

        #region Private

        private Layout _owner;
        private string _position;
        private ITemplate _contentTemplate;
        private Control _contentTemplateContainer;

        private AdvStyle _barStyle;
        private AdvStyle _contentStyle;

        #endregion
    }
}
