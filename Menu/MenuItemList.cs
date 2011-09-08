using System;
using System.Web.UI;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// A List of menu items
    /// </summary>
    public class MenuItemList : ViewStateListBase<MenuItem>
    {
        #region Constructor

        /// <summary>
        /// Creates an empty list
        /// </summary>
        public MenuItemList() { }

        /// <summary>
        /// Creates a new list for the parent menu item
        /// </summary>
        /// <param name="parent">Parent item</param>
        public MenuItemList(MenuItem parent)
        {
            _parent = parent;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns an item from the list below this point that matches the ID supplied
        /// </summary>
        /// <param name="ID">The ID to get</param>
        public MenuItem this[string ID]
        {
            get
            {
                foreach (MenuItem item in this)
                {
                    if (item.ID == ID)
                        return item;
                    else
                    {
                        MenuItem child = item.Items[ID];
                        if (child != null)
                            return child;
                    }
                }

                return null;
            }
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Adds an item to the list
        /// </summary>
        public override void Add(MenuItem item)
        {
            base.Add(item);
            if (_parent != null)
            {
                item.Parent = _parent;
                if (item.Parent.Owner != null)
                    item.Owner = _parent.Owner;
            }

            if (IsTrackingViewState)
                ((IStateManager)item).TrackViewState();

            item.SetDirty();
        }

        /// <summary>
        /// Removes an item from the list
        /// </summary>
        public new void Remove(MenuItem item)
        {
            if (this.Contains(item))
            {
                base.Remove(item);
                return;
            }
            else
            {
                foreach (MenuItemLink child in this)
                    child.Items.Remove(item);
            }
        }

        #endregion

        #region Internal

        /// <summary>
        /// Gets an item by its internal ID
        /// </summary>
        internal MenuItem ItemByInternalID(string InternalID)
        {
            foreach (MenuItem item in this)
            {
                if (item.InternalID == InternalID)
                    return item;
                else
                {
                    MenuItem child = item.Items.ItemByInternalID(InternalID);
                    if (child != null)
                        return child;
                }
            }

            return null;
        }

        /// <summary>
        /// Marks the viewstate as dirty for all items
        /// </summary>
        internal void SetDirty()
        {
            foreach (MenuItem item in this)
                item.SetDirty();
        }

        /// <summary>
        /// Sets the parent item for this list
        /// </summary>
        internal void SetParent(MenuItem parent)
        {
            _parent = parent;
            foreach (MenuItem item in this)
            {
                item.Parent = parent;
                item.Owner = parent.Owner;
            }
        }

        internal void SetOwner(Menu menu)
        {
            foreach (MenuItem item in this)
                item.Owner = menu;
        }

        /// <summary>
        /// Preps child items for rendering
        /// </summary>
        internal void OnPreRender()
        {
            foreach (MenuItem item in this)
                item.OnPreRender();
        }

        /// <summary>
        /// Renders all the child items
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        /// <param name="topLevel">True if this is a top level item</param>
        internal void Render(HtmlTextWriter writer, bool topLevel)
        {
            foreach (MenuItem item in this)
                item.Render(writer, topLevel);
        }

        #endregion

        #region Protected

        /// <summary>
        /// Creates the correct MenuItem and fills it with the state provided
        /// </summary>
        protected internal override MenuItem Create(object state)
        {
            object[] itemState = (object[])state;
            string itemName = itemState[0].ToString();
            MenuItem item = null;
            switch (itemName)
            {
                case "ESWCtrls.MenuItemText":
                    item = new MenuItemText();
                    break;
                case "ESWCtrls.MenuItemLink":
                    item = new MenuItemLink();
                    break;
                default:
                    System.Type mi = System.Type.GetType(itemName);
                    if (mi.IsSubclassOf(typeof(MenuItem)))
                    {
                        System.Reflection.ConstructorInfo ci = mi.GetConstructor(new Type[0]);
                        item = (MenuItem)ci.Invoke(null);
                    }
                    else
                        throw new ArgumentException("The state provided to MenuItemList is not a state from a menuitem sub class");
                    break;
            }
            item.LoadViewState(state);
            return item;
        }

        #endregion

        #region Private

        MenuItem _parent;

        #endregion
    }
}
