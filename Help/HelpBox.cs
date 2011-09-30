using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.Reflection;
using System.Web.Services;

namespace ESWCtrls
{
    using Internal;


    /// <summary>
    /// Help box style, used to display the help rovided by HelpPoints
    /// </summary>
    [ToolboxData("<{0}:helpbox runat=\"server\"></{0}:helpbox>"), ParseChildren(true), PersistChildren(false)]
    public class HelpBox : WebControl
    {
        #region Properties

        #region Appearance

        /// <summary>
        /// Gets the small box style.
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(PaddingStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public AdvStyle HoverBoxStyle
        {
            get
            {
                if(_hoverStyle == null)
                {
                    _hoverStyle = new AdvStyle();
                    if(IsTrackingViewState) ((IStateManager)_hoverStyle).TrackViewState();
                }

                return _hoverStyle;
            }
        }

        /// <summary>
        /// Gets the large box style.
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(PaddingStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public AdvStyle ClickBoxStyle
        {
            get
            {
                if(_clickStyle == null)
                {
                    _clickStyle = new AdvStyle();
                    if(IsTrackingViewState) ((IStateManager)_clickStyle).TrackViewState();
                }

                return _clickStyle;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the click box is modal.
        /// </summary>
        [Category("Appearance"),DefaultValue(false)]
        public bool ClickModal
        {
            get
            {
                if(ViewState["ClickModal"] != null)
                    return (bool)ViewState["ClickModal"];
                else
                    return false;
            }
            set
            {
                if(value != false)
                    ViewState["ClickModal"] = value;
                else
                    ViewState.Remove("ClickModal");
            }
        }

        #endregion

        #region Behaviour

        /// <summary>
        /// Gets or sets a value indicating whether to pre load hover help.
        /// </summary>
        [Bindable(true),DefaultValue(false)]
        public bool PreLoadHoverHelp
        {
            get
            {
                if(ViewState["PreLoadHover"] != null)
                    return (bool)ViewState["PreLoadHover"];
                else
                    return false;
            }
            set
            {
                if(value != false)
                    ViewState["PreLoadHover"] = value;
                else
                    ViewState.Remove("PreLoadHover");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to pre load click help.
        /// </summary>
        [Bindable(true), DefaultValue(false)]
        public bool PreLoadClickHelp
        {
            get
            {
                if(ViewState["PreLoadClick"] != null)
                    return (bool)ViewState["PreLoadClick"];
                else
                    return false;
            }
            set
            {
                if(value != false)
                    ViewState["PreLoadClick"] = value;
                else
                    ViewState.Remove("PreLoadClick");
            }
        }

        #endregion

        #region AjaxCallBack

        /// <summary>
        /// The delegate to get the actual help text for a help point
        /// </summary>
        /// <param name="id">The help id to get the Help for</param>
        /// <param name="click">True if this is the click help where asking for</param>
        public delegate string HelpDetailsHandler(string id, bool click);

        /// <summary>
        /// The event to get the actial gelp text for a help point
        /// </summary>
        public event HelpDetailsHandler HelpDetails;

        #endregion

        #endregion

        #region Internal Functions

        /// <summary>
        /// Gets the current Helpbox from a page
        /// </summary>
        /// <param name="ctrl">The Helppoint to find the helpbox for</param>
        internal static HelpBox Current(HelpPoint ctrl)
        {
            Control c = Util.FindControlRecursiveOut(ctrl, typeof(HelpBox), null);
            if(c != null && c is HelpBox)
            {
                HelpBox hb = c as HelpBox;
                if(hb._points == null)
                    hb._points = new List<HelpPoint>();
                hb._points.Add(ctrl);
                return hb;
            }
            else
                throw new Exception("A Helpbox is missing for use with helppoints");
        }

        #endregion

        #region Control Events

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Script.AddResourceScript(Page, "jquery.popup.js");
            Script.AddResourceScript(Page, "jquery.helpbox.js");

            string hoverHelp = null;
            string clickHelp = null;
            List<string> ctrls = new List<string>();
            foreach(HelpPoint hp in _points)
            {
                if(PreLoadHoverHelp && hp.HasHoverHelp && string.IsNullOrEmpty(hp.HoverHelp))
                    hoverHelp = HelpDetails(hp.HelpId, false);

                if(PreLoadClickHelp && hp.HasClickHelp && string.IsNullOrEmpty(hp.ClickHelp))
                    clickHelp = HelpDetails(hp.HelpId, true);

                string hps = hp.JSOptions(hoverHelp, clickHelp);
                if(!string.IsNullOrEmpty(hps))
                    ctrls.Add(hps);

                hoverHelp = clickHelp = null;
            }

            List<string> opts = new List<string>();
            opts.Add(string.Format("controls:[{0}]",string.Join(",",ctrls)));

            if(ClickModal)
                opts.Add("clickModal:true");

            if(HelpDetails != null)
            {
                MethodInfo mi = HelpDetails.Method;
                bool found = false;
                foreach(Attribute a in mi.GetCustomAttributes(true))
                {
                    if(a is WebMethodAttribute)
                    {
                        found = true;
                        break;
                    }
                }
                if(!found || !mi.IsStatic || !mi.IsPublic)
                    throw new ArgumentException("HelpDetails must be a static public web method.");

                opts.Add(string.Format("ajaxCallback:{{url:\"{0}/{1}\",paramHelpId:\"{2}\",paramClick:\"{3}\"}}", System.IO.Path.GetFileName(Page.Request.CurrentExecutionFilePath), mi.Name, mi.GetParameters()[0].Name, mi.GetParameters()[1].Name));
            }

            //Popup for Click
            Script.AddStartupScript(this, "ls_helpbox", opts);
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if(!HoverBoxStyle.IsEmpty)
                HoverBoxStyle.AddAttributesToRender(writer);
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_hover");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();

            if(!ClickBoxStyle.IsEmpty)
                ClickBoxStyle.AddAttributesToRender(writer);
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            writer.AddAttribute( HtmlTextWriterAttribute.Id, ClientID + "_click");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();

            writer.RenderEndTag();
        }

        #endregion

        #region Protected

        /// <summary>
        /// Causes the control to track changes to its view state so they can be stored in the object's <see cref="P:System.Web.UI.Control.ViewState"/> property.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();
            if(_hoverStyle != null) ((IStateManager)_hoverStyle).TrackViewState();
            if(_clickStyle != null) ((IStateManager)_clickStyle).TrackViewState();
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
                base.LoadViewState(states[0]);
                if(states[1] != null) ((IStateManager)HoverBoxStyle).LoadViewState(states[1]);
                if(states[2] != null) ((IStateManager)ClickBoxStyle).LoadViewState(states[2]);
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
            object[] states = new object[3];
            states[0] = base.SaveViewState();
            if(_hoverStyle != null) states[1] = ((IStateManager)_hoverStyle).SaveViewState();
            if(_clickStyle != null) states[2] = ((IStateManager)_clickStyle).SaveViewState();
            return states;
        }

        #endregion

        #region Private

        private AdvStyle _hoverStyle;
        private AdvStyle _clickStyle;
        List<HelpPoint> _points;

        #endregion
    }
}
