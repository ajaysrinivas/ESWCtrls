using System;
using System.Collections.Generic;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// The array of tab pages
    /// </summary>
    public class TabPageList : ViewStateListBase<TabPage>
    {
        #region Constructors

        /// <summary>
        /// Internal default constructor
        /// </summary>
        internal TabPageList()
        {
            _owner = null;
        }


        /// <summary>
        ///  Internal Contructor with owner
        /// </summary>
        internal TabPageList(TabControl owner)
        {
            _owner = owner;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns an item from the list with a matching title
        /// </summary>
        /// <param name="Title">The title to match</param>
        /// <returns>The matching page, or null if there was no matching page</returns>
        public TabPage this[string Title]
        {
            get
            {
                foreach (TabPage item in this)
                {
                    if (item.Title == Title)
                        return item;
                }

                return null;
            }
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Adds an item to the list
        /// </summary>
        public override void Add(TabPage item)
        {
            base.Add(item);
            if (_owner != null)
                item.Owner = _owner;

            item.SetDirty();
        }

        /// <summary>
        /// Removes an item from the list
        /// </summary>
        public new void Remove(TabPage item)
        {
            if (this.Contains(item))
            {
                base.Remove(item);
                return;
            }
        }

        /// <summary>
        /// Returns the index of the page with the matching title
        /// </summary>
        /// <param name="Title">The title to match</param>
        /// <returns>Index of page, or -1 if there is no matching page</returns>
        public int IndexOf(string Title)
        {
            foreach (TabPage item in this)
            {
                if (item.Title == Title)
                    return IndexOf(item);
            }
            return -1;
        }

        #endregion

        #region Protected

        ///
        protected internal override TabPage Create(object state)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Internal

        /// <summary>
        /// Marks the viewstate as dirty for all items
        /// </summary>
        internal void SetDirty()
        {
            foreach (TabPage item in this)
                item.SetDirty();
        }

        internal void SetOwner(TabControl owner)
        {
            _owner = owner;
            foreach (TabPage item in this)
                item.Owner = owner;
        }

        #endregion

        #region Private

        private TabControl _owner;

        #endregion
    }
}
