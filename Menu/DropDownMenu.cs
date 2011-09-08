using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.Design;

namespace ESWCtrls
{

    /// <summary>
    /// The drop down menu control.
    /// </summary>
    [ToolboxData("<{0}:DropDownMenu runat=\"server\"></{0}:DropDownMenu>")]
    public class DropDownMenu : Menu
    {
        #region Properties

        #region Appearance

        /// <summary>
        /// The effect to run on show
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public Effect ShowEffect
        {
            get
            {
                if(ViewState["ShowEffect"] != null)
                    return (Effect)ViewState["ShowEffect"];
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
                    return (Effect)ViewState["HideEffect"];
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

        /// <summary>
        /// Controls which arrows to display
        /// </summary>
        [Category("Appearance"), DefaultValue(1)]
        public ArrowDisplayType ArrowDisplay
        {
            get
            {
                if(this.ViewState["ArrowDisplay"] == null)
                    return ArrowDisplayType.Both;
                else
                    return (ArrowDisplayType)this.ViewState["ArrowDisplay"];
            }
            set { this.ViewState["ArrowDisplay"] = value; }
        }

        /// <summary>
        /// Controls the styles for a down arrow
        /// </summary>
        [UrlProperty, Category("Appearance"), Editor(typeof(ImageUrlEditor), typeof(UITypeEditor)), DefaultValue(typeof(string), "string.Empty")]
        public string DownArrowImage
        {
            get
            {
                if(this.ViewState["TopArrowImage"] == null)
                    return this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "ESWCtrls.ResEmbed.Gfxs.arrow_down.gif");
                else
                    return (string)this.ViewState["TopArrowImage"];
            }
            set { this.ViewState["TopArrowImage"] = value; }
        }

        /// <summary>
        /// The style for a down arrow
        /// </summary>
        [DefaultValue(typeof(PaddingStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty), Category("Appearance")]
        public PaddingStyle DownArrowStyle
        {
            get
            {
                if(this._downArrowStyle == null)
                {
                    this._downArrowStyle = new PaddingStyle();
                    if(base.IsTrackingViewState)
                        ((IStateManager)this._downArrowStyle).TrackViewState();
                }
                return this._downArrowStyle;
            }
        }

        /// <summary>
        /// Controls the image used for the right arrows
        /// </summary>
        [UrlProperty, Editor(typeof(ImageUrlEditor), typeof(UITypeEditor)), DefaultValue(typeof(string), "string.Empty"), Category("Appearance")]
        public string RightArrowImage
        {
            get
            {
                if(this.ViewState["ArrowImage"] == null)
                    return this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "ESWCtrls.ResEmbed.Gfxs.arrow_right.gif");
                else
                    return (string)this.ViewState["ArrowImage"];
            }
            set { this.ViewState["ArrowImage"] = value; }
        }

        /// <summary>
        /// The style for a right arrow
        /// </summary>
        [NotifyParentProperty(true), DefaultValue(typeof(PaddingStyle), null), PersistenceMode(PersistenceMode.InnerProperty), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public PaddingStyle RightArrowStyle
        {
            get
            {
                if(this._rightArrowStyle == null)
                {
                    this._rightArrowStyle = new PaddingStyle();
                    if(base.IsTrackingViewState)
                        ((IStateManager)this._rightArrowStyle).TrackViewState();
                }
                return this._rightArrowStyle;
            }
        }

        #endregion

        #endregion

        #region Control Events

        ///
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Script.AddResourceScript(Page, "jquery.dropdownmenu.js");

            List<string> opts = new List<string>();

            if(!ItemStyle.IsEmpty)
                opts.Add(string.Format("itemClass:\"{0}\"", ItemStyle.RenderClass));

            if(!OpenStyle.IsEmpty)
                opts.Add(string.Format("openClass:\"{0}\"", OpenStyle.RenderClass));

            if(!HoverStyle.IsEmpty)
                opts.Add(string.Format("hoverClass:\"{0}\"", HoverStyle.RenderClass));

            if(!TopItemStyle.IsEmpty)
                opts.Add(string.Format("topItemClass:\"{0}\"", TopItemStyle.RenderClass));

            if(!TopHoverStyle.IsEmpty)
                opts.Add(string.Format("topHoverClass:\"{0}\"", TopHoverStyle.RenderClass));

            if(!DisabledStyle.IsEmpty)
                opts.Add(string.Format("disabledClass:\"{0}\"", DisabledStyle.RenderClass));

            if(ArrowDisplay != ArrowDisplayType.None)
                opts.Add(string.Format("arrowDisplayType:\"{0}\"", ArrowDisplay.ToString().ToLower()));

            if(!string.IsNullOrEmpty(DownArrowImage))
                opts.Add(string.Format("arrowDown:{{image:\"{0}\",class:\"{1}\"}}", Page.ResolveUrl(DownArrowImage), DownArrowStyle.RenderClass));

            if(!string.IsNullOrEmpty(RightArrowImage))
                opts.Add(string.Format("arrowRight:{{image:\"{0}\",class:\"{1}\"}}", Page.ResolveUrl(RightArrowImage), RightArrowStyle.RenderClass));

            if(ShowEffect != null)
                opts.Add("showEffect:" + ShowEffect.Render(Page));

            if(HideEffect != null)
                opts.Add("hideEffect:" + HideEffect.Render(Page));

            Script.AddStartupScript(this, "ls_dropmenu", opts);

            AdvStyle style = new AdvStyle();
            style.Display = Display.None;
            style.Position = ElementPosition.Absolute;
            Page.Header.StyleSheet.CreateStyleRule(style, this, string.Format("#{0} li ul", ClientID));
        }

        #endregion

        #region ViewState

        ///
        protected override void TrackViewState()
        {
            base.TrackViewState();
            if(this._downArrowStyle != null)
                ((IStateManager)this._downArrowStyle).TrackViewState();
            if(this._rightArrowStyle != null)
                ((IStateManager)this._rightArrowStyle).TrackViewState();
        }

        ///
        protected override void LoadViewState(object savedState)
        {
            object[] objArray = (object[])savedState;
            if(objArray != null)
            {
                if(objArray[0] != null)
                    base.LoadViewState(objArray[0]);

                if(objArray[1] != null)
                    ((IStateManager)this.DownArrowStyle).LoadViewState(objArray[1]);

                if(objArray[2] != null)
                    ((IStateManager)this.RightArrowStyle).LoadViewState(objArray[2]);
            }
        }

        ///
        protected override object SaveViewState()
        {
            object[] objArray = new object[3];
            objArray[0] = base.SaveViewState();

            if(this._downArrowStyle != null)
                objArray[1] = ((IStateManager)this._downArrowStyle).SaveViewState();

            if(this._rightArrowStyle != null)
                objArray[2] = ((IStateManager)this._rightArrowStyle).SaveViewState();

            if(((objArray[0] == null) && (objArray[1] == null)) && (objArray[2] == null))
                return null;

            return objArray;
        }

        #endregion

        #region Private

        private PaddingStyle _downArrowStyle;
        private PaddingStyle _rightArrowStyle;

        #endregion
    }


}