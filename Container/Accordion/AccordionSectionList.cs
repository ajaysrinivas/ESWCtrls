using System;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// The list of Accordion Sections
    /// </summary>
    public class AccordionSectionList : ViewStateListBase<AccordionSection>
    {
        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        internal AccordionSectionList()
        {
            _owner = null;
        }

        /// <summary>
        /// Constructor with owning Accordion
        /// </summary>
        internal AccordionSectionList(Accordion owner)
        {
            _owner = owner;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns an item from the list with a matching title
        /// </summary>
        /// <param name="Title">The title to match</param>
        /// <returns>The matching section, or null if there was no matching page</returns>
        public AccordionSection this[string Title]
        {
            get
            {
                foreach(AccordionSection item in this)
                {
                    if(item.Title == Title)
                        return item;
                }

                return null;
            }
        }

        /// <summary>
        /// Returns the index of the sectopm with the matching title
        /// </summary>
        /// <param name="Title">The title to match</param>
        /// <returns>Index of section, or -1 if there is no matching page</returns>
        public int IndexOf(string Title)
        {
            foreach(AccordionSection item in this)
            {
                if(item.Title == Title)
                    return IndexOf(item);
            }
            return -1;
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Adds an item to the list
        /// </summary>
        public override void Add(AccordionSection item)
        {
            base.Add(item);
            if(_owner != null)
                item.Owner = _owner;

            item.SetDirty();
        }

        /// <summary>
        /// Removes an item from the list
        /// </summary>
        public new void Remove(AccordionSection item)
        {
            if(this.Contains(item))
            {
                base.Remove(item);
                return;
            }
        }

        #endregion

        #region Protected

        ///
        protected internal override AccordionSection Create(object state)
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
            foreach(AccordionSection item in this)
                item.SetDirty();
        }

        internal void SetOwner(Accordion owner)
        {
            _owner = owner;
            foreach(AccordionSection item in this)
                item.Owner = owner;
        }

        #endregion

        #region Private

        private Accordion _owner;

        #endregion
    }
}
