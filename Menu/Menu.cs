using System;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{

    /// <summary>
    /// The base class for all menus
    /// </summary>
    [ToolboxData("<{0}:DropDownMenu runat=\"server\"></{0}:DropDownMenu>")]
    public abstract class Menu : WebControl, IPostBackEventHandler
    {
        #region Properties

        #region Data

        /// <summary>
        /// Gets a list of menitems that represent the root items of the menu
        /// </summary>
        [DefaultValue(typeof(MenuItemList), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public MenuItemList Items
        {
            get { return Root.Items; }
            set { Root.Items = value; }
        }

        #endregion

        #region Appearance

        /// <summary>
        /// The size of icons in the menu
        /// </summary>
        [Category("Appearance"), MergableProperty(true)]
        public Size IconSize
        {
            get
            {

                if (ViewState["IconSize"] == null)
                    return new Size();
                else
                    return (Size)ViewState["IconSize"];
            }
            set { ViewState["IconSize"] = value; }
        }

        /// <summary>
        /// The size of top level icons in the menu if diffrent for normal
        /// </summary>
        [Category("Appearance")]
        public Size TopIconSize
        {
            get
            {
                if (ViewState["TopIconSize"] == null)
                    return new Size();
                else
                    return (Size)ViewState["TopIconSize"];
            }
            set { ViewState["TopIconSize"] = value; }
        }

        /// <summary>
        /// The style of the menu panels
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(PaddingStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public PaddingStyle PanelStyle
        {
            get
            {
                if (_panelStyle == null)
                {
                    _panelStyle = new PaddingStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_panelStyle).TrackViewState();
                }

                return _panelStyle;
            }
        }

        /// <summary>
        /// The style for a menu item
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(PaddingStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public PaddingStyle ItemStyle
        {
            get
            {
                if (_itemStyle == null)
                {
                    _itemStyle = new PaddingStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_itemStyle).TrackViewState();
                }

                return _itemStyle;
            }
        }

        /// <summary>
        /// The hover style for a menu item
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(PaddingStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public PaddingStyle HoverStyle
        {
            get
            {
                if (_hoverStyle == null)
                {
                    _hoverStyle = new PaddingStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_hoverStyle).TrackViewState();
                }

                return _hoverStyle;
            }
        }

        /// <summary>
        /// The open style for a menu item
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(PaddingStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public PaddingStyle OpenStyle
        {
            get
            {
                if (_openStyle == null)
                {
                    _openStyle = new PaddingStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_openStyle).TrackViewState();
                }

                return _openStyle;
            }
        }

        /// <summary>
        /// The disabled style for a menu item
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(PaddingStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public PaddingStyle DisabledStyle
        {
            get
            {
                if (_disabledStyle == null)
                {
                    _disabledStyle = new PaddingStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_disabledStyle).TrackViewState();
                }

                return _disabledStyle;
            }
        }

        /// <summary>
        /// The style for a top level menu item
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(PaddingStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public PaddingStyle TopItemStyle
        {
            get
            {
                if (_topItemStyle == null)
                {
                    _topItemStyle = new PaddingStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_topItemStyle).TrackViewState();
                }

                return _topItemStyle;
            }
        }

        /// <summary>
        /// The hover style for a top level menu item
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(PaddingStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public PaddingStyle TopHoverStyle
        {
            get
            {
                if (_topHoverStyle == null)
                {
                    _topHoverStyle = new PaddingStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_topHoverStyle).TrackViewState();
                }

                return _topHoverStyle;
            }
        }

        /// <summary>
        /// The open style for a top level menu item
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(PaddingStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public PaddingStyle TopOpenStyle
        {
            get
            {
                if (_topOpenStyle == null)
                {
                    _topOpenStyle = new PaddingStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_topOpenStyle).TrackViewState();
                }

                return _topOpenStyle;
            }
        }

        #endregion

        #endregion

        #region Control Events

        ///
        protected override void OnPreRender(EventArgs e)
        {
            if(!ItemStyle.IsEmpty)
                Page.Header.StyleSheet.RegisterStyle(ItemStyle, this);
            if(!OpenStyle.IsEmpty)
                Page.Header.StyleSheet.RegisterStyle(OpenStyle, this);
            if(!HoverStyle.IsEmpty)
                Page.Header.StyleSheet.RegisterStyle(HoverStyle, this);
            if(!TopItemStyle.IsEmpty)
                Page.Header.StyleSheet.RegisterStyle(TopItemStyle, this);
            if(!TopHoverStyle.IsEmpty)
                Page.Header.StyleSheet.RegisterStyle(TopHoverStyle, this);
            if(!DisabledStyle.IsEmpty)
                Page.Header.StyleSheet.RegisterStyle(DisabledStyle, this);

            Items.OnPreRender();

            AdvStyle style = new AdvStyle();
            style.ListStyleType = ListStyleType.None;
            Page.Header.StyleSheet.CreateStyleRule(style, this, string.Format("#{0}, #{0} ul", ClientID));

            base.OnPreRender(e);
        }

        ///
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            writer.AddStyleAttribute(HtmlTextWriterStyle.Margin, "0");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
            Items.Render(writer, true);
            writer.RenderEndTag();
        }

        /// <summary>
        /// Renders before the items content is rendered
        /// </summary>
        /// <param name="item">The current Item</param>
        /// <param name="writer">The writer to render to</param>
        /// <param name="topLevel">True if item is top level</param>
        internal virtual void RenderBeforeItemContent(MenuItem item, HtmlTextWriter writer, bool topLevel) { }

        /// <summary>
        /// Renders after the items content is rendered
        /// </summary>
        /// <param name="item">The current Item</param>
        /// <param name="writer">The writer to render to</param>
        /// <param name="topLevel">True if item is top level</param>
        internal virtual void RenderAfterItemContent(MenuItem item, HtmlTextWriter writer, bool topLevel) { }

        /// <summary>
        /// Additional rendering to perform before a menu panel is rendered
        /// </summary>
        /// <param name="item">The current Item</param>
        /// <param name="writer">The writer to render to</param>
        /// <param name="topLevel">True if item is top level</param>
        internal virtual void RenderBeforePanel(MenuItem item, HtmlTextWriter writer, bool topLevel) { }

        #endregion

        #region Postback

        /// <summary>
        /// The event handler for menu item events
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The menu item event arguments</param>
        public delegate void MenuItemEventHandler(object sender, MenuItemEventArgs e);

        /// <summary>
        /// A menu item has been clicked
        /// </summary>
        public event MenuItemEventHandler Click;

        /// <summary>
        /// A command menu item has been clicked
        /// </summary>
        public event CommandEventHandler Command;

        /// <summary>
        /// Raises a postback event for the menu
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
            MenuItem item = Items.ItemByInternalID(eventArgument);
            if (item != null)
            {
                if (Click != null)
                    Click(this, new MenuItemEventArgs(item));

                if (item is MenuItemLink && Command != null)
                {
                    Command(this, ((MenuItemLink)item).EventArgs);
                }
            }
        }

        #endregion

        #region ViewState

        ///
        protected override void LoadViewState(object savedState)
        {
            object[] state = (object[])savedState;
            if (state != null)
            {
                if (state[0] != null)
                    base.LoadViewState(state[0]);

                if (state[1] != null)
                    ((IStateManager)Items).LoadViewState(state[1]);

                if (state[2] != null)
                    ((IStateManager)PanelStyle).LoadViewState(state[2]);

                if (state[3] != null)
                    ((IStateManager)ItemStyle).LoadViewState(state[3]);

                if (state[4] != null)
                    ((IStateManager)HoverStyle).LoadViewState(state[4]);

                if (state[5] != null)
                    ((IStateManager)OpenStyle).LoadViewState(state[5]);

                if (state[6] != null)
                    ((IStateManager)DisabledStyle).LoadViewState(state[6]);

                if (state[7] != null)
                    ((IStateManager)TopItemStyle).LoadViewState(state[7]);

                if (state[8] != null)
                    ((IStateManager)TopHoverStyle).LoadViewState(state[8]);

                if (state[9] != null)
                    ((IStateManager)TopOpenStyle).LoadViewState(state[9]);
            }
        }

        /// 
        protected override object SaveViewState()
        {
            object[] state = new object[10];
            state[0] = base.SaveViewState();

            state[1] = ((IStateManager)Items).SaveViewState();

            if (_panelStyle != null)
                state[2] = ((IStateManager)_panelStyle).SaveViewState();

            if (_itemStyle != null)
                state[3] = ((IStateManager)_itemStyle).SaveViewState();

            if (_hoverStyle != null)
                state[4] = ((IStateManager)_hoverStyle).SaveViewState();

            if (_openStyle != null)
                state[5] = ((IStateManager)_openStyle).SaveViewState();

            if (_disabledStyle != null)
                state[6] = ((IStateManager)_disabledStyle).SaveViewState();

            if (_topItemStyle != null)
                state[7] = ((IStateManager)_topItemStyle).SaveViewState();

            if (_topHoverStyle != null)
                state[8] = ((IStateManager)_topHoverStyle).SaveViewState();

            if (_topOpenStyle != null)
                state[9] = ((IStateManager)_topOpenStyle).SaveViewState();

            if (state[0] == null && state[1] == null)
                return null;
            else
                return state;
        }

        ///
        protected override void TrackViewState()
        {
            base.TrackViewState();

            ((IStateManager)Items).TrackViewState();

            if (_panelStyle != null)
                ((IStateManager)_panelStyle).TrackViewState();

            if (_itemStyle != null)
                ((IStateManager)_itemStyle).TrackViewState();

            if (_hoverStyle != null)
                ((IStateManager)_hoverStyle).TrackViewState();

            if (_openStyle != null)
                ((IStateManager)_openStyle).TrackViewState();

            if (_disabledStyle != null)
                ((IStateManager)_disabledStyle).TrackViewState();

            if (_topItemStyle != null)
                ((IStateManager)_topItemStyle).TrackViewState();

            if (_topHoverStyle != null)
                ((IStateManager)_topHoverStyle).TrackViewState();

            if (_topOpenStyle != null)
                ((IStateManager)_topOpenStyle).TrackViewState();
        }

        #endregion

        #region Private

        private MenuItem Root
        {
            get
            {
                if (_data == null)
                {
                    _data = new MenuItemText();
                    _data.Owner = this;
                }

                return _data;
            }
        }

        MenuItem _data;

        PaddingStyle _panelStyle;

        PaddingStyle _itemStyle;
        PaddingStyle _hoverStyle;
        PaddingStyle _openStyle;
        PaddingStyle _disabledStyle;

        PaddingStyle _topItemStyle;
        PaddingStyle _topHoverStyle;
        PaddingStyle _topOpenStyle;

        #endregion
    }
}
