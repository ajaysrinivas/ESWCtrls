using System.Collections.Generic;
using System.Web.UI;

namespace ESWCtrls.Internal
{

    /// <summary>
    /// Base class for parts of control requiring viewstate
    /// </summary>
    public abstract class ViewStateBase : IStateManager
    {

        /// <summary>
        /// The viewstate of the object
        /// </summary>
        protected internal virtual StateBag ViewState
        {
            get
            {
                if (_viewState == null) _viewState = new StateBag();
                return _viewState;
            }
        }

        #region Private

        private StateBag _viewState;

        #endregion

        #region State Management

        /// <summary>
        /// Are we tracking viewstate
        /// </summary>
        protected internal bool IsTrackingViewState
        {
            get { return ((IStateManager)ViewState).IsTrackingViewState; }
        }

        /// <summary>
        /// Loads the view state
        /// </summary>
        /// <param name="state">The state to load from</param>
        protected internal virtual void LoadViewState(object state)
        {
            ((IStateManager)ViewState).LoadViewState(state);
        }

        /// <summary>
        /// Saves the view state
        /// </summary>
        /// <returns>The object containing the viewstate</returns>
        protected internal virtual object SaveViewState()
        {
            return ((IStateManager)ViewState).SaveViewState();
        }

        /// <summary>
        /// Sets if we are tracking view state
        /// </summary>
        protected internal virtual void TrackViewState()
        {
            ((IStateManager)ViewState).TrackViewState();
        }

        #endregion

        #region IStateManager Members

        bool IStateManager.IsTrackingViewState
        {
            get { return IsTrackingViewState; }
        }

        void IStateManager.LoadViewState(object state)
        {
            LoadViewState(state);
        }

        object IStateManager.SaveViewState()
        {
            return SaveViewState();
        }

        void IStateManager.TrackViewState()
        {
            TrackViewState();
        }

        #endregion
    }

    /// <summary>
    /// Base class for parts of control that are list and require viewstate
    /// </summary>
    /// <typeparam name="T">The type of class that requires viewstate</typeparam>
    /// <remarks>
    /// Classes derived from this should implement Create to allow for new items to be
    /// created on the fly. Solving some problems with dynamic controls 
    /// </remarks>
    public abstract class ViewStateListBase<T> : List<T>, IStateManager where T : IStateManager
    {

        #region Public

        /// <summary>
        /// Adds a collection of items to this list, and sets tracking viewstate
        /// on the new items if necessary
        /// </summary>
        public new virtual void AddRange(IEnumerable<T> Collection)
        {
            base.AddRange(Collection);
            if (_trackViewState)
            {
                foreach (T Item in this)
                    Item.TrackViewState();
            }
        }

        /// <summary>
        /// Adds a new iem to this list, setting trackviewstate if neccessary
        /// </summary>
        public new virtual void Add(T item)
        {
            base.Add(item);
            if (_trackViewState)
                item.TrackViewState();
        }

        #endregion

        #region Protected

        /// <summary>
        /// Creates a new item for the list
        /// </summary>
        protected internal abstract T Create(object state);

        #endregion

        #region Private

        private bool _trackViewState;

        #endregion

        #region State Management

        /// <summary>
        /// Are we tracking viewstate
        /// </summary>
        protected internal bool IsTrackingViewState
        {
            get { return _trackViewState; }
        }

        /// <summary>
        /// Loads the view state
        /// </summary>
        /// <param name="state">The state to load from</param>
        protected internal virtual void LoadViewState(object state)
        {
            if (state != null)
            {
                object[] itemStates = (object[])state;
                for (int i = 0; i < itemStates.Length; ++i)
                {
                    if (i < this.Count)
                        this[i].LoadViewState(itemStates[i]);
                    else if(itemStates[i] != null)
                        Add(Create(itemStates[i]));
                }
            }
        }

        /// <summary>
        /// Saves the view state
        /// </summary>
        /// <returns>The object containing the viewstate</returns>
        protected internal virtual object SaveViewState()
        {
            if (this.Count > 0)
            {
                object[] states = new object[this.Count];
                for (int i = 0; i < this.Count; ++i)
                    states[i] = this[i].SaveViewState();

                return states;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Sets if we are tracking view state
        /// </summary>
        protected internal virtual void TrackViewState()
        {
            _trackViewState = true;
            foreach (T Item in this)
                Item.TrackViewState();
        }

        #endregion

        #region IStateManager Members

        bool IStateManager.IsTrackingViewState
        {
            get { return IsTrackingViewState; }
        }

        void IStateManager.LoadViewState(object state)
        {
            LoadViewState(state);
        }

        object IStateManager.SaveViewState()
        {
            return SaveViewState();
        }

        void IStateManager.TrackViewState()
        {
            TrackViewState();
        }

        #endregion
    }

}