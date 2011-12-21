using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{

    /// <summary>
    /// Tab control either client or server side selection
    /// </summary>
    /// <remarks>
    ///     When using jQuery the styling control is through the jQuery tabs and the current tab look is ignored
    ///     Also, collapsible is only available when using jQuery.
    /// </remarks>
    [ToolboxData("<{0}:tabcontrol runat=\"server\"></{0}:tabcontrol>"), ParseChildren(true), PersistChildren(false)]
    public class TabControl : WebControl, IPostBackDataHandler, IPostBackEventHandler
    {
        #region Properties

        #region Appearance

        /// <summary>
        /// The style of the tab bar
        /// </summary>
        [Category("Appearance"), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public TableItemStyle TabBarStyle
        {
            get
            {
                if (_tabBarStyle == null)
                {
                    _tabBarStyle = new TableItemStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_tabBarStyle).TrackViewState();
                }
                return _tabBarStyle;
            }
        }

        /// <summary>
        /// The style for the active tab
        /// </summary>
        [Category("Appearance"), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public TabButtonStyle ActiveTabStyle
        {
            get
            {
                if (_activeTabStyle == null)
                {
                    _activeTabStyle = new TabButtonStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_activeTabStyle).TrackViewState();
                }
                return _activeTabStyle;
            }
        }

        /// <summary>
        /// The style for the inactive tab
        /// </summary>
        [Category("Appearance"), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public TabButtonStyle InactiveTabStyle
        {
            get
            {
                if (_inactiveTabStyle == null)
                {
                    _inactiveTabStyle = new TabButtonStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_inactiveTabStyle).TrackViewState();
                }
                return _inactiveTabStyle;
            }
        }

        /// <summary>
        /// The page style
        /// </summary>
        [Category("Appearance"), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public TableItemStyle PageStyle
        {
            get
            {
                if (_pageStyle == null)
                {
                    _pageStyle = new TableItemStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_pageStyle).TrackViewState();
                }
                return _pageStyle;
            }
        }

        /// <summary>
        /// Whether to use jquery styles, when not a jquery control
        /// </summary>
        [Category("Appearance"), DefaultValue(false)]
        public bool UsejQueryStyle
        {
            get
            {
                if(ViewState["UsejQueryStyle"] != null)
                    return (bool)ViewState["UsejQueryStyle"];
                else
                    return false;
            }
            set { ViewState["UsejQueryStyle"] = value; }
        }

        #endregion

        #region Data

        /// <summary>
        /// The pages to display
        /// </summary>
        [Category("Data"), PersistenceMode(PersistenceMode.InnerProperty), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), MergableProperty(false)]
        public TabPageList Pages
        {
            get
            {
                if (_pages == null)
                    _pages = new TabPageList(this);
                return _pages;
            }
        }

        /// <summary>
        /// The index of the active page
        /// </summary>
        [Category("Data"), DefaultValue(0)]
        public int ActivePageIndex
        {
            get
            {
                if (ViewState["ActivePageIdx"] == null)
                    return 0;
                else
                {
                    int idx = (int)ViewState["ActivePageIdx"];
                    if (idx < 0 || idx >= _pages.Count)
                    {
                        ViewState["ActivePageIdx"] = 0;
                        return 0;
                    }
                    else
                        return idx;
                }
            }
            set { ViewState["ActivePageIdx"] = value; }
        }

        /// <summary>
        /// The title of the active page
        /// </summary>
        [Category("Data")]
        public string ActivePageTitle
        {
            get { return _pages[ActivePageIndex].Title; }
            set { ActivePageIndex = _pages.IndexOfTitle(value) >= 0 ? _pages.IndexOfTitle(value) : 0; }
        }

        /// <summary>
        /// Gets or sets the active page by id.
        /// </summary>
        [Category("Data")]
        public string ActivePageId
        {
            get { return _pages[ActivePageIndex].Id; }
            set { ActivePageIndex = _pages.IndexOf(value) >= 0 ? _pages.IndexOf(value) : 0; }
        }

        /// <summary>
        /// The active page
        /// </summary>
        [Browsable(false)]
        public TabPage ActivePage
        {
            get { return _pages[ActivePageIndex]; }
        }

        /// <summary>
        /// The sorted order of the pages
        /// </summary>
        /// <remarks>
        /// Returns a list&lt;int&gt; with the tabs button order of the pages. You CANNOT change the order
        /// by doing:
        /// <code>
        ///     TabCtrl.SortOrder[0] = 2; 
        ///     TabCtrl.SortOrder[2] = 0; 
        /// </code>
        /// This will have no effect take the value, manipulate all the changes and then assign back:
        /// <code>
        ///     List&lt;int&gt; order = TabCtrl.SortOrder;
        ///     order[0] = 2;
        ///     order[2] = 0;
        ///     TabCtrl.SortOrder = order;
        /// </code>
        /// This is to guartenee consistancy, and the assigned values are checked to make sure its the right
        /// length, and that all the pages are present. (Stops TabCtrl.SortOrder.Add(2), etc..)
        /// </remarks>
        [Browsable(false)]
        public List<int> SortOrder
        {
            get
            {
                if (_sortOrder == null)
                {
                    if (ViewState["SortOrder"] != null)
                        _sortOrder = (List<int>)ViewState["SortOrder"];
                    else
                    {
                        _sortOrder = new List<int>();
                        for (int i = 0; i < Pages.Count; ++i)
                            _sortOrder.Add(i);
                    }
                }

                if (_sortOrder.Count < Pages.Count)
                {
                    for (int i = _sortOrder.Count; i < Pages.Count; ++i)
                        _sortOrder.Add(i);
                }
                else if(_sortOrder.Count > Pages.Count)
                {
                    for (int i = Pages.Count; i < _sortOrder.Count; ++i)
                        _sortOrder.Remove(i);
                }

                return new List<int>(_sortOrder);
            }
            set
            {
                if (value.Count != Pages.Count)
                    throw new ArgumentException("The new sort order does not have the correct number of values");

                bool[] check = new bool[Pages.Count];
                foreach (int i in value)
                    check[i] = true;

                foreach (bool i in check)
                {
                    if (!i)
                        throw new ArgumentException("A page is missing from the sort order");
                }

                _sortOrder = value;
            }
        }

        #endregion

        #region Behavior

        /// <summary>
        /// Whether to control tabs client or server side, or with jQuery
        /// </summary>
        [Category("Behaviour"), DefaultValue(TabStyle.ServerSide)]
        public TabStyle TabStyle
        {
            get
            {
                if (ViewState["TabStyle"] == null)
                    return TabStyle.ServerSide;
                else
                    return (TabStyle)ViewState["TabStyle"];
            }
            set { ViewState["TabStyle"] = value; }
        }

        /// <summary>
        /// Whether an already selected tab can become collapsed (jQuery only)
        /// </summary>
        [Category("Behaviour"), DefaultValue(false)]
        public bool Collapsible
        {
            get
            {
                if(ViewState["Collapsible"] != null)
                    return (bool)ViewState["Collapsible"];
                else
                    return false;
            }
            set { ViewState["Collapsible"] = value; }
        }

        /// <summary>
        /// Whether the tabs are sortable
        /// </summary>
        [Category("Behaviour"), DefaultValue(false)]
        public bool Sortable
        {
            get
            {
                if(ViewState["Sortable"] != null)
                    return (bool)ViewState["Sortable"];
                else
                    return false;
            }
            set { ViewState["Sortable"] = value; }
        }

        #endregion

        #endregion

        #region Control Events

        ///
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Page.RegisterRequiresPostBack(this);
        }

        ///
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!UsejQueryStyle && TabStyle != TabStyle.jQuery)
            {
                if (_activeTabStyle != null && !_activeTabStyle.IsEmpty)
                    Page.Header.StyleSheet.RegisterStyle(ActiveTabStyle, this);
                if (_activeTabStyle != null && !_inactiveTabStyle.IsEmpty)
                    Page.Header.StyleSheet.RegisterStyle(InactiveTabStyle, this);

                if (!string.IsNullOrEmpty(ActiveTabStyle.BackImage))
                {
                    _leftGfxAct = new TabGfx(Page.Server.MapPath(ActiveTabStyle.BackImage), ActiveTabStyle.ImageLeftSize, false);
                    _rightGfxAct = new TabGfx(Page.Server.MapPath(ActiveTabStyle.BackImage), ActiveTabStyle.ImageRightSize, true);
                    _midGfxAct = new TabGfx(Page.Server.MapPath(ActiveTabStyle.BackImage), 1, false);

                    _leftGfxInact = new TabGfx(Page.Server.MapPath(InactiveTabStyle.BackImage), InactiveTabStyle.ImageLeftSize, false);
                    _rightGfxInact = new TabGfx(Page.Server.MapPath(InactiveTabStyle.BackImage), InactiveTabStyle.ImageRightSize, true);
                    _midGfxInact = new TabGfx(Page.Server.MapPath(InactiveTabStyle.BackImage), 1, false);
                }

                AdvStyle style = new AdvStyle();
                style.ListStyleType = ListStyleType.None;
                style.PaddingLeft = new Unit("0");
                Page.Header.StyleSheet.CreateStyleRule(style, this, string.Format("#{0}_tabbar", ClientID));
                style = new AdvStyle();
                style.Float = ElementFloat.Left;
                Page.Header.StyleSheet.CreateStyleRule(style, this, string.Format("#{0}_tabbar li", ClientID));

            }

            if (TabStyle == TabStyle.ClientSide)
            {
                Script.AddResourceScript(Page, "jquery.tabctrl.js");

                List<string> opts = new List<string>();

                if (UsejQueryStyle)
                {
                    opts.Add("activeClass:\"ui-state-default ui-corner-top ui-tabs-selected ui-state-active\"");
                    opts.Add("inactiveClass:\"ui-state-default ui-corner-top\"");
                }
                else
                {
                    if (!string.IsNullOrEmpty(ActiveTabStyle.BackImage))
                    {
                        opts.Add("useImages:true");
                        opts.Add(string.Format("left:{{active:\"{0}{2}\",inactive:\"{1}{2}\"}}", _leftGfxAct.FileName, _leftGfxInact.FileName, Graphic.WebGfx.Extension));
                        opts.Add(string.Format("middle:{{active:\"{0}{2}\",inactive:\"{1}{2}\"}}", _midGfxAct.FileName, _midGfxInact.FileName, Graphic.WebGfx.Extension));
                        opts.Add(string.Format("right:{{active:\"{0}{2}\",inactive:\"{1}{2}\"}}", _rightGfxAct.FileName, _rightGfxInact.FileName, Graphic.WebGfx.Extension));
                    }


                    opts.Add("activeClass:\"" + ActiveTabStyle.RenderClass + "\"");
                    opts.Add("inactiveClass:\"" + InactiveTabStyle.RenderClass + "\"");
                }

                Script.AddStartupScript(this, "ls_tabctrl", opts);
                ScriptManager.RegisterHiddenField(Page, ClientID + "_actIdx", ActivePageIndex.ToString());
            }
            else if (TabStyle == TabStyle.jQuery)
            {
                Script.AddResourceScript(Page, "jquery.ui.tabs.js");

                List<string> opts = new List<string>();

                if (!Enabled)
                    opts.Add("disabled:true");
                if (Collapsible)
                    opts.Add("collapsible:true");
                if (SortOrder.IndexOf(ActivePageIndex) != 0)
                    opts.Add(string.Format("selected:{0}", SortOrder.IndexOf(ActivePageIndex)));

                opts.Add(string.Format("select:function(event,ui) {{ $(\"#{0}_actIdx\").val($(ui.tab).attr(\"oa\")); }}", ClientID));

                Script.AddStartupScript(this, "tabs", opts);
                ScriptManager.RegisterHiddenField(Page, ClientID + "_actIdx", ActivePageIndex.ToString());
            }

            if (Sortable)
            {
                Script.AddResourceScript(Page, "jquery.ui.sortable.js");
                StringBuilder sb = new StringBuilder();
                sb.Append("axis:\"x\",delay:300,containment:\"parent\",stop:function(event,ui){");
                sb.Append("var hid=$(\"#\"+$(this).attr(\"id\").replace(\"tabbar\",\"order\"));");
                sb.Append("var ord=\"\";");
                sb.Append("$(this).find(\"a\").each(function(){ord+=$(this).attr(\"oa\")+\";\";});");
                sb.Append("ord=ord.slice(0,-1);");
                sb.Append("hid.val(ord);");
                sb.Append("}");
                Script.AddStartupScript(this, string.Format("$(\"#{0}_tabbar\").sortable({{{1}}});", ClientID, sb.ToString()));
                sb.Clear();
                sb.Append(string.Join(";", SortOrder));
                ScriptManager.RegisterHiddenField(Page, ClientID + "_order", sb.ToString());
            }

            foreach (TabPage page in Pages)
                page.PreRender();
        }

        ///
        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            if (UsejQueryStyle || TabStyle == TabStyle.jQuery)
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-tabs ui-widget ui-widget-content ui-corner-all");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if (UsejQueryStyle || TabStyle == TabStyle.jQuery)
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all");
            else
            {
                if (!TabBarStyle.IsEmpty)
                    TabBarStyle.AddAttributesToRender(writer);
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_tabbar");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);

            for (int i = 0; i < _pages.Count; ++i)
            {
                int idx = SortOrder[i];
                Pages[idx].RenderButton(writer, idx);
            }

            writer.RenderEndTag(); // Tab Bar

            if (!UsejQueryStyle && TabStyle != TabStyle.jQuery && !PageStyle.IsEmpty)
                PageStyle.AddAttributesToRender(writer);

            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_TabPages");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            for (int i = 0; i < Pages.Count; ++i)
                    Pages[i].RenderPage(writer, i);

            writer.RenderEndTag(); // The end of the page collection

            writer.RenderEndTag(); // overall control 
        }

        #endregion

        #region Internal

        internal TabGfx _leftGfxAct;
        internal TabGfx _rightGfxAct;
        internal TabGfx _midGfxAct;

        internal TabGfx _leftGfxInact;
        internal TabGfx _rightGfxInact;
        internal TabGfx _midGfxInact;

        #endregion

        #region ViewState

        ///
        protected override void TrackViewState()
        {
            base.TrackViewState();
            if (_pages != null)
                _pages.TrackViewState();
            if (_tabBarStyle != null)
                ((IStateManager)_tabBarStyle).TrackViewState();
            if (_activeTabStyle != null)
                ((IStateManager)_activeTabStyle).TrackViewState();
            if (_inactiveTabStyle != null)
                ((IStateManager)_inactiveTabStyle).TrackViewState();
            if (_pageStyle != null)
                ((IStateManager)_pageStyle).TrackViewState();
        }

        ///
        protected override void LoadViewState(object savedState)
        {
            object[] states = (object[])savedState;
            if (states[0] != null) base.LoadViewState(states[0]);
            if (states[1] != null) Pages.LoadViewState(states[1]);
            if (states[2] != null) ((IStateManager)TabBarStyle).LoadViewState(states[2]);
            if (states[3] != null) ((IStateManager)ActiveTabStyle).LoadViewState(states[3]);
            if (states[4] != null) ((IStateManager)InactiveTabStyle).LoadViewState(states[4]);
            if (states[5] != null) ((IStateManager)PageStyle).LoadViewState(states[5]);
        }

        ///
        protected override object SaveViewState()
        {
            if (_sortOrder != null)
                ViewState["SortOrder"] = _sortOrder;

            object[] states = new object[6];
            states[0] = base.SaveViewState();
            if (_pages != null) states[1] = _pages.SaveViewState();
            if (_tabBarStyle != null) states[2] = ((IStateManager)_tabBarStyle).SaveViewState();
            if (_activeTabStyle != null) states[3] = ((IStateManager)_activeTabStyle).SaveViewState();
            if (_inactiveTabStyle != null) states[4] = ((IStateManager)_inactiveTabStyle).SaveViewState();
            if (_pageStyle != null) states[5] = ((IStateManager)_pageStyle).SaveViewState();
            return states;
        }

        #endregion

        #region IPostBackEventHandler Members

        /// <summary>
        /// The event if the tab order changes
        /// </summary>
        public event EventHandler OrderChanged;
        /// <summary>
        /// The event handler if the active tab has changed
        /// </summary>
        public event EventHandler ActiveTabChanged;

        ///
        public void RaisePostBackEvent(string eventArgument)
        {
            int idx = 0;
            if (int.TryParse(eventArgument, out idx))
            {
                ActivePageIndex = idx;
                if (ActiveTabChanged != null)
                    ActiveTabChanged(this, new EventArgs());
            }
        }

        ///
        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            _activeTabChanged = _orderChanged = false;

            if (!string.IsNullOrEmpty(postCollection[ClientID + "_actIdx"]))
            {
                int idx = 0;
                if (int.TryParse(postCollection[ClientID + "_actIdx"], out idx))
                {
                    if (idx != ActivePageIndex)
                    {
                        ActivePageIndex = idx;
                        _activeTabChanged = true;
                    }
                }
            }

            if(!string.IsNullOrEmpty(postCollection[ClientID + "_order"]))
            {
                string newVals = postCollection[ClientID + "_order"].TrimEnd(';');
                string original = string.Join(";", SortOrder);
                if(newVals != original)
                {
                    try
                    {
                        List<int> newOrder = new List<int>();
                        string[] vals = newVals.Split(';');
                        for(int i = 0; i < Pages.Count; ++i)
                            newOrder.Add(int.Parse(vals[i]));

                        SortOrder = newOrder;
                        _orderChanged = true;
                    }
                    catch(Exception)
                    {
                        // Should I throw an exception?
                        // Something messed up on the client side
                        // Ignoring so as not to cause problems
                    }
                }
            }

            return (_activeTabChanged || _orderChanged);
        }

        ///
        public void RaisePostDataChangedEvent()
        {
            if (_activeTabChanged && ActiveTabChanged != null)
                ActiveTabChanged(this, new EventArgs());

            if (_orderChanged && OrderChanged != null)
                OrderChanged(this, new EventArgs());
        }

        #endregion

        #region Private

        private TabPageList _pages;
        private TableItemStyle _tabBarStyle;
        private TabButtonStyle _activeTabStyle;
        private TabButtonStyle _inactiveTabStyle;
        private TableItemStyle _pageStyle;
        private bool _orderChanged;
        private bool _activeTabChanged;
        private List<int> _sortOrder;

        #endregion

        #region TabGfx

        internal class TabGfx : Graphic.WebGfx
        {
            public TabGfx(string fileName, int size, bool right)
            {
                _gfxFile = fileName;
                _size = size;
                _right = right;

                Save();
            }

            public override System.Drawing.Image Image()
            {
                try
                {
                    Bitmap input = new Bitmap(_gfxFile);
                    Rectangle rect = new Rectangle(0, 0, _size, input.Height);
                    Bitmap output = new Bitmap(_size, input.Height);
                    using (Graphics gfx = Graphics.FromImage(output))
                    {
                        gfx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                        gfx.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;

                        if (_size == 1)
                            gfx.DrawImage(input, rect, input.Width / 2, 0, _size, input.Height, GraphicsUnit.Pixel);
                        else
                        {
                            if (_right)
                                gfx.DrawImage(input, rect, input.Width - _size, 0, _size, input.Height, GraphicsUnit.Pixel);
                            else
                                gfx.DrawImage(input, rect, 0, 0, _size, input.Height, GraphicsUnit.Pixel);
                        }
                    }

                    return output;
                }
                catch (Exception)
                { /* Nothing we can do at this point */ }

                return null;
            }

            public int ImageSize
            {
                get { return _size; }
            }

            public override ImageFormat ImageFormat
            {
                get { return ImageFormat.Png; }
                set
                { }
            }

            private string _gfxFile;
            private int _size;
            private bool _right;
        }

        #endregion
    }
}
