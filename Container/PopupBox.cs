using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// Popup Box control doesn't use ASP.NET AJAX but does support it, works with updatepanels inside
    /// but with no Heavy overhead if their not
    /// </summary>
    [ToolboxData("<{0}:popupbox runat=\"server\"></{0}:popupbox>"), ParseChildren(true), PersistChildren(false)]
    public class PopupBox : WebControl
    {
        #region Public

        #region Appearance Properties

        /// <summary>
        /// If the control is to be displayed modally
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(false)]
        public bool Modal
        {
            get
            {
                if(ViewState["Modal"] == null)
                    return false;
                else
                    return (bool)ViewState["Modal"];
            }
            set
            {
                if(!value)
                    ViewState.Remove("Modal");
                else
                    ViewState["Modal"] = value;
            }
        }

        /// <summary>
        /// Whether to use its own or the jquery overlay
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(false)]
        public bool UseJQueryOverlay
        {
            get
            {
                if(ViewState["UseJQueryOverlay"] != null)
                    return(bool)ViewState["UseJQueryOverlay"];
                else
                    return false;
            }
            set
            {
                if(value != false)
                    ViewState["UseJQueryOverlay"] = value;
                else
                    ViewState.Remove("UseJQueryOverlay");
            }
        }
        
        /// <summary>
        /// The position of the popup on the screen
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(typeof(Positioning), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public Positioning Position
        {
            get
            {
                if(_pos == null)
                {
                    _pos = new Positioning();
                    _pos.Collision = Collision.Fit;
                    if(IsTrackingViewState) ((IStateManager)_pos).TrackViewState();
                }
                return _pos;
            }
        }

        /// <summary>
        /// Whether the popup is currently shown
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(false)]
        public bool Shown
        {
            get
            {
                if(ViewState["Shown"] == null)
                    return false;
                else
                    return (bool)ViewState["Shown"];
            }
            set
            {
                if(!value)
                    ViewState.Remove("Shown");
                else
                {
                    Visible = true;
                    ViewState["Shown"] = true;
                }
            }
        }

        /// <summary>
        /// The z index of the pop up
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(1000)]
        public int ZIndex
        {
            get
            {
                if(string.IsNullOrEmpty(Style["z-index"]))
                    return 1000;
                else
                    return int.Parse(Style["z-index"]);
            }
            set
            {
                if(value == 1000)
                    Style.Remove("z-index");
                else
                    Style["z-index"] = value.ToString();
            }
        }

        /// <summary>
        /// The effect to run on show
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(null),MergableProperty(false),PersistenceMode(PersistenceMode.InnerProperty)]
        public Effect ShowEffect
        {
            get
            {
                if(ViewState["ShowEffect"] != null)
                    return(Effect)ViewState["ShowEffect"];
                else
                    return null;
            }
            set
            {
                if(value != null)
                    ViewState["ShowEffect"] = value;
                else
                    ViewState.Remove("ShowEffect");
            }
        }

        /// <summary>
        ///  The effect to show in hiding
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public Effect HideEffect
        {
            get
            {
                if(ViewState["HideEffect"] != null)
                    return(Effect)ViewState["HideEffect"];
                else
                    return null;
            }
            set
            {
                if(value != null)
                    ViewState["HideEffect"] = value;
                else
                    ViewState.Remove("HideEffect");
            }
        }



        #endregion

        #region Behaviour Properties

        /// <summary>
        /// Events that can trigger a popup to open
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(TriggerList), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public TriggerList OpenTriggers
        {
            get
            {
                if(_openTriggers == null)
                {
                    _openTriggers = new TriggerList();
                    if(IsTrackingViewState) ((IStateManager)_openTriggers).TrackViewState();
                }
                return _openTriggers;
            }
        }

        /// <summary>
        /// Events that can trigger a popup to close
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(TriggerList), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public TriggerList CloseTriggers
        {
            get
            {
                if(_closeTriggers == null)
                {
                    _closeTriggers = new TriggerList();
                    if(IsTrackingViewState) ((IStateManager)_closeTriggers).TrackViewState();
                }
                return _closeTriggers;
            }
        }

        /// <summary>
        /// The client side events
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(PopupClientEvents), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public PopupClientEvents ClientSideEvents
        {
            get
            {
                if(_clientEvents == null)
                {
                    _clientEvents = new PopupClientEvents();
                    if(IsTrackingViewState) ((IStateManager)_clientEvents).TrackViewState();
                }
                return _clientEvents;
            }
        }

        /// <summary>
        /// The default button within the Popup Box
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string DefaultButton
        {
            get
            {
                if(ViewState["DefaultButton"] == null)
                    return null;
                else
                    return (string)ViewState["DefaultButton"];
            }
            set { ViewState["DefaultButton"] = value; }
        }

        /// <summary>
        /// The element used to allow the user to move the box
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string MovementElement
        {
            get
            {
                if(ViewState["MoveElement"] == null)
                    return null;
                else
                    return (string)ViewState["MoveElement"];
            }
            set { ViewState["MoveElement"] = value; }
        }

        /// <summary>
        /// Whether to keep a screen aligned popup in the same place when scrolling the screen
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(true)]
        public bool Scrolling
        {
            get
            {
                if(ViewState["Scrolling"] == null)
                    return true;
                else
                    return (bool)ViewState["Scrolling"];
            }
            set { ViewState["Scrolling"] = value; }
        }

        /// <summary>
        /// Links visibility to shown, so won't render if its not shown
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(false)]
        public bool VisibleAsShown
        {
            get
            {
                if(ViewState["VisibleAsShown"] == null)
                    return false;
                else
                    return (bool)ViewState["VisibleAsShown"];
            }
            set { ViewState["VisibleAsShown"] = value; }
        }

        #endregion

        #region Data Properties

        /// <summary>
        /// The contents of the box
        /// </summary>
        [Category("Data"), Browsable(false), PersistenceMode(PersistenceMode.InnerProperty), TemplateInstance(TemplateInstance.Single)]
        public ITemplate Content
        {
            get { return _contentTemplate; }
            set
            {
                if(!DesignMode & _contentTemplate != null)
                    throw new InvalidOperationException("Could not set contents template on control " + ID);
                _contentTemplate = value;
                if(_contentTemplate != null)
                    CreateContents();
            }
        }

        #endregion

        #endregion

        #region protected

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
                    ChildControls.AddInternal(_contentTemplateContainer);
                }

                return _contentTemplateContainer;
            }
        }

        /// <summary>
        /// Creates a single collection for the controls within the template
        /// </summary>
        protected override sealed System.Web.UI.ControlCollection CreateControlCollection()
        {
            return new LimitCollection(this, 1);
        }

        /// <summary>
        /// Causes the control to track changes to its view state so they can be stored in the object's <see cref="P:System.Web.UI.Control.ViewState"/> property.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();
            if(_openTriggers != null) _openTriggers.TrackViewState();
            if(_closeTriggers != null) _closeTriggers.TrackViewState();
            if(_clientEvents != null) _clientEvents.TrackViewState();
            if(_pos != null) _pos.TrackViewState();
        }

        /// <summary>
        /// Restores view-state information from a previous request that was saved with the <see cref="M:System.Web.UI.WebControls.WebControl.SaveViewState"/> method.
        /// </summary>
        /// <param name="savedState">An object that represents the control state to restore.</param>
        protected override void LoadViewState(object savedState)
        {
            object[] states = (object[])savedState;
            if(states != null)
            {
                if(states[0] != null) base.LoadViewState(states[0]);
                if(states[1] != null) OpenTriggers.LoadViewState(states[1]);
                if(states[2] != null) CloseTriggers.LoadViewState(states[2]);
                if(states[3] != null) ClientSideEvents.LoadViewState(states[3]);
                if(states[4] != null) Position.LoadViewState(states[4]);
            }
        }

        /// <summary>
        /// Saves any state that was modified after the <see cref="M:System.Web.UI.WebControls.Style.TrackViewState"/> method was invoked.
        /// </summary>
        /// <returns>
        /// An object that contains the current view state of the control; otherwise, if there is no view state associated with the control, null.
        /// </returns>
        protected override object SaveViewState()
        {
            object[] states = new object[5];
            states[0] = base.SaveViewState();
            if(_openTriggers != null) states[1] = _openTriggers.SaveViewState();
            if(_closeTriggers != null) states[2] = _closeTriggers.SaveViewState();
            if(_clientEvents != null) states[3] = _clientEvents.SaveViewState();
            if(_pos != null) states[4] = _pos.SaveViewState();
            return states;
        }

        #endregion

        #region Control Events

        ///
        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            if(_contentTemplateContainer == null)
            {
                _contentTemplateContainer = new Control();
                ChildControls.AddInternal(_contentTemplateContainer);
            }
        }

        ///
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if(!string.IsNullOrEmpty(Page.Request.Params[ClientID + "_shown"]))
            {
                bool t = Shown;
                bool.TryParse(Page.Request.Params[ClientID + "_shown"], out t);
                Shown = t;
            }

            foreach(ServerSideTrigger ctrl in OpenTriggers)
            {
                if(!(ctrl is ClientSideTrigger))
                {
                    Control webCtrl = Util.FindControlRecursiveOut(this, ctrl.ControlID, null);
                    System.Reflection.EventInfo ei = webCtrl.GetType().GetEvent(ctrl.Event);
                    if(ei != null)
                    {
                        System.Reflection.MethodInfo mi = this.GetType().GetMethod("OpenPopup");
                        Delegate del = Delegate.CreateDelegate(ei.EventHandlerType, this, mi);
                        System.Reflection.MethodInfo eiAdd = ei.GetAddMethod();
                        object[] eiAddArgs = { del };
                        eiAdd.Invoke(webCtrl, eiAddArgs);
                    }
                }
            }

            foreach(ServerSideTrigger ctrl in CloseTriggers)
            {
                if(!(ctrl is ClientSideTrigger))
                {
                    Control webCtrl = Util.FindControlRecursiveOut(this, ctrl.ControlID, null);
                    System.Reflection.EventInfo ei = webCtrl.GetType().GetEvent(ctrl.Event);
                    if(ei != null)
                    {
                        System.Reflection.MethodInfo mi = this.GetType().GetMethod("ClosePopup");
                        Delegate del = Delegate.CreateDelegate(ei.EventHandlerType, this, mi);
                        System.Reflection.MethodInfo eiAdd = ei.GetAddMethod();
                        object[] eiAddArgs = { del };
                        eiAdd.Invoke(webCtrl, eiAddArgs);
                    }
                }

            }
        }

        ///
        protected override void OnPreRender(System.EventArgs e)
        {
            if(VisibleAsShown)
            {
                if(Shown)
                    Visible = true;
                else
                {
                    Visible = false;
                    return;
                }
            }

            base.OnPreRender(e);

            Script.AddResourceScript(Page, "jquery.popup.js", "jquery.ui.position.js");
            List<string> opts = new List<string>();

            if(Modal)
                opts.Add("modal:true");

            opts.Add("position:" + Position.JSOption(this));

            if(UseJQueryOverlay)
                opts.Add("usejqueryoverlay:true");
            if(ZIndex != 1000)
                opts.Add("zIndex:" + ZIndex.ToString());

            if(!string.IsNullOrEmpty(MovementElement))
            {
                Control ctrl = Util.FindControlRecursive(this, MovementElement);
                if(ctrl != null)
                    opts.Add("moveelement:\"" + ctrl.ClientID + "\"");
                else
                    opts.Add("moveelement:\"" + MovementElement + "\"");
            }

            if(!Scrolling)
                opts.Add("scrolling:false");

            if(_openTriggers != null && _openTriggers.Count > 0)
                opts.Add(TriggerPreRender(_openTriggers, "opentriggers"));

            if(_closeTriggers != null && _closeTriggers.Count > 0)
                opts.Add(TriggerPreRender(_closeTriggers, "closetriggers"));

            if((_clientEvents != null))
                _clientEvents.PreRender(opts);

            if(ShowEffect != null)
                opts.Add("showeffect:" + ShowEffect.Render(Page));

            if(HideEffect != null)
                opts.Add("hideeffect:" + HideEffect.Render(Page));

            if(TriggerInUpdatePanel(_openTriggers) || TriggerInUpdatePanel(_closeTriggers))
                Script.AddStartupScriptSM(this, "ls_popup", opts);
            else
                Script.AddStartupScript(this, "ls_popup", opts);
            ScriptManager.RegisterHiddenField(this, ClientID + "_shown", Shown.ToString().ToLower());
        }

        ///
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            if(!string.IsNullOrEmpty(DefaultButton))
            {
                Control button = Util.FindControlRecursive(this, DefaultButton);
                if(button is IButtonControl)
                    writer.AddAttribute("onkeypress", "return WebForm_FireDefaultButton(event, '" + button.ClientID + "')");
            }

            if(string.IsNullOrEmpty(Style["z-index"])) writer.AddStyleAttribute(HtmlTextWriterStyle.ZIndex, "1000");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            RenderChildren(writer);
            writer.RenderEndTag();
        }

        #endregion

        #region Close/Open Events

        /// <summary>
        /// Event Handler for open Popup
        /// </summary>
        public void OpenPopup(object sender, EventArgs e)
        {
            Shown = true;
        }

        /// <summary>
        /// Event Handler for close Popup
        /// </summary>
        public void ClosePopup(object sender, EventArgs e)
        {
            Shown = false;
        }

        #endregion

        #region Private

        private void ClearContent()
        {
            ContentTemplateContainer.Controls.Clear();
            _contentTemplateContainer = null;
            ChildControls.ClearInternal();
        }

        private void CreateContents()
        {
            if(DesignMode) ClearContent();

            if(_contentTemplateContainer == null)
            {
                _contentTemplateContainer = new Control();
                if(_contentTemplate != null)
                    _contentTemplate.InstantiateIn(_contentTemplateContainer);

                ChildControls.AddInternal(_contentTemplateContainer);
            }
            else if(_contentTemplate != null)
                _contentTemplate.InstantiateIn(_contentTemplateContainer);
        }

        private string TriggerPreRender(TriggerList triggers, string name)
        {
            List<string> trigs = new List<string>();
            foreach(ServerSideTrigger trig in triggers)
            {
                ClientSideTrigger cst = trig as ClientSideTrigger;
                if(cst != null)
                {
                    Control ctrl = Util.FindControlRecursiveOut(this, cst.ControlID, null);
                    if(ctrl != null)
                        trigs.Add(string.Format("{{control:\"{0}\",event:\"{1}\",cancel:{2}}}", ctrl.ClientID, cst.Event, cst.CancelEvent.ToString().ToLower()));
                    else
                        trigs.Add(string.Format("{{control:\"{0}\",event:\"{1}\",cancel:{2}}}", cst.ControlID, cst.Event, cst.CancelEvent.ToString().ToLower()));
                }
            }
            return string.Format("{0}:[{1}]", name, string.Join(",", trigs));
        }

        private bool TriggerInUpdatePanel(TriggerList triggers)
        {
            if(triggers != null)
            {
                List<string> trigs = new List<string>();
                foreach(ServerSideTrigger trig in triggers)
                {
                    ClientSideTrigger cst = trig as ClientSideTrigger;
                    if(cst != null)
                    {
                        Control ctrl = Util.FindControlRecursiveOut(this, cst.ControlID, null);
                        if(ctrl != null)
                        {
                            if(Util.InUpdatePanel(ctrl))
                                return true;
                        }
                    }
                }
            }

            return false;
        }

        private LimitCollection ChildControls
        {
            get
            {
                if(Controls is LimitCollection)
                    return (LimitCollection)Controls;
                else
                    throw new InvalidOperationException("The controls property has the wrong class");
            }
        }

        private ITemplate _contentTemplate;
        private Control _contentTemplateContainer;
        private TriggerList _openTriggers;
        private TriggerList _closeTriggers;
        private PopupClientEvents _clientEvents;
        private Positioning _pos;

        #endregion
    }

    /// <summary>
    /// The client events for the tree
    /// </summary>
    [ParseChildren(true)]
    public class PopupClientEvents :ViewStateBase
    {
        /// <summary>
        /// Javascript to call before opening popup
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string BeforeOpen
        {
            get
            {
                if(ViewState["BeforeOpen"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["BeforeOpen"];
            }
            set { ViewState["BeforeOpen"] = value; }
        }

        /// <summary>
        /// Javascript to call after opening popup
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string AfterOpen
        {
            get
            {
                if(ViewState["AfterOpen"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["AfterOpen"];
            }
            set { ViewState["AfterOpen"] = value; }
        }

        /// <summary>
        /// Javascript to call before closing popup
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string BeforeClose
        {
            get
            {
                if(ViewState["BeforeClose"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["BeforeClose"];
            }
            set { ViewState["BeforeClose"] = value; }
        }

        /// <summary>
        /// Javascript to call after closing popup
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string AfterClose
        {
            get
            {
                if(ViewState["AfterClose"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["AfterClose"];
            }
            set { ViewState["AfterClose"] = value; }
        }

        #region Internal

        internal void PreRender(List<string> opts)
        {
            if(!string.IsNullOrEmpty(BeforeOpen))
                opts.Add(string.Format("beforeopen:function(){{{0}}}", BeforeOpen));
            if(!string.IsNullOrEmpty(AfterOpen))
                opts.Add(string.Format("afteropen:function(){{{0}}}", AfterOpen));
            if(!string.IsNullOrEmpty(BeforeClose))
                opts.Add(string.Format("beforeclose:function(){{{0}}}", BeforeClose));
            if(!string.IsNullOrEmpty(AfterClose))
                opts.Add(string.Format("afterclose:function(){{{0}}}", AfterClose));
        }

        #endregion

    }

}