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
        /// Returns an item from the list with a matching id
        /// </summary>
        /// <param name="id">The Id to match</param>
        /// <returns>The matching page, or null if there was no matching page</returns>
        public TabPage this[string id]
        {
            get
            {
                foreach (TabPage item in this)
                {
                    if (item.Id == id)
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
        /// Returns the index of the page with the matching Id
        /// </summary>
        /// <param name="Id">The Id to match</param>
        /// <returns>Index of page, or -1 if there is no matching page</returns>
        public int IndexOf(string Id)
        {
            foreach (TabPage item in this)
            {
                if (item.Title == Id)
                    return IndexOf(item);
            }
            return -1;
        }

        /// <summary>
        /// Returns the index of the page with the matching Title
        /// </summary>
        /// <param name="Title">The Title to match</param>
        /// <returns>Index of page, or -1 if there is no matching page</returns>
        public int IndexOfTitle(string Title)
        {
            foreach(TabPage item in this)
            {
                if(item.Title == Title)
                    return IndexOf(item);
            }
            return -1;
        }

        /// <summary>
        /// Finds the first page whos title matches the given string
        /// </summary>
        /// <param name="Title">The title.</param>
        public TabPage ByTitle(string Title)
        {
            foreach(TabPage p in this)
            {
                if(p.Title == Title)
                    return p;
            }

            return null;
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
