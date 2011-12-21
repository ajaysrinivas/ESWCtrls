using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI.Design;
using System.Drawing.Design;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// Displays help on a page, this is a control for a single help, the help is actually displayed by the <see cref="HelpBox">helpbox</see> control
    /// </summary>
    [ToolboxData("<{0}:helppoint runat=\"server\" />")]
    public class HelpPoint : WebControl
    {
        #region Properties

        #region Appearance

        /// <summary>
        /// Gets or sets the display type.
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(ImageText.Both)]
        public ImageText DisplayType
        {
            get
            {
                if(ViewState["DisplayType"] != null)
                    return (ImageText)ViewState["DisplayType"];
                else
                    return ImageText.Both;
            }
            set { ViewState["DisplayType"] = value; }
        }

        /// <summary>
        /// The text that appears on the page
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("help")]
        public string Text
        {
            get
            {
                if(ViewState["Text"] != null)
                    return (string)ViewState["Text"];
                else
                    return "help";
            }
            set { ViewState["Text"] = value; }
        }

        /// <summary>
        /// The image to display on the page for the help point
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(null), UrlProperty(), Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        public string ImageURL
        {
            get
            {
                if(ViewState["ImageURL"] != null)
                    return (string)ViewState["ImageURL"];
                else
                    return null;
            }
            set { ViewState["ImageURL"] = value; }
        }

        /// <summary>
        /// Gets the hover position.
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(typeof(Positioning), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public Positioning HoverPosition
        {
            get
            {
                if(_hoverPos == null)
                {
                    _hoverPos = new Positioning();
                    if(string.IsNullOrEmpty(ForControl))
                    {
                        _hoverPos.My = Position.CenterTop;
                        _hoverPos.At = Position.CenterBottom;
                    }
                    else
                    {
                        _hoverPos.My = Position.LeftBottom;
                        _hoverPos.At = Position.RightTop;
                    }
                    _hoverPos.Of = ClientID;
                    _hoverPos.Collision = Collision.Fit;
                    if(IsTrackingViewState) _hoverPos.TrackViewState();
                }

                return _hoverPos;
            }
        }

        /// <summary>
        /// Gets or sets the click position.
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(typeof(Positioning), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public Positioning ClickPosition
        {
            get
            {
                _clickPos = new Positioning();
                if(_clickPos == null)
                {
                    if(string.IsNullOrEmpty(ForControl))
                    {
                        _clickPos.My = Position.CenterCenter;
                        _clickPos.At = Position.CenterCenter;
                        _clickPos.Of = ClientID;
                    }
                    else
                    {
                        _clickPos.My = Position.LeftTop;
                        _clickPos.At = Position.LeftBottom;
                        Control c = Util.FindControlRecursiveOut(this, ForControl, null);
                        if(c != null)
                            _clickPos.Of = c.ClientID;
                        else
                            _clickPos.Of = ForControl;
                    }
                    _clickPos.Collision = Collision.Fit;
                    if(IsTrackingViewState) _hoverPos.TrackViewState();
                }

                return _clickPos;
            }
        }

        #endregion

        #region Behaviour

        /// <summary>
        /// The control this help is for, or null if no control for this help point
        /// </summary>
        [Bindable(true), Category("Behavior"), DefaultValue(null), IDReferenceProperty()]
        public string ForControl
        {
            get
            {
                if(ViewState["ForCtrl"] != null)
                    return (string)ViewState["ForCtrl"];
                else
                    return null;
            }
            set { ViewState["ForCtrl"] = value; }
        }

        #endregion

        #region Data

        /// <summary>
        /// Gets or sets the help id.
        /// </summary>
        /// <remarks>If not manually set builds a unqiue id from the page and forcontrol, or its own Id</remarks>
        [Bindable(true), Category("Data"), DefaultValue(null)]
        public string HelpId
        {
            get
            {
                if(ViewState["HelpId"] == null)
                {
                    string path = Util.BuildVirtPath(this);
                    if(!string.IsNullOrEmpty(ForControl))
                    {
                        Control c = Util.FindControlRecursiveOut(this, ForControl, null);
                        if(c != null)
                            ViewState["HelpId"] = path + "|" + c.ID;
                        else if(string.IsNullOrEmpty(ID))
                            ViewState["HelpId"] = path + "|" + ForControl;
                        else
                            ViewState["HelpId"] = path + "|" + ID;
                    }
                    else
                    {
                        if(string.IsNullOrEmpty(ID))
                            ViewState["HelpId"] = path + "|" + ClientID;
                        else
                            ViewState["HelpId"] = path + "|" + ID;
                    }
                }

                return (string)ViewState["HelpId"];
            }
            set { ViewState["HelpId"] = value; }
        }

        /// <summary>
        /// Gets or sets the hover help.
        /// </summary>
        [Bindable(true),Category("Data"), DefaultValue(null),PersistenceMode(PersistenceMode.InnerProperty)]
        public string HoverHelp
        {
            get
            {
                if(ViewState["HoverHelp"] != null)
                    return (string)ViewState["HoverHelp"];
                else
                    return null;
            }
            set
            {
                ViewState["HoverHelp"] = value;
                if(!string.IsNullOrEmpty(value))                    
                    HasHoverHelp = true;
            }
        }

        /// <summary>
        /// Gets or sets the click help.
        /// </summary>
        [Bindable(true), Category("Data"), DefaultValue(null),PersistenceMode(PersistenceMode.InnerProperty)]
        public string ClickHelp
        {
            get
            {
                if(ViewState["ClickHelp"] != null)
                    return (string)ViewState["ClickHelp"];
                else
                    return null;
            }
            set
            {
                ViewState["ClickHelp"] = value;
                if(!string.IsNullOrEmpty(value))                    
                    HasClickHelp = true;
            }
        }

        /// <summary>
        /// Whether the control has help on hover
        /// </summary>
        [Bindable(true), Category("Data"), DefaultValue(false)]
        public bool HasHoverHelp
        {
            get
            {
                if(ViewState["HasHoverHelp"] != null)
                    return (bool)ViewState["HasHoverHelp"];
                else
                    return false;
            }
            set { ViewState["HasHoverHelp"] = value; }
        }

        /// <summary>
        /// Whether the control has help on click
        /// </summary>
        [Bindable(true), Category("Data"), DefaultValue(false)]
        public bool HasClickHelp
        {
            get
            {
                if(ViewState["HasClickHelp"] != null)
                    return (bool)ViewState["HasClickHelp"];
                else
                    return false;
            }
            set { ViewState["HasClickHelp"] = value; }
        }

        #endregion

        #endregion

        #region Control Events

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            HelpBox.Current(this);
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if(!HasHoverHelp && !HasClickHelp)
                return;

            base.AddAttributesToRender(writer);
            if(DisplayType != ImageText.ImageOnly || string.IsNullOrEmpty(ImageURL))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write(Text);
            }

            if(!string.IsNullOrEmpty(ImageURL) && DisplayType != ImageText.TextOnly)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src,Page.ResolveUrl(ImageURL));
                writer.AddAttribute(HtmlTextWriterAttribute.Alt, "Help");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
            }

            if(DisplayType != ImageText.ImageOnly || string.IsNullOrEmpty(ImageURL))
            {
                writer.RenderEndTag();
            }
        }

        #endregion

        #region Internal

        /// <summary>
        /// Returns the relative id to use when position the help
        /// </summary>
        internal string RelativeID()
        {
            if(string.IsNullOrEmpty(ForControl))
                return ClientID;
            else
            {
                Control c = Util.FindControlRecursiveOut(this, ForControl, null);
                if(c != null)
                    return c.ClientID;
                else
                    return ForControl;
            }
        }

        internal string JSOptions(string preLoadHoverHelp, string preLoadClickHelp)
        {
            if(HasHoverHelp || HasClickHelp)
            {
                List<string> opts = new List<string>();
                opts.Add("id:\"" + ClientID + "\"");
                opts.Add("helpId:\"" + HelpId + "\"");

                if(!string.IsNullOrEmpty(ForControl))
                {
                    Control c = Util.FindControlRecursiveOut(this, ForControl, null);
                    if(c != null)
                        opts.Add(string.Format("forCtrl:\"{0}\"", c.ClientID));
                    else
                        opts.Add(string.Format("forCtrl:\"{0}\"", ForControl));
                }

                if(HasHoverHelp)
                {
                    opts.Add("hasHoverHelp:true");

                    if(!string.IsNullOrEmpty(HoverHelp))
                        opts.Add(string.Format("hoverHelp:\"{0}\"", Page.Server.HtmlEncode(HoverHelp)));
                    else if(!string.IsNullOrEmpty(preLoadHoverHelp))
                        opts.Add(string.Format("hoverHelp:\"{0}\"", Page.Server.HtmlEncode(preLoadHoverHelp)));

                    if(_hoverPos != null)
                        opts.Add(string.Format("hoverPosition:{0}", HoverPosition.JSOption(this)));
                }

                if(HasClickHelp)
                {
                    opts.Add("hasClickHelp:true");

                    if(!string.IsNullOrEmpty(ClickHelp))
                        opts.Add(string.Format("clickHelp:\"{0}\"", Page.Server.HtmlEncode(ClickHelp)));
                    else if(!string.IsNullOrEmpty(preLoadClickHelp))
                        opts.Add(string.Format("clickHelp:\"{0}\"", Page.Server.HtmlEncode(preLoadClickHelp)));

                    if(_clickPos != null)
                        opts.Add(string.Format("clickPosition:{0}", ClickPosition.JSOption(this)));
                }

                return "{" + string.Join(",", opts) + "}";
            }
            else
                return null;
        }

        #endregion

        #region Protected

        /// <summary>
        /// Causes the control to track changes to its view state so they can be stored in the object's <see cref="P:System.Web.UI.Control.ViewState"/> property.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();
            if(_hoverPos != null) _hoverPos.TrackViewState();
            if(_clickPos != null) _clickPos.TrackViewState();
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
                if(states[1] != null) HoverPosition.LoadViewState(states[1]);
                if(states[2] != null) ClickPosition.LoadViewState(states[2]);
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
            if(_hoverPos != null) states[1] = _hoverPos.SaveViewState();
            if(_clickPos != null) states[2] = _clickPos.SaveViewState();
            return states;
        }

        #endregion

        #region Private

        private Positioning _hoverPos;
        private Positioning _clickPos;

        #endregion
    }

    /// <summary>
    /// A list of help points
    /// </summary>
    public class HelpPointList : List<HelpPoint>
    {
        /// <summary>
        /// Returns the HelpPoint with the matching help id or null
        /// </summary>
        /// <param name="helpId">The help id to match</param>
        public HelpPoint this[string helpId]
        {
            get
            {
                foreach(HelpPoint hp in this)
                {
                    if(hp.HelpId == helpId)
                        return hp;
                }

                return null;
            }
        }
    }
}
