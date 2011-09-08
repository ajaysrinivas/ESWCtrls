using System;
using System.ComponentModel;
using System.Web.UI;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// The base class for menu items
    /// </summary>
    public abstract class MenuItem : ViewStateBase
    {
        #region Constructors

        /// <summary>
        /// Blank Constructor
        /// </summary>
        public MenuItem()
        {
            _parent = null;
        }

        /// <summary>
        /// Constructor with ID
        /// </summary>
        internal MenuItem(string ID)
            : this()
        {
            this.ID = ID;
        }

        #endregion

        #region Properties

        #region Appearance

        /// <summary>
        /// The style for the node
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
        /// The hover style for the node
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
        /// The open style for the node
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

        #endregion

        #region Data

        /// <summary>
        /// The ID of the node
        /// </summary>
        [Category("Data"), DefaultValue(typeof(string), "string.Empty")]
        public string ID
        {
            get
            {
                if (ViewState["ID"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["ID"];
            }
            set { ViewState["ID"] = value; }
        }

        /// <summary>
        /// The child nodes of this node
        /// </summary>
        [Category("Data"), Browsable(false), DefaultValue(typeof(MenuItemList), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public MenuItemList Items
        {
            get
            {
                if (_items == null)
                    _items = new MenuItemList(this);

                return _items;
            }
            internal set
            {
                _items = value;
                _items.SetParent(this);
            }
        }

        /// <summary>
        /// The parent of the node
        /// </summary>
        [Category("Data"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual MenuItem Parent
        {
            get { return _parent; }
            internal set
            {
                _parent = value;
                if (_items != null)
                    _items.SetParent(this);
            }
        }

        #endregion

        #region Behaviour

        /// <summary>
        /// Whether the item is enabled
        /// </summary>
        [Category("Behaviour"), DefaultValue(true)]
        public bool Enabled
        {
            get
            {
                if (ViewState["Enabled"] == null)
                    return true;
                else
                    return (bool)ViewState["Enabled"];
            }
            set { ViewState["Enabled"] = value; }
        }

        #endregion

        #endregion

        #region Internal

        /// <summary>
        /// returns an item that contains the child specified
        /// </summary>
        /// <param name="item">The item to get the parent for</param>
        internal MenuItem GetParent(MenuItem item)
        {
            foreach (MenuItem child in Items)
            {
                if (child.Equals(item))
                    return this;
                else
                {
                    MenuItem rst = child.GetParent(item);
                    if (rst != null)
                        return rst;
                }
            }

            return null;
        }

        /// <summary>
        /// The internal ID for the item within the full menu
        /// </summary>
        internal string InternalID
        {
            get
            {
                if (_parent == null)
                    return string.Empty;
                else
                {
                    if (_parent.InternalID == string.Empty)
                        return _parent.Items.IndexOf(this).ToString();
                    else
                        return _parent.InternalID + "_" + _parent.Items.IndexOf(this).ToString();
                }
            }
        }

        /// <summary>
        /// If the node is dirty
        /// </summary>
        internal virtual void SetDirty()
        {
            ViewState.SetDirty(true);
            if (_itemStyle != null)
                _itemStyle.SetDirty();
            if (_openStyle != null)
                _itemStyle.SetDirty();
            if (_hoverStyle != null)
                _itemStyle.SetDirty();
        }

        /// <summary>
        /// Sets the owner of the item
        /// </summary>
        internal virtual Menu Owner
        {
            get { return _owner; }
            set
            {
                _owner = value;
                if (_items != null)
                    _items.SetOwner(value);
            }
        }

        #endregion

        #region Rendering

        /// <summary>
        /// Registers the additional style for the item
        /// </summary>
        internal virtual void OnPreRender()
        {
            if(_itemStyle != null && !_itemStyle.IsEmpty)
                _owner.Page.Header.StyleSheet.RegisterStyle(_itemStyle, _owner);
            if(_openStyle != null && !_openStyle.IsEmpty)
                _owner.Page.Header.StyleSheet.RegisterStyle(_openStyle, _owner);
            if(_owner.Enabled && _hoverStyle != null && !_hoverStyle.IsEmpty)
                _owner.Page.Header.StyleSheet.RegisterStyle(_hoverStyle, _owner);
            Items.OnPreRender();
        }

        /// <summary>
        /// Does the rendering of the item
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        /// <param name="topLevel">True if this is a top level item</param>
        internal abstract void Render(HtmlTextWriter writer, bool topLevel);

        #endregion

        #region RenderParts

        /// <summary>
        /// Renders the start of the menuitem
        /// </summary>
        protected void RenderStart(HtmlTextWriter writer, bool topLevel)
        {
            // The start tag for the item
            writer.AddAttribute(HtmlTextWriterAttribute.Id, _owner.ClientID + "_" + InternalID);

            string cssClass = ItemStyle.RenderClass;

            if (!_owner.ItemStyle.IsEmpty)
                cssClass += " " + _owner.ItemStyle.RenderClass;

            if (topLevel && !_owner.TopItemStyle.IsEmpty)
                cssClass += " " + _owner.TopItemStyle.RenderClass;

            if (!Enabled && !_owner.DisabledStyle.IsEmpty)
                cssClass += " " + _owner.DisabledStyle.RenderClass;

            if (!string.IsNullOrEmpty(cssClass.Trim()))
                writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass.Trim());

            writer.RenderBeginTag(HtmlTextWriterTag.Li);
        }

        /// <summary>
        /// Renders the end of the menuitem
        /// </summary>
        protected void RenderEnd(HtmlTextWriter writer, bool topLevel)
        {
            if (Items.Count > 0)
            {
                if (!_owner.PanelStyle.IsEmpty)
                    _owner.PanelStyle.AddAttributesToRender(writer);
                writer.AddStyleAttribute(HtmlTextWriterStyle.Margin, "0");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "0");
                Owner.RenderBeforePanel(this, writer, topLevel);
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);

                Items.Render(writer, false);

                writer.RenderEndTag();
            }

            // End the tag for the item
            writer.RenderEndTag();
        }

        #endregion

        #region ViewState

        ///
        protected internal override void TrackViewState()
        {
            base.TrackViewState();

            if (_items != null)
                ((IStateManager)_items).TrackViewState();

            if (_itemStyle != null)
                ((IStateManager)_itemStyle).TrackViewState();

            if (_hoverStyle != null)
                ((IStateManager)_hoverStyle).TrackViewState();

            if (_openStyle != null)
                ((IStateManager)_openStyle).TrackViewState();
        }

        ///
        protected internal override void LoadViewState(object state)
        {
            object[] itemState = (object[])state;

            if (itemState != null)
            {
                if (itemState[1] != null)
                    base.LoadViewState(itemState[1]);

                if (itemState[2] != null)
                    ((IStateManager)Items).LoadViewState(itemState[2]);

                if (itemState[3] != null)
                    ((IStateManager)ItemStyle).LoadViewState(itemState[3]);

                if (itemState[4] != null)
                    ((IStateManager)HoverStyle).LoadViewState(itemState[4]);

                if (itemState[5] != null)
                    ((IStateManager)OpenStyle).LoadViewState(itemState[5]);
            }
        }

        ///
        protected internal override object SaveViewState()
        {
            object[] state = new object[6];

            state[0] = this.GetType().FullName;

            state[1] = base.SaveViewState();

            if (_items != null)
                state[2] = ((IStateManager)_items).SaveViewState();

            if (_itemStyle != null)
                state[3] = ((IStateManager)_itemStyle).SaveViewState();

            if (_hoverStyle != null)
                state[4] = ((IStateManager)_hoverStyle).SaveViewState();

            if (_openStyle != null)
                state[5] = ((IStateManager)_openStyle).SaveViewState();

            if (state[0] == null && state[1] == null && state[2] == null && state[3] == null && state[4] == null && state[5] == null)
                return null;

            return state;
        }

        #endregion

        #region Private

        private MenuItemList _items;
        private MenuItem _parent;
        private Menu _owner;

        private PaddingStyle _itemStyle;
        private PaddingStyle _openStyle;
        private PaddingStyle _hoverStyle;

        #endregion
    }

    #region Event Handling

    /// <summary>
    /// Menu Event Arguments for use by Menus
    /// </summary>
    public class MenuItemEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs a new Menu Item event arguments class
        /// </summary>
        public MenuItemEventArgs(MenuItem item)
        {
            _item = item;
        }

        /// <summary>
        /// The item that caused the command event
        /// </summary>
        public MenuItem Item
        {
            get { return _item; }
        }

        MenuItem _item;
    }

    #endregion
}
