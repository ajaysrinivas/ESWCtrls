using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// jQuery based dialog box, provides buttons, and useful functions
    /// </summary>
    [ToolboxData("<{0}:dialog runat=\"server\"></{0}:dialog>"), ParseChildren(true), PersistChildren(false)]
    public class Dialog : WebControl, IPostBackEventHandler
    {
        #region Properties

        #region Behaviour

        /// <summary>
        /// Whether to start open
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(true)]
        public bool AutoOpen
        {
            get
            {
                if(ViewState["autoOpen"] != null)
                    return (bool)ViewState["autoOpen"];
                else
                    return true;
            }
            set { ViewState["autoOpen"] = value; }
        }

        /// <summary>
        /// Whether escape closes the dialog
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(true)]
        public bool CloseOnEscape
        {
            get
            {
                if(ViewState["closeOnEscape"] != null)
                    return (bool)ViewState["closeOnEscape"];
                else
                    return true;
            }
            set { ViewState["closeOnEscape"] = value; }
        }

        /// <summary>
        /// Whether the dialog can be dragged
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(true)]
        public bool Draggable
        {
            get
            {
                if(ViewState["Draggable"] != null)
                    return (bool)ViewState["Draggable"];
                else
                    return true;
            }
            set
            { ViewState["Draggable"] = value; }
        }

        /// <summary>
        /// If the dialog is resizeable
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(true)]
        public bool Resizeable
        {
            get
            {
                if(ViewState["Resizeable"] != null)
                    return (bool)ViewState["Resizeable"];
                else
                    return true;
            }
            set { ViewState["Resizeable"] = value; }
        }
        
        /// <summary>
        /// Whether to display the Dialog modally
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(false)]
        public bool Modal
        {
            get
            {
                if(ViewState["Modal"] != null)
                    return (bool)ViewState["Modal"];
                else
                    return false;
            }
            set { ViewState["Modal"] = value; }
        }
        
        /// <summary>
        /// Whether the dialog is stackable
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(true)]
        public bool Stackable
        {
            get
            {
                if(ViewState["Stackable"] != null)
                    return (bool)ViewState["Stackable"];
                else
                    return true;
            }
            set { ViewState["Stackable"] = value; }
        }

        /// <summary>
        /// Buttons on the dialog box
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(DialogButtonList), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public DialogButtonList Buttons
        {
            get
            {
                if (_buttons == null)
                {
                    _buttons = new DialogButtonList();
                    if (IsTrackingViewState) ((IStateManager)_buttons).TrackViewState();
                }
                return _buttons;
            }
        }

        /// <summary>
        /// Events that can trigger a popup to open
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(TriggerList), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public TriggerList OpenTriggers
        {
            get
            {
                if (_openTriggers == null)
                {
                    _openTriggers = new TriggerList();
                    if (IsTrackingViewState) ((IStateManager)_openTriggers).TrackViewState();
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
                if (_closeTriggers == null)
                {
                    _closeTriggers = new TriggerList();
                    if (IsTrackingViewState) ((IStateManager)_closeTriggers).TrackViewState();
                }
                return _closeTriggers;
            }
        }

        /// <summary>
        /// The client side events
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(DialogClientEvents), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public DialogClientEvents ClientSideEvents
        {
            get
            {
                if (_clientEvents == null)
                {
                    _clientEvents = new DialogClientEvents();
                    if (IsTrackingViewState) ((IStateManager)_clientEvents).TrackViewState();
                }
                return _clientEvents;
            }
        }

        #endregion

        #region Appearance

        /// <summary>
        /// The title to use for the dialog
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(null)]
        public string Title
        {
            get
            {
                if(ViewState["Title"] != null)
                    return (string)ViewState["Title"];
                else
                    return null;
            }
            set { ViewState["Title"] = value; }
        }

        /// <summary>
        /// When not using a theme, the text to appear for close
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("close")]
        public string CloseText
        {
            get
            {
                if(ViewState["CloseText"] != null)
                    return (string)ViewState["CloseText"];
                else
                    return "close";
            }
            set { ViewState["CloseText"] = value; }
        }

        /// <summary>
        /// The class that will be added to the dialog for theming
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(null)]
        public string DialogClass
        {
            get
            {
                if(ViewState["DialogClass"] != null)
                    return (string)ViewState["DialogClass"];
                else
                    return null;
            }
            set { ViewState["DialogClass"] = value; }
        }

        /// <summary>
        /// Positioning the dialog relative to the viewport
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(Position.CenterCenter)]
        public Position Position
        {
            get
            {
                if(ViewState["Position"] != null)
                    return (Position)ViewState["Position"];
                else
                    return Position.CenterCenter;
            }
            set { ViewState["Position"] = value; }
        }

        /// <summary>
        /// Positions the dialog at a pixel position
        /// </summary>
        /// <remarks>Takes over Position if its not empty</remarks>
        [Bindable(true), Category("Appearance")]
        public System.Drawing.Point PositionPixel
        {
            get
            {
                if(ViewState["PositionPixel"] != null)
                    return (System.Drawing.Point)ViewState["PositionPixel"];
                else
                    return System.Drawing.Point.Empty;
            }
            set { ViewState["PositionPixel"] = value; }
        }

        /// <summary>
        /// The height of the dialog in pixels (default 0, auto)
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(0)]
        public new int Height
        {
            get
            {
                if(ViewState["height"] != null)
                    return (int)ViewState["height"];
                else
                    return 0;
            }
            set { ViewState["height"] = value; }
        }

        /// <summary>
        /// The maximum height allowed (default 0, none)
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(0)]
        public int MaxHeight
        {
            get
            {
                if(ViewState["MaxHeight"] != null)
                    return (int)ViewState["MaxHeight"];
                else
                    return 0;
            }
            set { ViewState["MaxHeight"] = value; }
        }

        /// <summary>
        /// The minimum height allowed (default 0, none)
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(150)]
        public int MinHeight
        {
            get
            {
                if(ViewState["MinHeight"] != null)
                    return (int)ViewState["MinHeight"];
                else
                    return 150;
            }
            set { ViewState["MinHeight"] = value; }
        }

        /// <summary>
        /// The width of the dialog box (default 300)
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(300)]
        public new int Width
        {
            get
            {
                if(ViewState["Width"] != null)
                    return (int)ViewState["Width"];
                else
                    return 300;
            }
            set { ViewState["Width"] = value; }
        }

        /// <summary>
        /// The maximum width allowed (default 0, none)
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(0)]
        public int MaxWidth
        {
            get
            {
                if(ViewState["MaxWidth"] != null)
                    return (int)ViewState["MaxWidth"];
                else
                    return 0;
            }
            set { ViewState["MaxWidth"] = value; }
        }

        /// <summary>
        /// The minimum height allowed (default 0, none)
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(150)]
        public int MinWidth
        {
            get
            {
                if(ViewState["MinWidth"] != null)
                    return (int)ViewState["MinWidth"];
                else
                    return 150;
            }
            set { ViewState["MinWidth"] = value; }
        }

        /// <summary>
        /// The zIndex to use for the dialog
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(1000)]
        public int ZIndex
        {
            get
            {
                if(ViewState["ZIndex"] != null)
                    return (int)ViewState["ZIndex"];
                else
                    return 1000;
            }
            set { ViewState["ZIndex"] = value; }
        }

        /// <summary>
        /// The animation effect to use on hiding
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(Effect.EffectType.None)]
        public Effect.EffectType HideEffect
        {
            get
            {
                if(ViewState["HideEffect"] != null)
                    return (Effect.EffectType)ViewState["HideEffect"];
                else
                    return Effect.EffectType.None;
            }
            set { ViewState["HideEffect"] = value; }
        }

        /// <summary>
        /// The animation effect to use when showing
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(Effect.EffectType.None)]
        public Effect.EffectType ShowEffect
        {
            get
            {
                if(ViewState["ShowEffect"] != null)
                    return (Effect.EffectType)ViewState["ShowEffect"];
                else
                    return Effect.EffectType.None;
            }
            set { ViewState["ShowEffect"] = value; }
        }

        #endregion

        #region Data Properties

        /// <summary>
        /// The contents of the box
        /// </summary>
        [Browsable(false), Category("Data"), PersistenceMode(PersistenceMode.InnerProperty), TemplateInstance(TemplateInstance.Single)]
        public ITemplate Content
        {
            get { return _contentTemplate; }
            set
            {
                if (!DesignMode & _contentTemplate != null)
                    throw new InvalidOperationException("Could not set contents template on control " + ID);
                _contentTemplate = value;
                if (_contentTemplate != null)
                    CreateContents();
            }
        }

        #endregion

        #endregion

        #region Close/Open Events

        /// <summary>
        /// Event Handler for open Popup
        /// </summary>
        public void OpenDialog(object sender, EventArgs e)
        {
            AutoOpen = true;
        }

        /// <summary>
        /// Event Handler for close Popup
        /// </summary>
        public void CloseDialog(object sender, EventArgs e)
        {
            AutoOpen = false;
        }

        #endregion

        #region Control Events

        ///
        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            if (_contentTemplateContainer == null)
            {
                _contentTemplateContainer = new Control();
                ChildControls.AddInternal(_contentTemplateContainer);
            }
        }

        ///
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!string.IsNullOrEmpty(Page.Request.Params[ClientID + "_shown"]))
            {
                bool t = AutoOpen;
                bool.TryParse(Page.Request.Params[ClientID + "_shown"], out t);
                AutoOpen = t;
            }

            ProcessServerSideTriggers();
        }

        /// 
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Script.AddResourceScript(Page, "jquery.ui.dialog.js");

            List<string> opts = new List<string>();
            if (!Enabled)
                opts.Add("disabled:true");
            if (!AutoOpen)
                opts.Add("autoOpen:false");
            if (!CloseOnEscape)
                opts.Add("closeOnEscape:false");
            if (!Draggable)
                opts.Add("draggable:false");
            if (!Resizeable)
                opts.Add("resizable:false");
            if (Modal)
                opts.Add("modal:true");
            if (!Stackable)
                opts.Add("stackable:false");

            if (!string.IsNullOrEmpty(Title))
                opts.Add(string.Format("title:\"{0}\"", Title));
            if(CloseText != "close")
                opts.Add(string.Format("closeText:\"{0}\"", CloseText));
            if(!string.IsNullOrEmpty(DialogClass))
                opts.Add(string.Format("dialogClass:\"{0}\"", DialogClass));
            if (PositionPixel.IsEmpty)
            {
                if (Position != Position.CenterCenter)
                    opts.Add(string.Format("postion:{0}", PositionStrings[(int)Position]));
            }
            else
                opts.Add(string.Format("position:[{0},{1}]", PositionPixel.X, PositionPixel.Y));
            if (Height > 0)
                opts.Add(string.Format("height:{0}", Height));
            if(MinHeight != 150)
                opts.Add(string.Format("minHeight:{0}", MinHeight));
            if (MaxHeight > 0)
                opts.Add(string.Format("maxHeight:{0}", MaxHeight));
            if (Width != 300)
                opts.Add(string.Format("width:{0}", Width));
            if(MinWidth != 150)
                opts.Add(string.Format("minWidth:{0}", MinWidth));
            if(MaxWidth > 0)
                opts.Add(string.Format("maxWidth:{0}", MaxWidth));
            if(ZIndex != 1000)
                opts.Add(string.Format("zIndex:{0}", ZIndex));
            if(ShowEffect != Effect.EffectType.None)
            {
                Effect.SetScripts(Page, ShowEffect);
                if(!AutoOpen)
                    opts.Add(string.Format("show:\"{0}\"", Effect.TypeString(ShowEffect)));
            }
            if(HideEffect != Effect.EffectType.None)
            {
                Effect.SetScripts(Page, HideEffect);
                opts.Add(string.Format("hide:\"{0}\"", Effect.TypeString(HideEffect)));
            }


            ProcessButtons(opts);
            ProcessClientSideEvents(opts);

            Script.AddStartupScript(this, ClientID, "dialog", opts);
            ScriptManager.RegisterHiddenField(this, ClientID + "_shown", AutoOpen.ToString().ToLower());

            ProcessClientSideTriggers();
        }
        
        /// 
        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            RenderChildren(writer);
            writer.RenderEndTag();
        }

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
                if (_contentTemplateContainer == null)
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

        ///
        protected override void TrackViewState()
        {
            base.TrackViewState();
            if (_openTriggers != null) ((IStateManager)_openTriggers).TrackViewState();
            if (_closeTriggers != null) ((IStateManager)_closeTriggers).TrackViewState();
            if (_clientEvents != null) ((IStateManager)_clientEvents).TrackViewState();
        }

        ///
        protected override void LoadViewState(object savedState)
        {
            object[] states = (object[])savedState;
            if (states != null)
            {
                if (states[0] != null) base.LoadViewState(states[0]);
                if(states[1] != null) ((IStateManager)Buttons).LoadViewState(states[1]);
                if (states[2] != null) ((IStateManager)OpenTriggers).LoadViewState(states[2]);
                if (states[3] != null) ((IStateManager)CloseTriggers).LoadViewState(states[3]);
                if (states[4] != null) ((IStateManager)ClientSideEvents).LoadViewState(states[4]);
            }
        }

        ///
        protected override object SaveViewState()
        {
            object[] states = new object[5];
            states[0] = base.SaveViewState();
            if(_buttons != null) states[1] = ((IStateManager)_buttons).SaveViewState();
            if (_openTriggers != null) states[2] = ((IStateManager)_openTriggers).SaveViewState();
            if (_closeTriggers != null) states[3] = ((IStateManager)_closeTriggers).SaveViewState();
            if (_clientEvents != null) states[4] = ((IStateManager)_clientEvents).SaveViewState();
            return states;
        }

        #endregion

        #region Private

        private void ProcessButtons(List<string> options)
        {
            if (_buttons != null && _buttons.Count > 0)
            {
                List<string> buttons = new List<string>();
                foreach (DialogButton b in _buttons)
                {
                    if(string.IsNullOrEmpty(b.ClientSide))
                    {
                        if(string.IsNullOrEmpty(b.PostbackArgument))
                            buttons.Add(string.Format("\"{0}\":function(){{{1}}}", b.Text, Page.ClientScript.GetPostBackEventReference(this, b.Text)));
                        else
                            buttons.Add(string.Format("\"{0}\":function(){{{1}}}", b.Text, Page.ClientScript.GetPostBackEventReference(this, b.PostbackArgument)));
                    }
                    else
                    {
                        if(string.IsNullOrEmpty(b.PostbackArgument))
                            buttons.Add(string.Format("\"{0}\":function(){{{1};}}", b.Text, b.ClientSide));
                        else
                            buttons.Add(string.Format("\"{0}\":function(){{{1};{2}}}", b.Text, b.ClientSide, Page.ClientScript.GetPostBackEventReference(this, b.Text)));
                    }
                }
                options.Add("buttons:{" + string.Join(",", buttons) + "}");
            }
        }

        private void ProcessClientSideEvents(List<string> options)
        {
            string open = string.Format("$(\"#{0}{1}\").val(true);$(this).parent().appendTo(\"form:first\");", ClientID, "_shown");
            string close = string.Format("$(\"#{0}{1}\").val(false);$(this).parent().appendTo(\"form:first\");", ClientID, "_shown");
            if (AutoOpen && ShowEffect != Effect.EffectType.None)
                close += string.Format("$(\"#{0}\").dialog(\"option\", \"show\", \"{1}\");", ClientID, ShowEffect.ToString().ToLower());

            if (_clientEvents != null)
            {
                if(!string.IsNullOrEmpty(_clientEvents.Create))
                    options.Add(string.Format("create:function(event,ui){{{0};}}", _clientEvents.Create));

                if (!string.IsNullOrEmpty(_clientEvents.Open))
                    options.Add(string.Format("open:function(event,ui){{{0}{1};}}", open, _clientEvents.Open));
                else
                    options.Add(string.Format("open:function(event,ui){{{0}}}", open));

                if (!string.IsNullOrEmpty(_clientEvents.Close))
                    options.Add(string.Format("close:function(event,ui){{{0}{1};}}", close, _clientEvents.Close));
                else
                    options.Add(string.Format("close:function(event,ui){{{0}}}", close));

                if (!string.IsNullOrEmpty(_clientEvents.BeforeClose))
                    options.Add(string.Format("beforeClose:function(event,ui){{{0};}}", _clientEvents.BeforeClose));

                if(!string.IsNullOrEmpty(_clientEvents.Focus))
                    options.Add(string.Format("focus:function(event,ui){{{0};}}", _clientEvents.Focus));

                if(!string.IsNullOrEmpty(_clientEvents.DragStart))
                    options.Add(string.Format("dragStart:function(event,ui){{{0};}}", _clientEvents.DragStart));

                if(!string.IsNullOrEmpty(_clientEvents.Drag))
                    options.Add(string.Format("drag:function(event,ui){{{0};}}", _clientEvents.Drag));

                if(!string.IsNullOrEmpty(_clientEvents.DragStop))
                    options.Add(string.Format("dragStop:function(event,ui){{{0};}}", _clientEvents.DragStop));

                if(!string.IsNullOrEmpty(_clientEvents.ResizeStart))
                    options.Add(string.Format("resizeStart:function(event,ui){{{0};}}", _clientEvents.ResizeStart));

                if(!string.IsNullOrEmpty(_clientEvents.Resize))
                    options.Add(string.Format("resize:function(event,ui){{{0};}}", _clientEvents.Resize));

                if(!string.IsNullOrEmpty(_clientEvents.ResizeStop))
                    options.Add(string.Format("resizeStop:function(event,ui){{{0};}}", _clientEvents.ResizeStop));
            }
            else
            {
                options.Add(string.Format("open:function(event,ui){{{0}}}", open));
                options.Add(string.Format("close:function(event,ui){{{0}}}", close));
            }
        }

        private void ProcessClientSideTriggers()
        {
            if (_openTriggers != null)
            {
                foreach (ServerSideTrigger t in _openTriggers)
                {
                    if (t is ClientSideTrigger)
                    {
                        ClientSideTrigger cst = (ClientSideTrigger)t;
                        Control webCtrl = Util.FindControlRecursiveOut(this, cst.ControlID, null);
                        if (webCtrl != null)
                            Script.AddStartupScript(this, ClientID + "_open_" + webCtrl.ClientID, string.Format("$(\"#{0}\").bind(\"{1}\",function(){{$(\"#{2}\").dialog(\"open\");return {3};}});", webCtrl.ClientID, cst.Event, this.ClientID, (!cst.CancelEvent).ToString().ToLower()));
                    }
                }
            }

            if (_closeTriggers != null)
            {
                foreach (ServerSideTrigger t in _closeTriggers)
                {
                    if (t is ClientSideTrigger)
                    {
                        ClientSideTrigger cst = (ClientSideTrigger)t;
                        Control webCtrl = Util.FindControlRecursiveOut(this, cst.ControlID, null);
                        if (webCtrl != null)
                            Script.AddStartupScript(this, ClientID + "_close_" + webCtrl.ClientID, string.Format("$(\"#{0}\").bind(\"{1}\",function(){{$(\"#{2}\").dialog(\"close\");return {3};}});", webCtrl.ClientID, cst.Event, this.ClientID, (!cst.CancelEvent).ToString().ToLower()));
                    }
                }
            }
        }

        private void ProcessServerSideTriggers()
        {
            if (_openTriggers != null)
            {
                foreach (ServerSideTrigger ctrl in _openTriggers)
                {
                    if (!(ctrl is ClientSideTrigger))
                    {
                        Control webCtrl = Util.FindControlRecursiveOut(this, ctrl.ControlID, null);
                        System.Reflection.EventInfo ei = webCtrl.GetType().GetEvent(ctrl.Event);
                        if (ei != null)
                        {
                            System.Reflection.MethodInfo mi = this.GetType().GetMethod("OpenDialog");
                            Delegate del = Delegate.CreateDelegate(ei.EventHandlerType, this, mi);
                            System.Reflection.MethodInfo eiAdd = ei.GetAddMethod();
                            object[] eiAddArgs = { del };
                            eiAdd.Invoke(webCtrl, eiAddArgs);
                        }
                    }
                }
            }

            if (_closeTriggers != null)
            {
                foreach (ServerSideTrigger ctrl in CloseTriggers)
                {
                    if (!(ctrl is ClientSideTrigger))
                    {
                        Control webCtrl = Util.FindControlRecursiveOut(this, ctrl.ControlID, null);
                        System.Reflection.EventInfo ei = webCtrl.GetType().GetEvent(ctrl.Event);
                        if (ei != null)
                        {
                            System.Reflection.MethodInfo mi = this.GetType().GetMethod("CloseDialog");
                            Delegate del = Delegate.CreateDelegate(ei.EventHandlerType, this, mi);
                            System.Reflection.MethodInfo eiAdd = ei.GetAddMethod();
                            object[] eiAddArgs = { del };
                            eiAdd.Invoke(webCtrl, eiAddArgs);
                        }
                    }
                }
            }
        }

        private void ClearContent()
        {
            ContentTemplateContainer.Controls.Clear();
            _contentTemplateContainer = null;
            ChildControls.ClearInternal();
        }

        private void CreateContents()
        {
            if (DesignMode) ClearContent();

            if (_contentTemplateContainer == null)
            {
                _contentTemplateContainer = new Control();
                if (_contentTemplate != null)
                    _contentTemplate.InstantiateIn(_contentTemplateContainer);

                ChildControls.AddInternal(_contentTemplateContainer);
            }
            else if (_contentTemplate != null)
                _contentTemplate.InstantiateIn(_contentTemplateContainer);
        }

        private LimitCollection ChildControls
        {
            get
            {
                if (Controls is LimitCollection)
                    return (LimitCollection)Controls;
                else
                    throw new InvalidOperationException("The controls property has the wrong class");
            }
        }

        private ITemplate _contentTemplate;
        private Control _contentTemplateContainer;
        private DialogButtonList _buttons;
        private TriggerList _openTriggers;
        private TriggerList _closeTriggers;
        private DialogClientEvents _clientEvents;

        private static readonly string[] PositionStrings = new string[] { "'top'", "'center'", "'bottom'", "['left','top']", "'left'", "['left','bottom']", "['right','top']", "'right'", "['right','bottom']" };

        #endregion

        #region Postback

        /// <summary>
        /// The event handler delegate for dialogs
        /// </summary>
        /// <param name="sender">The sending object</param>
        /// <param name="value">The event value</param>
        public delegate void DialogEventHandler(object sender, string value);

        /// <summary>
        /// The event fired when a button is pressed
        /// </summary>
        public event DialogEventHandler ButtonPressed;

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            if (ButtonPressed != null)
                ButtonPressed(this, eventArgument);
        }

        #endregion
    }

    /// <summary>
    /// The client events for the tree
    /// </summary>
    [ParseChildren(true)]
    public class DialogClientEvents : ViewStateBase
    {
        /// <summary>
        /// Javascript to call on creation
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string Create
        {
            get
            {
                if(ViewState["Create"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Create"];
            }
            set { ViewState["Create"] = value; }
        }

        /// <summary>
        /// Javascript to call when opened
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string Open
        {
            get
            {
                if (ViewState["Open"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Open"];
            }
            set { ViewState["Open"] = value; }
        }

        /// <summary>
        /// Javascript to call after closing
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string Close
        {
            get
            {
                if (ViewState["Close"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Close"];
            }
            set { ViewState["Close"] = value; }
        }

        /// <summary>
        /// Javascript to call before closing
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string BeforeClose
        {
            get
            {
                if (ViewState["BeforeClose"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["BeforeClose"];
            }
            set { ViewState["BeforeClose"] = value; }
        }

        /// <summary>
        /// Javascript to call when gaining focus
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string Focus
        {
            get
            {
                if(ViewState["Focus"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Focus"];
            }
            set { ViewState["Focus"] = value; }
        }

        /// <summary>
        /// Javascript to call when drag starts
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string DragStart
        {
            get
            {
                if(ViewState["DragStart"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["DragStart"];
            }
            set { ViewState["DragStart"] = value; }
        }

        /// <summary>
        /// Javascript to call when dragging dialog
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string Drag
        {
            get
            {
                if(ViewState["Drag"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Drag"];
            }
            set { ViewState["Drag"] = value; }
        }

        /// <summary>
        /// Javascript to call when dragging stops
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string DragStop
        {
            get
            {
                if(ViewState["DragStop"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["DragStop"];
            }
            set { ViewState["DragStop"] = value; }
        }

        /// <summary>
        /// Javascript to call when resize starts
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string ResizeStart
        {
            get
            {
                if(ViewState["ResizeStart"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["ResizeStart"];
            }
            set { ViewState["ResizeStart"] = value; }
        }

        /// <summary>
        /// Javascript to call when resizing
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string Resize
        {
            get
            {
                if(ViewState["Resize"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Resize"];
            }
            set { ViewState["Resize"] = value; }
        }

        /// <summary>
        /// Javascript to call when resizing stops
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string ResizeStop
        {
            get
            {
                if(ViewState["ResizeStop"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["ResizeStop"];
            }
            set { ViewState["ResizeStop"] = value; }
        }
    }

    /// <summary>
    /// THe buttons to appear in a dialog
    /// </summary>
    public class DialogButton : ViewStateBase
    {
        /// <summary>
        /// The text of the button
        /// </summary>
        [Bindable(true), DefaultValue(null)]
        public string Text
        {
            get
            {
                if(ViewState["Text"] != null)
                    return (string)ViewState["Text"];
                else
                    return null;
            }
            set { ViewState["Text"] = value; }
        }

        /// <summary>
        /// The Client side event to execute
        /// </summary>
        [Bindable(true), DefaultValue(null)]
        public string ClientSide
        {
            get
            {
                if(ViewState["ClientSide"] != null)
                    return (string)ViewState["ClientSide"];
                else
                    return null;
            }
            set { ViewState["ClientSide"] = value; }
        }

        /// <summary>
        /// Whether to do a postback
        /// </summary>
        [Bindable(true), DefaultValue(null)]
        public string PostbackArgument
        {
            get
            {
                if(ViewState["Postback"] != null)
                    return (string)ViewState["Postback"];
                else
                    return null;
            }
            set { ViewState["Postback"] = value; }
        }
    }

    /// <summary>
    /// Button list class for dialogs
    /// </summary>
    /// <remarks></remarks>
    public class DialogButtonList : ViewStateListBase<DialogButton>
    {
        ///
        protected internal override DialogButton Create(object state)
        {
            throw new NotImplementedException();
        }
    }
}