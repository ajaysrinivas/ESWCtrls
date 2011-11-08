using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{
    using System.Drawing;
    using Internal;

    /// <summary>
    /// Displays a busybox on the page.
    /// </summary>
    /// <remarks>
    /// This control display a busybox/please wait.. etc for use when some task will take some time.
    /// You have complete control of the styling of the box, and it can handle animating graphics
    /// through javascript.
    /// If you specify a submitbutton then when that button is clicked ( or activated ) then the busybox appears.
    /// if submitbutton is empty then the busybox appears for any postback or redirect.
    /// </remarks>
    [ToolboxData("<{0}:BusyBox runat=\"server\"></{0}:BusyBox>")]
    public class BusyBox : WebControl
    {
        #region Properties

        /// <summary>
        /// Busy Box properties for Postback events
        /// </summary>
        [Bindable(true), Category("behaviour"), DefaultValue(typeof(BusyBoxPostBack), null), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public BusyBoxPostBack PostBack
        {
            get
            {
                if(_postback == null)
                {
                    _postback = new BusyBoxPostBack();
                    if(IsTrackingViewState)
                        ((IStateManager)_postback).TrackViewState();
                }

                return _postback;
            }
        }

        /// <summary>
        /// Busy Box properties for ajax events
        /// </summary>
        [Bindable(true), Category("behaviour"), DefaultValue(typeof(BusyBoxPostBack), null), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public BusyBoxAjax Ajax
        {
            get
            {
                if(_ajax == null)
                {
                    _ajax = new BusyBoxAjax();
                    if(IsTrackingViewState)
                        ((IStateManager)_ajax).TrackViewState();
                }

                return _ajax;
            }
        }

        #endregion

        #region Control events

        ///
        protected override void OnPreRender(System.EventArgs e)
        {
            if(PostBack.Enabled)
            { 
                Script.AddResourceScript(Page, "jquery.popup.js");
            }
            Script.AddResourceScript(Page, "BusyBox.js");

            PostBack.PreRender(this);
            Ajax.PreRender(this);

            Script.AddStartupScript(this, string.Format("ESW_SetupBusyBox({0});", ClientID));
        }

        ///
        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            PostBack.Render(writer, this);
            Ajax.Render(writer, this);

            writer.RenderEndTag();
        }

        #endregion

        #region ViewState

        ///
        protected override void TrackViewState()
        {
            base.TrackViewState();
            if(_postback != null)
                ((IStateManager)_postback).TrackViewState();
            if(_ajax != null)
                ((IStateManager)_ajax).TrackViewState();
        }

        ///
        protected override void LoadViewState(object savedState)
        {
            object[] states = (object[])savedState;
            if(states != null)
            {
                if(states[0] != null)
                    base.LoadViewState(states[0]);
                if(states[1] != null && _postback != null)
                    _postback.LoadViewState(states[1]);
                if(states[2] != null && _ajax != null)
                    _ajax.LoadViewState(states[2]);
            }
        }

        ///
        protected override object SaveViewState()
        {
            object[] states = new object[3];
            states[0] = base.SaveViewState();
            if(_postback != null)
                states[1] = ((IStateManager)_postback).SaveViewState();
            if(_ajax != null)
                states[2] = ((IStateManager)_ajax).SaveViewState();
            return states;
        }

        #endregion

        private BusyBoxPostBack _postback;
        private BusyBoxAjax _ajax;
    }

    /// <summary>
    /// base class for busy box types
    /// </summary>
    public abstract class BusyBoxBase : ViewStateBase
    {
        /// <summary>
        /// Prevent inheritance outside the lib
        /// </summary>
        internal BusyBoxBase() { }

        /// <summary>
        /// Whether this option is enabled
        /// </summary>
        [Category("Behavior"), DefaultValue(false)]
        public bool Enabled
        {
            get
            {
                if (ViewState["Enabled"] == null)
                    return false;
                else
                    return (bool)ViewState["Enabled"];
            }
            set { ViewState["Enabled"] = value; }
        }

        /// <summary>
        /// The style of the box
        /// </summary>
        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public AdvStyle BoxStyle
        {
            get
            {
                if (_boxStyle == null)
                {
                    _boxStyle = new AdvStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_boxStyle).TrackViewState();
                }
                return _boxStyle;
            }
        }

        /// <summary>
        /// The text for the title to appear before the graphic
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(string), null)]
        public string PreTitle
        {
            get
            {
                if (ViewState["PreTitle"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["PreTitle"];
            }
            set { ViewState["PreTitle"] = value; }
        }

        /// <summary>
        /// The style of the text to appear before the graphic
        /// </summary>
        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public AdvStyle PreTitleStyle
        {
            get
            {
                if (_preTitleStyle == null)
                {
                    _preTitleStyle = new AdvStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_preTitleStyle).TrackViewState();
                }
                return _preTitleStyle;
            }
        }

        /// <summary>
        /// The text to appear after the graphic
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(string), null)]
        public string PostTitle
        {
            get
            {
                if (ViewState["PostTitle"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["PostTitle"];
            }
            set { ViewState["PostTitle"] = value; }
        }

        /// <summary>
        /// The style of the text to appear after the graphic
        /// </summary>
        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public AdvStyle PostTitleStyle
        {
            get
            {
                if (_postTitleStyle == null)
                {
                    _postTitleStyle = new AdvStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_postTitleStyle).TrackViewState();
                }
                return _postTitleStyle;
            }
        }

        /// <summary>
        /// Any style to add to the graphic
        /// </summary>
        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public AdvStyle GfxStyle
        {
            get
            {
                if (_gfxStyle == null)
                {
                    _gfxStyle = new AdvStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_gfxStyle).TrackViewState();
                }
                return _gfxStyle;
            }
        }

        /// <summary>
        /// The graphic path to use for the busybox
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(string), null)]
        public string GfxPath
        {
            get
            {
                if (ViewState["GfxPath"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["GfxPath"];
            }
            set { ViewState["GfxPath"] = value; }
        }

        internal abstract void PreRender(Control parent);
        internal abstract void Render(HtmlTextWriter writer, Control parent);

        AdvStyle _boxStyle;
        AdvStyle _preTitleStyle;
        AdvStyle _postTitleStyle;
        AdvStyle _gfxStyle;
    }

    /// <summary>
    /// Information for AJAX busy box
    /// </summary>
    public class BusyBoxAjax : BusyBoxBase
    {
        /// <summary>
        /// If to cover the elements or hide them
        /// </summary>
        [Category("Behavior"), DefaultValue(false)]
        public bool CoverElements
        {
            get
            {
                if (ViewState["CoverElements"] == null)
                    return false;
                else
                    return (bool)ViewState["CoverElements"];
            }
            set { ViewState["CoverElements"] = value; }
        }

        /// <summary>
        /// If to cover the elements or hide them
        /// </summary>
        [Category("Behavior"), DefaultValue(typeof(Color),"Color.White")]
        public Color CoverColor
        {
            get
            {
                if (ViewState["CoverColor"] == null)
                    return Color.White;
                else
                    return (Color)ViewState["CoverColor"];
            }
            set { ViewState["CoverColor"] = value; }
        }

        /// <summary>
        /// If to cover the elements or hide them
        /// </summary>
        [Category("Behavior"), DefaultValue(0.5f)]
        public float CoverTransparency
        {
            get
            {
                if (ViewState["CoverTransparency"] == null)
                    return 0.5f;
                else
                    return (float)ViewState["CoverTransparency"];
            }
            set { ViewState["CoverTransparency"] = value; }
        }

        internal override void PreRender(Control parent)
        {
            ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID, "AX_Enabled", Enabled.ToString().ToLower(), false);
            ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID, "AX_Cover", CoverElements.ToString().ToLower(), false);
        }

        internal override void Render(HtmlTextWriter writer, Control parent)
        {
            if (!Enabled)
                return;

            if (!BoxStyle.IsEmpty)
                BoxStyle.AddAttributesToRender(writer);
            writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "absolute");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, parent.ClientID + "_ax");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if (!string.IsNullOrEmpty(PreTitle))
            {
                if (!PreTitleStyle.IsEmpty)
                    PreTitleStyle.AddAttributesToRender(writer);
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write(PreTitle);
                writer.RenderEndTag();
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Id, parent.ClientID + "_axgfx");
            writer.AddAttribute(HtmlTextWriterAttribute.Alt, "Busy Graphic");
            if (!string.IsNullOrEmpty(GfxPath))
                writer.AddAttribute(HtmlTextWriterAttribute.Src, parent.ResolveUrl(GfxPath));
            else
                writer.AddAttribute(HtmlTextWriterAttribute.Src, parent.Page.ClientScript.GetWebResourceUrl(parent.GetType(), "ESWCtrls.ResEmbed.Gfxs.busybox.busy.gif"));
            if (!GfxStyle.IsEmpty)
                GfxStyle.AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();

            if (!string.IsNullOrEmpty(PostTitle))
            {
                if (!PostTitleStyle.IsEmpty)
                    PostTitleStyle.AddAttributesToRender(writer);
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write(PostTitle);
                writer.RenderEndTag();
            }

            writer.RenderEndTag();

            if (CoverElements)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "absolute");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, ColorTranslator.ToHtml(CoverColor));
                writer.AddStyleAttribute("opacity", CoverTransparency.ToString("###.0"));
                writer.AddStyleAttribute("filter", "alpha(opacity=" + (CoverTransparency * 100).ToString("###") + ")");
                writer.AddAttribute(HtmlTextWriterAttribute.Id, parent.ClientID + "_ax_cover");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
            }
        }
    }

    /// <summary>
    /// Information for postback busy box
    /// </summary>
    public class BusyBoxPostBack : BusyBoxBase
    {
        /// <summary>
        /// The post fix to add to the graphic path
        /// </summary>
        [Category("Data"), DefaultValue(typeof(string), null)]
        public string GfxPostFix
        {
            get
            {
                if(ViewState["GfxPostFix"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["GfxPostFix"];
            }
            set { ViewState["GfxPostFix"] = value; }
        }

        /// <summary>
        /// The number of frames in the graphic
        /// </summary>
        [Category("Data"), DefaultValue(0)]
        public int GfxFrameCount
        {
            get
            {
                if(ViewState["GfxFrameCount"] == null)
                    return 0;
                else
                    return (int)ViewState["GfxFrameCount"];
            }
            set { ViewState["GfxFrameCount"] = value; }
        }

        /// <summary>
        /// The position of the graphics box
        /// </summary>
        [Category("Appearance"), DefaultValue(null), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public Positioning BoxPosition
        {
            get
            {
                if(_pos == null)
                {
                    _pos = new Positioning();
                    if(IsTrackingViewState)
                        _pos.TrackViewState();
                }

                return _pos;
            }
        }

        /// <summary>
        /// Any margib for the box
        /// </summary>
        [Category("Appearance"), DefaultValue(0)]
        public int BoxMargin
        {
            get
            {
                if(ViewState["BoxMargin"] == null)
                    return 0;
                else
                    return (int)ViewState["BoxMargin"];
            }
            set { ViewState["BoxMargin"] = value; }
        }

        /// <summary>
        /// Whether the box is to be modal
        /// </summary>
        [Category("Appearance"), DefaultValue(false)]
        public bool Modal
        {
            get
            {
                if(ViewState["Modal"] == null)
                    return false;
                else
                    return (bool)ViewState["Modal"];
            }
            set { ViewState["Modal"] = value; }
        }

        /// <summary>
        /// Z-index of box
        /// </summary>
        [Category("Appearance"), DefaultValue(0)]
        public int ZIndex
        {
            get
            {
                if (ViewState["ZIndex"] == null)
                    return 10;
                else
                    return (int)ViewState["ZIndex"];
            }
            set { ViewState["ZIndex"] = value; }
        }

        /// <summary>
        /// The button that triggers the busybox, or blank for any postback
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string SubmitButton
        {
            get
            {
                if(ViewState["SubmitButton"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["SubmitButton"];
            }
            set { ViewState["SubmitButton"] = value; }
        }

        internal override void PreRender(Control parent)
        {
            if(Enabled)
            {
                ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID, "PB_Enabled", "true", false);

                if(string.IsNullOrEmpty(GfxPath))
                {
                    ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "GFXBase", "", false);
                    ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "ResImg0", parent.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.busybox.busy00.png"), false);
                    ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "ResImg1", parent.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.busybox.busy01.png"), false);
                    ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "ResImg2", parent.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.busybox.busy02.png"), false);
                    ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "ResImg3", parent.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.busybox.busy03.png"), false);
                    ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "ResImg4", parent.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.busybox.busy04.png"), false);
                    ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "ResImg5", parent.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.busybox.busy05.png"), false);
                    ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "ResImg6", parent.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.busybox.busy06.png"), false);
                    ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "ResImg7", parent.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.busybox.busy07.png"), false);
                    ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "ResImg8", parent.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.busybox.busy08.png"), false);
                    ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "ResImg9", parent.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.busybox.busy09.png"), false);
                    ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "ResImg10", parent.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.busybox.busy10.png"), false);
                    ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "ResImg11", parent.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.busybox.busy11.png"), false);
                    ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "GFXFrameCount", "12", false);
                }
                else
                {
                    ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "GFXBase", parent.Page.ResolveUrl(GfxPath), false);
                    ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "GFXPostFix", GfxPostFix, false);
                    ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "GFXFrameCount", GfxFrameCount.ToString(), false);
                }
                //ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "position", BoxPosition.JSOption(parent),true);
                ScriptManager.RegisterClientScriptBlock(parent, parent.GetType(), parent.ClientID + "_pb", string.Format( "{0}_pb = {1};", parent.ClientID, BoxPosition.JSOption(parent)), true);
                ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "modal", Modal.ToString().ToLower(), false);

                if(!string.IsNullOrEmpty(SubmitButton))
                {
                    Control ctrl = Util.FindControlRecursive(parent.Page, SubmitButton);
                    if(ctrl != null)
                        ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "SubmitButton", ctrl.ClientID, false);
                    else
                        ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "SubmitButton", "", false);
                }
                else
                    ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID + "_pb", "SubmitButton", "", false);
            }
            else
                ScriptManager.RegisterExpandoAttribute(parent, parent.ClientID, "PB_Enabled", "false", false);
        }

        internal override void Render(HtmlTextWriter writer, Control parent)
        {
            if(!Enabled)
                return;

            if(!BoxStyle.IsEmpty)
                BoxStyle.AddAttributesToRender(writer);
            writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "absolute");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            writer.AddStyleAttribute(HtmlTextWriterStyle.ZIndex, ZIndex.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Id, parent.ClientID + "_pb");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if(!string.IsNullOrEmpty(PreTitle))
            {
                if(!PreTitleStyle.IsEmpty)
                    PreTitleStyle.AddAttributesToRender(writer);
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write(PreTitle);
                writer.RenderEndTag();
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Id, parent.ClientID + "_pb_gfx");
            writer.AddAttribute(HtmlTextWriterAttribute.Alt, "Busy Graphic");
            if(!string.IsNullOrEmpty(GfxPath))
                writer.AddAttribute(HtmlTextWriterAttribute.Src, parent.Page.ResolveUrl(GfxPath) + "00" + GfxPostFix);
            else
                writer.AddAttribute(HtmlTextWriterAttribute.Src, parent.Page.ClientScript.GetWebResourceUrl(parent.GetType(), "ESWCtrls.ResEmbed.Gfxs.busybox.busy00.png"));
            if(!GfxStyle.IsEmpty)
                GfxStyle.AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();

            if(!string.IsNullOrEmpty(PostTitle))
            {
                if(!PostTitleStyle.IsEmpty)
                    PostTitleStyle.AddAttributesToRender(writer);
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write(PostTitle);
                writer.RenderEndTag();
            }

            writer.RenderEndTag();
        }

        #region Viewstate

        /// <summary>
        /// Sets if we are tracking view state
        /// </summary>
        protected internal override void TrackViewState()
        {
            base.TrackViewState();
            if(_pos != null) _pos.TrackViewState();
        }

        /// <summary>
        /// Loads the state of the view.
        /// </summary>
        /// <param name="savedState">State of the saved.</param>
        protected internal override void LoadViewState(object savedState)
        {
            object[] state = (object[])savedState;
            if(state != null)
            {
                if(state[0] != null) base.LoadViewState(state[0]);
                if(state[1] != null) BoxPosition.LoadViewState(state[1]);
            }
        }

        /// <summary>
        /// Saves the view state
        /// </summary>
        /// <returns>
        /// The object containing the viewstate
        /// </returns>
        protected internal override object SaveViewState()
        {
            object[] state = new object[2];
            state[0] = base.SaveViewState();
            if(_pos != null) state[1] = _pos.SaveViewState();
            return state;
        }

        #endregion

        private Positioning _pos;
    }

}