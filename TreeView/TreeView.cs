using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// TreeView control, clean rendering, and works with AJAX
    /// </summary>
    /// <remarks></remarks>
    [ToolboxData("<{0}:TreeView runat=\"server\"></{0}:TreeView>")]
    public class TreeView : WebControl, IPostBackEventHandler
    {
        #region Properties

        #region Data

        /// <summary>
        /// Gets a list of nodes that represent the root nodes of the tree
        /// </summary>
        [DefaultValue(typeof(TreeNodeList), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public TreeNodeList Nodes
        {
            get { return RootNode.ChildNodes; }
            set { RootNode.ChildNodes = value; }
        }

        #endregion

        #region Behaviour

        /// <summary>
        /// The type of selection allowed on the tree
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(SelectMode.None)]
        public SelectMode SelectMode
        {
            get
            {
                if(ViewState["SelectMode"] == null)
                    return SelectMode.None;
                else
                    return (SelectMode)ViewState["SelectMode"];
            }
            set { ViewState["SelectMode"] = value; }
        }

        /// <summary>
        /// The client side events for the control
        /// </summary>
        [Bindable(true), Category("behaviour"), DefaultValue(typeof(TreeClientEvents), null), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public TreeClientEvents ClientSideEvents
        {
            get
            {
                if(_clientEvents == null)
                {
                    _clientEvents = new TreeClientEvents();
                    if(IsTrackingViewState) 
                        ((IStateManager)_clientEvents).TrackViewState();
                }

                return _clientEvents;
            }
        }

        #endregion

        #region Appearance

        /// <summary>
        /// A user defined path to the graphics to use instead of the default images
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(typeof(string), null), UrlProperty()]
        public string GfxsPath
        {
            get
            {
                if(ViewState["GfxsPath"] == null)
                    return null;
                else
                    return (string)ViewState["GfxsPath"];
            }
            set { ViewState["GfxsPath"] = value; }
        }

        /// <summary>
        /// Whether to allow the wrapping of text in the nodes
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(true)]
        public bool AllowTextWrap
        {
            get
            {
                if(ViewState["AllowTextWrap"] == null)
                    return true;
                else
                    return (bool)ViewState["AllowTextWrap"];
            }
            set { ViewState["AllowTextWrap"] = value; }
        }

        /// <summary>
        /// The style to make the tree
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(false)]
        public bool ShowLines
        {
            get
            {
                if(ViewState["ShowLines"] == null)
                    return false;
                else
                    return (bool)ViewState["ShowLines"];
            }
            set { ViewState["ShowLines"] = value; }
        }

        /// <summary>
        /// The style for normal nodes
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(typeof(AdvStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public AdvStyle NodeStyle
        {
            get
            {
                if(_nodeStyle == null)
                {
                    _nodeStyle = new AdvStyle();
                    if(IsTrackingViewState)
                        ((IStateManager)_nodeStyle).TrackViewState();
                }
                return _nodeStyle;
            }
        }

        /// <summary>
        /// The style for selected nodes
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(typeof(AdvStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public AdvStyle SelectedNodeStyle
        {
            get
            {
                if(_selectStyle == null)
                {
                    _selectStyle = new AdvStyle();
                    if(IsTrackingViewState)
                        ((IStateManager)_selectStyle).TrackViewState();
                }
                return _selectStyle;
            }
        }

        /// <summary>
        /// The style for when hovering over a node
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(typeof(AdvStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public AdvStyle HoverNodeStyle
        {
            get
            {
                if(_hoverStyle == null)
                {
                    _hoverStyle = new AdvStyle();
                    if(IsTrackingViewState)
                        ((IStateManager)_hoverStyle).TrackViewState();
                }
                return _hoverStyle;
            }
        }

        #endregion

        #endregion

        #region Events

        ///
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            Nodes.OnLoad(this);
            if(SelectMode == SelectMode.Single)
            {
                Nodes.ClearSelected();
                var selected = Page.Request.Params[ClientID + "_selected"];
                if(!string.IsNullOrEmpty(selected))
                    Nodes.NodeByInternalID(selected).Selected = true;
            }
        }

        ///
        protected override void OnPreRender(System.EventArgs e)
        {
            Util.RegisterStyle(this, NodeStyle, "NodeStyle");
            Util.RegisterStyle(this, HoverNodeStyle, "HoverStyle");
            Util.RegisterStyle(this, SelectedNodeStyle, "SelectedStyle");

            _bgdStyle = new AdvStyle();
            _bgdStyle.BackgroundImagePosition = BackgroundPosition.LeftCenter;
            _bgdStyle.BackgroundImageRepeat = BackgroundRepeat.RepeatY;
            Page.Header.StyleSheet.RegisterStyle(_bgdStyle, this);

            _bgdPadStyle = new AdvStyle(this.ViewState);
            _bgdPadStyle.PaddingLeft = Unit.Pixel(16);
            _bgdPadStyle.BackgroundImagePosition = BackgroundPosition.LeftCenter;
            _bgdPadStyle.BackgroundImageRepeat = BackgroundRepeat.RepeatY;
            Page.Header.StyleSheet.RegisterStyle(_bgdPadStyle, this);

            Script.AddResourceScript(Page, "TreeView.js");
            ScriptManager.RegisterExpandoAttribute(this, ClientID, "CBoxImg", Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.tree.cbox.gif"), true);
            ScriptManager.RegisterExpandoAttribute(this, ClientID, "EBoxImg", Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.tree.ebox.gif"), true);
            ScriptManager.RegisterExpandoAttribute(this, ClientID, "SelectMode", SelectMode.ToString(), false);

            if(SelectMode != SelectMode.None)
            {

                if(SelectMode == SelectMode.Single | SelectMode == SelectMode.SinglePostback)
                {
                    if(Nodes.Selected().Count > 0)
                        ScriptManager.RegisterHiddenField(Page, ClientID + "_selected", Nodes.Selected()[0].InternalID);
                    else
                        ScriptManager.RegisterHiddenField(Page, ClientID + "_selected", "");
                }
            }

            if(_clientEvents != null) _clientEvents.OnPreRender(this);
            Nodes.OnPreRender(this);

            base.OnPreRender(e);
        }

        ///
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "left");
            if(!AllowTextWrap) writer.AddStyleAttribute(HtmlTextWriterStyle.WhiteSpace, "nowrap");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            Nodes.Render(this, writer);

            writer.RenderEndTag();
        }

        #endregion

        #region Postback

        /// <summary>
        /// The event handler for tree node events
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The tree node event arguments</param>
        public delegate void TreeNodeEventHandler(object sender, TreeNodeEventArgs e);

        /// <summary>
        /// A node has been expanded or collapsed
        /// </summary>
        public event TreeNodeEventHandler ExpandCollape;

        /// <summary>
        /// A node with command arguments has been clicked
        /// </summary>
        public event TreeNodeEventHandler Command;

        /// <summary>
        /// A node has been clicked
        /// </summary>
        public event TreeNodeEventHandler Clicked;

        /// <summary>
        /// Raises a post back event for the tree
        /// </summary>
        /// <param name="eventArgument">The string containing the post back event data</param>
        public void RaisePostBackEvent(string eventArgument)
        {
            string[] args = eventArgument.Split(":".ToCharArray());

            TreeNode node = Nodes.NodeByInternalID(args[1]);
            if(node != null)
            {
                switch(args[0])
                {
                    case "expcol":
                        node.Expanded = !node.Expanded;
                        if(ExpandCollape != null)
                            ExpandCollape(this, new TreeNodeEventArgs(node));

                        break;
                    case "clicked":
                        if(!string.IsNullOrEmpty(node.CommandName))
                        {
                            if(Command != null)
                                Command(this, new TreeNodeEventArgs(node));
                        }
                        else
                        {
                            if(SelectMode != SelectMode.None)
                            {
                                if(SelectMode == SelectMode.Single | SelectMode == SelectMode.SinglePostback)
                                {
                                    if(node.Selected)
                                        node.Selected = false;
                                    else
                                        Nodes.ClearSelected();
                                        node.Selected = true;
                                }
                                else
                                    node.Selected = !node.Selected;
                            }
                        }

                        if(Clicked != null)
                            Clicked(this, new TreeNodeEventArgs(node));

                        break;
                }
            }
        }

        #endregion

        #region ViewState

        ///
        protected override void LoadViewState(object savedState)
        {
            object[] state = (object[])savedState;
            if(state != null)
            {
                if(state[0] != null) 
                    base.LoadViewState(state[0]);

                Nodes.Clear();
                if(state[1] != null) 
                    ((IStateManager)Nodes).LoadViewState(state[1]);

                if(state[2] != null) 
                    ((IStateManager)ClientSideEvents).LoadViewState(state[2]);

                if(state[3] != null) 
                    ((IStateManager)NodeStyle).LoadViewState(state[3]);

                if(state[4] != null) 
                    ((IStateManager)SelectedNodeStyle).LoadViewState(state[4]);

                if(state[5] != null) 
                    ((IStateManager)HoverNodeStyle).LoadViewState(state[5]);

            }
        }

        /// 
        protected override object SaveViewState()
        {
            object[] state = new object[6];
            state[0] = base.SaveViewState();
            state[1] = ((IStateManager)Nodes).SaveViewState();
            if(_clientEvents != null) 
                state[2] = ((IStateManager)_clientEvents).SaveViewState();

            if(_nodeStyle != null) 
                state[3] = ((IStateManager)_nodeStyle).SaveViewState();

            if(_selectStyle != null)
                state[4] = ((IStateManager)_selectStyle).SaveViewState();

            if(_hoverStyle != null) 
                state[5] = ((IStateManager)_hoverStyle).SaveViewState();

            if(state[0] == null && state[1] == null)
                return null;
            else
                return state;
        }

        ///
        protected override void TrackViewState()
        {
            base.TrackViewState();
            if(_clientEvents != null) 
                ((IStateManager)_clientEvents).TrackViewState();

            if(_nodeStyle != null) 
                ((IStateManager)_nodeStyle).TrackViewState();

            if(_selectStyle != null)
                ((IStateManager)_selectStyle).TrackViewState();

            if(_hoverStyle != null)
                ((IStateManager)_hoverStyle).TrackViewState();

            ((IStateManager)Nodes).TrackViewState();
        }

        #endregion

        #region Internal Functions

        /// <summary>
        /// The background style for setting lines image
        /// </summary>
        internal AdvStyle BackgroundStyle
        {
            get { return _bgdStyle; }
        }

        /// <summary>
        /// The background style with padding to use
        /// </summary>
        internal AdvStyle BackgroundPaddedStyle
        {
            get { return _bgdPadStyle; }
        }

        internal new bool IsEnabled
        {
            get { return base.IsEnabled; }
        }

        #endregion

        #region Private

        private TreeNode RootNode
        {
            get
            {
                if(_data == null)
                    _data = new TreeNode();

                return _data;
            }
        }

        TreeNode _data;
        TreeClientEvents _clientEvents;
        AdvStyle _nodeStyle;
        AdvStyle _selectStyle;
        AdvStyle _hoverStyle;
        AdvStyle _bgdStyle;
        AdvStyle _bgdPadStyle;

        #endregion

    }

    /// <summary>
    /// The client events for the tree
    /// </summary>
    [ParseChildren(true)]
    public class TreeClientEvents : ViewStateBase
    {

        /// <summary>
        /// The event to fire before a click has occured
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string BeforeClick
        {
            get
            {
                if(ViewState["BeforeClick"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["BeforeClick"];
            }
            set { ViewState["BeforeClick"] = value; }
        }

        /// <summary>
        /// The event to fire after a click has occured
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string AfterClick
        {
            get
            {
                if(ViewState["AfterClick"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["AfterClick"];
            }
            set { ViewState["AfterClick"] = value; }
        }

        /// <summary>
        /// The event to fire before a branch is expanded or collapsed
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string BeforeExpandCollapse
        {
            get
            {
                if(ViewState["BeforeExpandCollapse"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["BeforeExpandCollapse"];
            }
            set { ViewState["BeforeExpandCollapse"] = value; }
        }

        /// <summary>
        /// The event to fire after a branch is expanded or collapsed
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string AfterExpandCollapse
        {
            get
            {
                if(ViewState["AfterExpandCollapse"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["AfterExpandCollapse"];
            }
            set { ViewState["AfterExpandCollapse"] = value; }
        }

        #region Internal

        internal void OnPreRender(TreeView tree)
        {
            if(!string.IsNullOrEmpty(BeforeClick)) 
                ScriptManager.RegisterExpandoAttribute(tree, tree.ClientID, "ClientBeforeClick", BeforeClick, false);

            if(!string.IsNullOrEmpty(AfterClick)) 
                ScriptManager.RegisterExpandoAttribute(tree, tree.ClientID, "ClientAfterClick", AfterClick, false);

            if(!string.IsNullOrEmpty(BeforeExpandCollapse)) 
                ScriptManager.RegisterExpandoAttribute(tree, tree.ClientID, "ClientBeforeExpandCollapse", BeforeExpandCollapse, false);

            if(!string.IsNullOrEmpty(AfterExpandCollapse)) 
                ScriptManager.RegisterExpandoAttribute(tree, tree.ClientID, "ClientAfterExpandCollapse", AfterExpandCollapse, false);
        }

        #endregion

    }

}