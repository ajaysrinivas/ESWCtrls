using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.Design;

namespace ESWCtrls
{

    /// <summary>
    /// The panel menu control.
    /// </summary>
    [ToolboxData("<{0}:PanelMenu runat=\"server\"></{0}:PanelMenu>")]
    public class PanelMenu : Menu
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
            set { ViewState["ShowEffect"] = value; }
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
            { ViewState["HideEffect"] = value; }
        }

        /// <summary>
        /// Whether to display the down arrow
        /// </summary>
        [Category("Appearance"), DefaultValue(false)]
        public bool DisplayArrow
        {
            get
            {
                if(this.ViewState["DisplayArrow"] == null)
                    return false;
                else
                    return (bool)this.ViewState["DisplayArrow"];
            }
            set { this.ViewState["DisplayArrow"] = value; }
        }

        /// <summary>
        /// Controls the styles for a down arrow
        /// </summary>
        [UrlProperty, Category("Appearance"), Editor(typeof(ImageUrlEditor), typeof(UITypeEditor)), DefaultValue(typeof(string), "string.Empty")]
        public string ArrowImage
        {
            get
            {
                if(this.ViewState["ArrowImage"] == null)
                    return this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "ESWCtrls.ResEmbed.Gfxs.arrow_down.gif");
                else
                    return (string)this.ViewState["ArrowImage"];
            }
            set { this.ViewState["ArrowImage"] = value; }
        }

        /// <summary>
        /// The style for a down arrow
        /// </summary>
        [DefaultValue(typeof(PaddingStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty), Category("Appearance")]
        public PaddingStyle ArrowStyle
        {
            get
            {
                if(this._arrowStyle == null)
                {
                    this._arrowStyle = new PaddingStyle();
                    if(base.IsTrackingViewState)
                        ((IStateManager)this._arrowStyle).TrackViewState();
                }
                return this._arrowStyle;
            }
        }

        #endregion

        #endregion

        #region Control Events

        ///
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Script.AddResourceScript(Page, "jquery.panelmenu.js");

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

            if(DisplayArrow)
                opts.Add(string.Format("DisplayArrow:true"));

            if(!string.IsNullOrEmpty(ArrowImage))
                opts.Add(string.Format("arrow:{{image:\"{0}\",class:\"{1}\"}}", Page.ResolveUrl(ArrowImage), ArrowStyle.RenderClass));

            if(ShowEffect != null)
                opts.Add("showEffect:" + ShowEffect.Render(Page));

            if(HideEffect != null)
                opts.Add("hideEffect:" + HideEffect.Render(Page));

            Script.AddStartupScript(this, ClientID, "ls_panelmenu", opts);

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
            if(this._arrowStyle != null)
                ((IStateManager)this._arrowStyle).TrackViewState();
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
                    ((IStateManager)this.ArrowStyle).LoadViewState(objArray[1]);
            }
        }

        ///
        protected override object SaveViewState()
        {
            object[] objArray = new object[2];
            objArray[0] = base.SaveViewState();

            if(this._arrowStyle != null)
                objArray[1] = ((IStateManager)this._arrowStyle).SaveViewState();

            if((objArray[0] == null) && (objArray[1] == null))
                return null;

            return objArray;
        }

        #endregion

        #region Private

        private PaddingStyle _arrowStyle;

        #endregion
    }
}
