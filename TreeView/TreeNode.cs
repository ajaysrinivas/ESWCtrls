using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// The nodes that belong in a tree view
    /// </summary>
    [ParseChildren(true, "ChildNodes")]
    public class TreeNode : ViewStateBase, ICloneable
    {

        #region Public Properties

        /// <summary>
        /// The ID of the node
        /// </summary>
        [DefaultValue("")]
        public string ID
        {
            get
            {
                if(ViewState["ID"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["ID"];
            }
            set { ViewState["ID"] = value; }
        }

        /// <summary>
        /// The text to display for the node
        /// </summary>
        [DefaultValue("")]
        public string Text
        {
            get
            {
                if(ViewState["Text"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Text"];
            }
            set { ViewState["Text"] = value; }
        }

        /// <summary>
        /// The tooltip to display for the node
        /// </summary>
        [DefaultValue("")]
        public string ToolTip
        {
            get
            {
                if(ViewState["ToolTip"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["ToolTip"];
            }
            set { ViewState["ToolTip"] = value; }
        }

        /// <summary>
        /// The icon for the node
        /// </summary>
        [DefaultValue(""), UrlProperty(), Editor("System.Web.UI.Design.ImageUrlEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor))]
        public string Icon
        {
            get
            {
                if(ViewState["Icon"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Icon"];
            }
            set { ViewState["Icon"] = value; }
        }

        /// <summary>
        /// A url link to  redirect to when the node is clicked
        /// </summary>
        [DefaultValue(""), UrlProperty()]
        public string Link
        {
            get
            {
                if(ViewState["Link"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Link"];
            }
            set { ViewState["Link"] = value; }
        }

        /// <summary>
        /// Command name to pass when the node is clicked
        /// </summary>
        [DefaultValue("")]
        public string CommandName
        {
            get
            {
                if(ViewState["CommandName"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["CommandName"];
            }
            set { ViewState["CommandName"] = value; }
        }

        /// <summary>
        /// Command argument to pass when the node is clicked
        /// </summary>
        [DefaultValue(typeof(object), null)]
        public object CommandArgument
        {
            get { return ViewState["CommandArgument"]; }
            set { ViewState["CommandArgument"] = value; }
        }

        /// <summary>
        /// Whether the node is expanded to show children
        /// </summary>
        [DefaultValue(false)]
        public bool Expanded
        {
            get
            {
                if(ViewState["Expanded"] == null)
                    return false;
                else
                    return (bool)ViewState["Expanded"];
            }
            set { ViewState["Expanded"] = value; }
        }

        /// <summary>
        /// Whether the node is selected
        /// </summary>
        [DefaultValue(false)]
        public bool Selected
        {
            get
            {
                if(ViewState["Selected"] == null)
                    return false;
                else
                    return (bool)ViewState["Selected"];
            }
            set { ViewState["Selected"] = value; }
        }

        /// <summary>
        /// Whether the node is clickable
        /// </summary>
        [DefaultValue(true)]
        public bool Clickable
        {
            get
            {
                if(ViewState["Clickable"] == null)
                    return true;
                else
                    return (bool)ViewState["Clickable"];
            }
            set { ViewState["Clickable"] = value; }
        }

        /// <summary>
        /// The style for the node
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(AdvStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public AdvStyle Style
        {
            get
            {
                if(_style == null)
                {
                    _style = new AdvStyle();
                    if(IsTrackingViewState)
                        ((IStateManager)_style).TrackViewState();
                }

                return _style;
            }
        }

        /// <summary>
        /// The selected style for the node
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(AdvStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public AdvStyle SelectedStyle
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
        /// The hover style for the node
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(AdvStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public AdvStyle HoverStyle
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

        /// <summary>
        /// The child nodes of this node
        /// </summary>
        [Browsable(false), DefaultValue(typeof(TreeNodeList), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        public TreeNodeList ChildNodes
        {
            get
            {
                if(_childNodes == null)
                    _childNodes = new TreeNodeList(this);

                return _childNodes;
            }
            internal set
            {
                _childNodes = value;
                _childNodes.SetParent(this);
            }
        }

        /// <summary>
        /// The parent of the node
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TreeNode Parent
        {
            get { return _parent; }
            internal set
            {
                _parent = value;
                if(_childNodes != null)
                    _childNodes.SetParent(this);
            }
        }

        #endregion

        #region Contructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TreeNode()
        {
            _parent = null;
        }

        /// <summary>
        /// Creates a new item with a specific text
        /// </summary>
        /// <param name="Text">The text for the item</param>
        public TreeNode(string Text)
            : this()
        {
            this.Text = Text;
        }

        /// <summary>
        /// Creates a new item with an ID, and text
        /// </summary>
        /// <param name="ID">The items identifying string</param>
        /// <param name="Text">The text for the item</param>
        public TreeNode(string ID, string Text)
            : this()
        {
            this.ID = ID;
            this.Text = Text;
        }

        /// <summary>
        /// Creates a new item with a specific text and icon
        /// </summary>
        /// <param name="ID">The items identifying string</param>
        /// <param name="Text">The text for the item</param>
        /// <param name="Icon">The path to the icon</param>
        public TreeNode(string ID, string Text, string Icon)
            : this()
        {
            this.ID = ID;
            this.Text = Text;
            this.Icon = Icon;
        }

        /// <summary>
        /// Creates an item with ID, text, and command variables
        /// </summary>
        /// <param name="ID">The items identifying string</param>
        /// <param name="Text">The text for the item</param>
        /// <param name="CommandName">The command name for the item</param>
        /// <param name="CommandArgument">The command argument for the item</param>
        public TreeNode(string ID, string Text, string CommandName, object CommandArgument)
            : this()
        {
            this.ID = ID;
            this.Text = Text;
            this.CommandName = CommandName;
            this.CommandArgument = CommandArgument;
        }

        /// <summary>
        /// Creates a item with text, icon, and command variables set
        /// </summary>
        /// <param name="ID">The items identifying string</param>
        /// <param name="Text">The text for the item</param>
        /// <param name="Icon">The path to the icon</param>
        /// <param name="CommandName">The command name for the item</param>
        /// <param name="CommandArgument">The command argument for the item</param>
        public TreeNode(string ID, string Text, string Icon, string CommandName, object CommandArgument)
            : this()
        {
            this.ID = ID;
            this.Text = Text;
            this.Icon = Icon;
            this.CommandName = CommandName;
            this.CommandArgument = CommandArgument;
        }

        /// <summary>
        /// Creates an item with the details specified
        /// </summary>
        /// <param name="ID">The items identifying string</param>
        /// <param name="Text">The text for the item</param>
        /// <param name="Icon">The path to the icon</param>
        /// <param name="Link">The link to use for the item</param>
        /// <param name="CommandName">The command name for the item</param>
        /// <param name="CommandArgument">The command argument for the item</param>
        /// <param name="Expanded">Whether the node is expanded</param>
        /// <param name="Selected">Whether the item is selected</param>
        public TreeNode(string ID, string Text, string Icon, string Link, string CommandName, object CommandArgument, bool Expanded, bool Selected)
        {
            this.ID = ID;
            this.Text = Text;
            this.Icon = Icon;
            this.Link = Link;
            this.CommandName = CommandName;
            this.CommandArgument = CommandArgument;
            this.Expanded = Expanded;
            this.Selected = Selected;
            _childNodes = new TreeNodeList(this);
        }

        #endregion

        #region Internal

        /// <summary>
        /// returns an item that contains the child specified
        /// </summary>
        /// <param name="item">The item to get the parent for</param>
        /// <returns>TreeItem or nothing</returns>
        internal TreeNode GetParent(TreeNode item)
        {
            foreach(TreeNode child in ChildNodes)
            {
                if(child.Equals(item))
                    return this;
                else
                {
                    TreeNode rst = child.GetParent(item);
                    if(rst != null) return rst;
                }
            }

            return null;
        }

        /// <summary>
        /// The internal ID for the node within the full tree
        /// </summary>
        internal string InternalID
        {
            get
            {
                if(_parent == null)
                    return string.Empty;
                else
                {
                    if(string.IsNullOrEmpty(_parent.InternalID))
                        return _parent.ChildNodes.IndexOf(this).ToString();
                    else
                        return _parent.InternalID + "_" + _parent.ChildNodes.IndexOf(this).ToString();
                }
            }
        }

        /// <summary>
        /// If the node is dirty
        /// </summary>
        internal void SetDirty()
        {
            ViewState.SetDirty(true);
            if(_style != null) _style.SetDirty();
            if(_selectStyle != null) _style.SetDirty();
            if(_hoverStyle != null) _style.SetDirty();
            if(_childNodes != null) ChildNodes.SetDirty();
        }

        /// <summary>
        /// Processing after viewstate, to get details when javascript has been used
        /// </summary>
        /// <param name="tree">The tree the node belongs to</param>
        internal void OnLoad(TreeView tree)
        {
            bool temp;
            if(!string.IsNullOrEmpty(tree.Page.Request[tree.ClientID + "_" + InternalID + "_expanded"]))
            {
                if(bool.TryParse(tree.Page.Request[tree.ClientID + "_" + InternalID + "_expanded"], out temp))
                    Expanded = temp;
            }

            if(tree.SelectMode == ESWCtrls.SelectMode.Multiple)
            {
                if(bool.TryParse(tree.Page.Request[tree.ClientID + "_" + InternalID + "_select"], out temp))
                    Selected = temp;
            }

            ChildNodes.OnLoad(tree);
        }

        /// <summary>
        /// Registers the additional style for the node
        /// </summary>
        /// <param name="treeview">The tree the node belongs to</param>
        internal void OnPreRender(TreeView treeview)
        {
            Util.RegisterStyle(treeview,Style, "NodeStyle_" + InternalID);
            Util.RegisterStyle(treeview,SelectedStyle, "SelectedStyle_" + InternalID);
            if(treeview.Enabled)
                Util.RegisterStyle(treeview,HoverStyle, "HoverStyle_" + InternalID);

            ChildNodes.OnPreRender(treeview);
        }

        /// <summary>
        /// Does the rendering of the node
        /// </summary>
        /// <param name="tree">The tree the node belongs to</param>
        /// <param name="writer">The writer to write to</param>
        /// <param name="lastChild">If this node is the last one of its siblings</param>
        /// <param name="firstNode">If this is the very first node in the tree</param>
        internal void Render(TreeView tree, HtmlTextWriter writer, bool lastChild, bool firstNode)
        {
            if(tree.SelectMode == SelectMode.Multiple) ScriptManager.RegisterHiddenField(tree.Page, tree.ClientID + "_" + InternalID + "_select", Selected.ToString());
            if(tree.ShowLines)
            {
                if(lastChild)
                {
                    if(firstNode)
                        writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url('" + tree.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.tree.sline.gif") + "')");
                    else
                        writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url('" + tree.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.tree.bline.gif") + "')");
                }
                else
                {
                    if(firstNode)
                        writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url('" + tree.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.tree.tline.gif") + "')");
                    else
                        writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url('" + tree.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.tree.mline.gif") + "')");
                }
            }

            if(ChildNodes.Count > 0)
            {
                ScriptManager.RegisterHiddenField(tree.Page, tree.ClientID + "_" + InternalID + "_expanded", Expanded.ToString());
                tree.BackgroundStyle.AddAttributesToRender(writer);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                if(tree.IsEnabled)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, tree.Page.ClientScript.GetPostBackClientHyperlink(tree, "expcol:" + InternalID));
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "return ESW_Tree_expcolBranch( " + tree.ClientID + ", '" + InternalID + "' );");
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                }
                else
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);


                writer.AddAttribute(HtmlTextWriterAttribute.Id, tree.ClientID + "_" + InternalID + "_ecImg");
                writer.AddStyleAttribute(HtmlTextWriterStyle.VerticalAlign, "middle");
                if(Expanded)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, tree.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.tree.ebox.gif"));
                    writer.AddAttribute(HtmlTextWriterAttribute.Alt, "Collapse");
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, tree.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.tree.cbox.gif"));
                    writer.AddAttribute(HtmlTextWriterAttribute.Alt, "Expand");
                }

                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();

                writer.RenderEndTag();
            }
            else
            {
                if(firstNode && lastChild)
                {
                    tree.BackgroundStyle.AddAttributesToRender(writer);
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, tree.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.tree.box.gif"));
                    writer.AddAttribute(HtmlTextWriterAttribute.Alt, "Only Node");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.VerticalAlign, "middle");
                    writer.RenderBeginTag(HtmlTextWriterTag.Img);
                    writer.RenderEndTag();
                }
                else
                {
                    tree.BackgroundPaddedStyle.AddAttributesToRender(writer);
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                }
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Id, tree.ClientID + "_" + InternalID);
            if(!string.IsNullOrEmpty(ID))
                writer.AddAttribute("extid", ID);
            string cssClass = tree.NodeStyle.RenderClass;
            if(!Style.IsEmpty) cssClass += " " + Style.RenderClass;
            if(Selected)
            {
                if(!tree.SelectedNodeStyle.IsEmpty)
                    cssClass += " " + tree.SelectedNodeStyle.RenderClass;

                if(!SelectedStyle.IsEmpty)
                    cssClass += " " + SelectedStyle.RenderClass;
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass.Trim());

            if(tree.IsEnabled)
            {
                if(!tree.HoverNodeStyle.IsEmpty && (Clickable || !string.IsNullOrEmpty(Link)))
                {
                    writer.AddAttribute("onmouseover", "ESW_Tree_mouseOver( " + tree.ClientID + ", '" + InternalID + "' );");
                    writer.AddAttribute("onmouseout", "ESW_Tree_mouseOut( " + tree.ClientID + ", '" + InternalID + "' );");
                }

                if(string.IsNullOrEmpty(ToolTip))
                    writer.AddAttribute(HtmlTextWriterAttribute.Title, Text);
                else
                    writer.AddAttribute(HtmlTextWriterAttribute.Title, ToolTip);

                if(!string.IsNullOrEmpty(Link))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, tree.ResolveUrl(Link));
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                }
                else if(Clickable)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, tree.Page.ClientScript.GetPostBackClientHyperlink(tree, "clicked:" + InternalID));
                    if(string.IsNullOrEmpty(CommandName))
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "return ESW_Tree_clickNode( " + tree.ClientID + ", '" + InternalID + "' );");

                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                }
                else
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                }
            }
            else
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
            }

            if(!string.IsNullOrEmpty(Icon))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, tree.ResolveUrl(Icon));
                writer.AddAttribute(HtmlTextWriterAttribute.Alt, Text);
                writer.AddStyleAttribute(HtmlTextWriterStyle.VerticalAlign, "middle");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
            }
            writer.Write(Text);

            writer.RenderEndTag();

            writer.RenderEndTag();

            if(ChildNodes.Count > 0)
            {
                if(!Expanded & !tree.IsEnabled)
                    return;

                tree.BackgroundPaddedStyle.AddAttributesToRender(writer);
                writer.AddAttribute(HtmlTextWriterAttribute.Id, tree.ClientID + "_" + InternalID + "_children");

                if(!Expanded)
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");

                if(tree.ShowLines && !lastChild)
                    writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url('" + tree.Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Gfxs.tree.vline.gif") + "')");

                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                ChildNodes.Render(tree, writer);
                writer.RenderEndTag();
            }
        }

        #endregion

        #region Private

        private TreeNode _parent;
        private TreeNodeList _childNodes;
        private AdvStyle _style;
        private AdvStyle _selectStyle;
        private AdvStyle _hoverStyle;

        #endregion

        #region ViewState

        ///
        protected internal override void TrackViewState()
        {
            base.TrackViewState();

            if(_style != null)
                ((IStateManager)_style).TrackViewState();

            if(_selectStyle != null)
                ((IStateManager)_selectStyle).TrackViewState();

            if(_hoverStyle != null)
                ((IStateManager)_hoverStyle).TrackViewState();

            if(_childNodes != null)
                ((IStateManager)_childNodes).TrackViewState();
        }

        ///
        protected internal override void LoadViewState(object state)
        {
            object[] nodeState = (object[])state;

            if(nodeState != null)
            {
                if(nodeState[0] != null)
                    base.LoadViewState(nodeState[0]);

                if(nodeState[1] != null)
                    ((IStateManager)Style).LoadViewState(nodeState[1]);

                if(nodeState[2] != null)
                    ((IStateManager)SelectedStyle).LoadViewState(nodeState[1]);

                if(nodeState[3] != null)
                    ((IStateManager)HoverStyle).LoadViewState(nodeState[1]);

                if(nodeState[4] != null)
                    ((IStateManager)ChildNodes).LoadViewState(nodeState[4]);

            }
        }

        ///
        protected internal override object SaveViewState()
        {
            object[] state = new object[5];

            state[0] = base.SaveViewState();

            if(_style != null)
                state[1] = ((IStateManager)_style).SaveViewState();

            if(_selectStyle != null)
                state[2] = ((IStateManager)_selectStyle).SaveViewState();

            if(_hoverStyle != null)
                state[3] = ((IStateManager)_hoverStyle).SaveViewState();

            if(_childNodes != null)
                state[4] = ((IStateManager)_childNodes).SaveViewState();

            if(state[0] == null && state[1] == null && state[2] == null && state[3] == null && state[4] == null)
                return null;

            return state;
        }

        #endregion

        #region ICloneable

        /// <summary>
        /// Clones the node
        /// </summary>
        public object Clone()
        {
            TreeNode node = new TreeNode();
            node.ID = ID;
            node.Text = Text;
            node.ToolTip = ToolTip;
            node.Icon = Icon;
            node.Link = Link;
            node.CommandName = CommandName;
            node.CommandArgument = CommandArgument;
            node.Expanded = Expanded;
            node.Selected = Selected;
            node._style = Style;
            node._selectStyle = Style;
            node._hoverStyle = Style;

            if(IsTrackingViewState)
                node.TrackViewState();

            return node;
        }

        #endregion

    }

    /// <summary>
    /// A list of tree nodes
    /// </summary>
    public class TreeNodeList : ViewStateListBase<TreeNode>
    {


        #region Constructor

        /// <summary>
        /// Creates an empty list
        /// </summary>
        public TreeNodeList()
        {
        }

        /// <summary>
        /// Creates a new list for the parent node
        /// </summary>
        /// <param name="node">Parent Node</param>
        public TreeNodeList(TreeNode node)
        {
            _parent = node;
        }

        /// <summary>
        /// Copies a nodelist
        /// </summary>
        /// <param name="nodes">The node list to copy</param>
        public TreeNodeList(TreeNodeList nodes)
        {
            _parent = null;
            if(nodes.IsTrackingViewState) TrackViewState();
            foreach(TreeNode node in nodes)
            {
                TreeNode child = (TreeNode)node.Clone();
                this.Add(child);
                if(node.ChildNodes.Count > 0)
                    child.ChildNodes = new TreeNodeList(node.ChildNodes);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns a node from the tree below this point that matches the ID supplied
        /// </summary>
        /// <param name="ID">The ID to get</param>
        public TreeNode this[string ID]
        {
            get
            {
                foreach(TreeNode node in this)
                {
                    if(node.ID == ID)
                        return node;
                    else
                    {
                        TreeNode child = node.ChildNodes[ID];
                        if(child != null) return child;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the last node in the list
        /// </summary>
        public TreeNode Last
        {
            get { return this[this.Count - 1]; }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Adds a node to the list
        /// </summary>
        /// <param name="node">The node to add</param>
        public override void Add(TreeNode node)
        {
            base.Add(node);
            if(_parent != null)
                node.Parent = _parent;

            if(IsTrackingViewState)
                ((IStateManager)node).TrackViewState();

            node.SetDirty();
        }

        /// <summary>
        /// Removes a node from the list
        /// </summary>
        /// <param name="node">The node to remove</param>
        public new void Remove(TreeNode node)
        {
            if(this.Contains(node))
            {
                base.Remove(node);
                return;
            }
            else
            {
                foreach(TreeNode child in this)
                    child.ChildNodes.Remove(node);
            }
        }

        /// <summary>
        /// Returns a list of selected nodes
        /// </summary>
        /// <returns>TreeNodeList</returns>
        public TreeNodeList Selected()
        {
            TreeNodeList list = new TreeNodeList();
            foreach(TreeNode node in this)
            {
                if(node.Selected)
                    list.Add(node);

                list.AddRange(node.ChildNodes.Selected());
            }

            return list;
        }

        /// <summary>
        /// Resets all nodes to be unselected
        /// </summary>
        public void ClearSelected()
        {
            foreach(TreeNode node in this)
            {
                node.Selected = false;
                node.ChildNodes.ClearSelected();
            }
        }

        /// <summary>
        /// Closes all the nodes in the tree
        /// </summary>
        public void CloseBranches()
        {
            foreach(TreeNode node in this)
            {
                node.Expanded = false;
                node.ChildNodes.CloseBranches();
            }
        }

        /// <summary>
        /// Opens all the nodes above the one given
        /// </summary>
        /// <param name="ID">The ID of the node</param>
        public void ExpandToNode(string ID)
        {
            TreeNode node = this[ID];
            if(node != null)
            {
                while(node.Parent != null)
                {
                    node.Parent.Expanded = true;
                    node = node.Parent;
                }
            }
        }

        #endregion

        #region Internal

        /// <summary>
        /// Gets a node by its internal ID
        /// </summary>
        /// <param name="InternalID">The ID of the node to get</param>
        /// <returns>TreeNode</returns>
        internal TreeNode NodeByInternalID(string InternalID)
        {
            foreach(TreeNode node in this)
            {
                if(node.InternalID == InternalID)
                    return node;
                else
                {
                    TreeNode child = node.ChildNodes.NodeByInternalID(InternalID);
                    if(child != null) return child;
                }
            }

            return null;
        }

        /// <summary>
        /// Sets the parent node for this list
        /// </summary>
        /// <param name="parent">The node to be set as parent</param>
        /// <remarks></remarks>
        internal void SetParent(TreeNode parent)
        {
            _parent = parent;
            foreach(TreeNode node in this)
                node.Parent = parent;
        }

        /// <summary>
        /// Marks the viewstate as dirty for all childnodes
        /// </summary>
        internal void SetDirty()
        {
            foreach(TreeNode node in this)
                node.SetDirty();
        }

        /// <summary>
        /// Loads the altered state of the tree
        /// </summary>
        /// <param name="treeview">The tree this list belongs to</param>
        internal void OnLoad(TreeView treeview)
        {
            foreach(TreeNode node in this)
                node.OnLoad(treeview);
        }

        /// <summary>
        /// Preps child nodes for rendering
        /// </summary>
        /// <param name="treeview">The tree this list belongs to</param>
        internal void OnPreRender(TreeView treeview)
        {
            foreach(TreeNode node in this)
                node.OnPreRender(treeview);
        }

        /// <summary>
        /// Renders all the child nodes
        /// </summary>
        /// <param name="treeview">The tree the list belongs to</param>
        /// <param name="writer">The eriter to write to</param>
        internal void Render(TreeView treeview, HtmlTextWriter writer)
        {
            if(this.Count > 0)
            {
                bool first = this[0].InternalID == treeview.Nodes[0].InternalID;
                int lastIdx = this.Count - 1;
                for(int i = 0; i <= lastIdx; i++)
                {
                    this[i].Render(treeview, writer, lastIdx == i, first);
                    first = false;
                }
            }
        }

        #endregion

        #region Private

        /// 
        protected internal override TreeNode Create(object state)
        {
            TreeNode node = new TreeNode();
            node.LoadViewState(state);
            return node;
        }

        private TreeNode _parent;

        #endregion
    }

    /// <summary>
    /// The Event Arguments for TreeView
    /// </summary>
    public class TreeNodeEventArgs : EventArgs
    {

        /// <summary>
        /// The node the event happened to
        /// </summary>
        public TreeNode Node
        {
            get { return _node; }
        }

        /// <summary>
        /// Creates a new TreeNodeEventArgs
        /// </summary>
        /// <param name="node">The node the event occured to</param>
        public TreeNodeEventArgs(TreeNode node)
        {
            _node = node;
        }

        private TreeNode _node;

    }

}