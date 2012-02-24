using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// Accordion style control
    /// Uses jQuery to do all the work
    /// </summary>
    [ToolboxData("<{0}:accordion runat=\"server\"></accordion>"),ParseChildren(true), PersistChildren(false)]
    public class Accordion : WebControl, IPostBackDataHandler
    {
        #region Properties

        #region Appearance

        /// <summary>
        /// The icon to use for unselected headers
        /// </summary>
        [Category("Appearance"), DefaultValue("ui-icon-triangle-1-e")]
        public string HeaderUnselectedIcon
        {
            get
            {
                if(ViewState["HeaderUnselectedIcon"] != null)
                    return (string)ViewState["HeaderUnselectedIcon"];
                else
                    return "ui-icon-triangle-1-e";
            }
            set { ViewState["HeaderUnselectedIcon"] = value; }
        }

        /// <summary>
        /// The icon to use for selected headers
        /// </summary>
        [Category("Appearance"), DefaultValue("ui-icon-triangle-1-s")]
        public string HeaderSelectedIcon
        {
            get
            {
                if(ViewState["HeaderSelectedIcon"] != null)
                    return (string)ViewState["HeaderSelectedIcon"];
                else
                    return "ui-icon-triangle-1-s";
            }
            set
            { ViewState["HeaderSelectedIcon"] = value; }
        }

        /// <summary>
        /// The animation effect to use
        /// </summary>
        [Category("Appearance"), DefaultValue(Effect.EffectType.Slide)]
        public Effect.EffectType Animation
        {
            get
            {
                if(ViewState["Animation"] != null)
                    return (Effect.EffectType)ViewState["Animation"];
                else
                    return Effect.EffectType.Slide;
            }
            set
            { ViewState["Animation"] = value; }
        }

        #endregion

        #region Behaviour

        /// <summary>
        /// If true highest content part is used as height reference for all other parts
        /// </summary>
        [Category("Behaviour"), DefaultValue(true)]
        public bool AutoHeight
        {
            get
            {
                if(ViewState["AutoHeight"] != null)
                    return (bool)ViewState["AutoHeight"];
                else
                    return true;
            }
            set { ViewState["AutoHeight"] = value; }
        }

        /// <summary>
        /// If set, clears height and overflow styles, after animation. Won't work with AutoHeight.
        /// </summary>
        [Category("Behaviour"), DefaultValue(false)]
        public bool ClearStyle
        {
            get
            {
                if(ViewState["ClearStyle"] != null)
                    return (bool)ViewState["ClearStyle"];
                else
                    return false;
            }
            set { ViewState["ClearStyle"] = value; }
        }

        /// <summary>
        /// Whether all sections can be closed at once
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
        /// If set the Accordion will fill the height of the parent element, overrides AutoHeight
        /// </summary>
        [Category("Behaviour"), DefaultValue(false)]
        public bool FillSpace
        {
            get
            {
                if(ViewState["FillSpace"] != null)
                    return (bool)ViewState["FillSpace"];
                else
                    return false;
            }
            set { ViewState["FillSpace"] = value; }
        }

        /// <summary>
        /// The event to use to activate the header (default "click")
        /// </summary>
        [Category("Behaviour"), DefaultValue("click")]
        public string Event
        {
            get
            {
                if(ViewState["Event"] != null)
                    return (string)ViewState["Event"];
                else
                    return "click";
            }
            set { ViewState["Event"] = value; }
        }

        /// <summary>
        /// Whether the Accordion is sortable
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

        /// <summary>
        /// The client side events
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(AccordionClientEvents), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public AccordionClientEvents ClientSideEvents
        {
            get
            {
                if(_clientEvents == null)
                {
                    _clientEvents = new AccordionClientEvents();
                    if(IsTrackingViewState) ((IStateManager)_clientEvents).TrackViewState();
                }
                return _clientEvents;
            }
        }

        /// <summary>
        /// Post back when the Active Section changed
        /// </summary>
        [Category("Behaviour"), DefaultValue(false)]
        public bool AutoPostBackActiveChanged
        {
            get
            {
                if(ViewState["APBActive"] != null)
                    return (bool)ViewState["APBActive"];
                else
                    return false;
            }
            set { ViewState["APBActive"] = value; }
        }

        /// <summary>
        /// Auto post back when the order of the sections change
        /// </summary>
        [Category("Behaviour"), DefaultValue(false)]
        public bool AutoPostBackOrderChanged
        {
            get
            {
                if(ViewState["APBOrder"] != null)
                    return (bool)ViewState["APBOrder"];
                else
                    return false;
            }
            set { ViewState["APBOrder"] = value; }
        }

        #endregion

        #region Data

        /// <summary>
        /// The Accordion sections
        /// </summary>
        [Category("Data"), PersistenceMode(PersistenceMode.InnerProperty), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), MergableProperty(false)]
        public AccordionSectionList Sections
        {
            get
            {
                if(_sections == null)
                    _sections = new AccordionSectionList(this);
                return _sections;
            }
        }

        /// <summary>
        /// The active sections index
        /// </summary>
        [Category("Data"), DefaultValue(0)]
        public int ActiveSectionIndex
        {
            get
            {
                if(ViewState["ActiveSectionIndex"] != null)
                    return (int)ViewState["ActiveSectionIndex"];
                else
                    return 0;
            }
            set { ViewState["ActiveSectionIndex"] = value; }
        }

        /// <summary>
        /// Gets/Sets the active section by title
        /// </summary>
        [Category("Data")]
        public string ActiveSectionTitle
        {
            get { return Sections[ActiveSectionIndex].Title; }
            set { ActiveSectionIndex = Sections.IndexOf(value) >= 0 ? Sections.IndexOf(value) : 0; }
        }

        /// <summary>
        /// The active section
        /// </summary>
        [Browsable(false)]
        public AccordionSection ActiveSection
        {
            get { return Sections[ActiveSectionIndex];  }
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
                        for (int i = 0; i < Sections.Count; ++i)
                            _sortOrder.Add(i);
                    }
                }

                if (_sortOrder.Count < Sections.Count)
                {
                    for (int i = _sortOrder.Count; i < Sections.Count; ++i)
                        _sortOrder.Add(i);
                }
                else if (_sortOrder.Count > Sections.Count)
                {
                    for (int i = Sections.Count; i < _sortOrder.Count; ++i)
                        _sortOrder.Remove(i);
                }

                return new List<int>(_sortOrder);
            }
            set
            {
                if (value.Count != Sections.Count)
                    throw new ArgumentException("The new sort order does not have the correct number of values");

                bool[] check = new bool[Sections.Count];
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

        #endregion

        #region Control Events

        ///
        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
            Page.RegisterRequiresPostBack(this);

            Script.AddResourceScript(Page, "jquery.ui.accordion.js");

            List<string> opts = new List<string>();

            opts.Add("header:\"> div > h3\"");
            if(SortOrder.IndexOf(ActiveSectionIndex) != 0)
                opts.Add(string.Format("active:{0}", SortOrder.IndexOf(ActiveSectionIndex)));

            if(!Enabled)
                opts.Add("disabled:true");
            if(!AutoHeight)
                opts.Add("autoHeight:false");
            if(ClearStyle)
                opts.Add("clearStyle:true");
            if(Collapsible)
                opts.Add("collapsible:true");
            if(FillSpace)
                opts.Add("fillSpace:true");
            if(Event != "click")
                opts.Add(string.Format("event:\"{0}\"", Event));

            if(HeaderUnselectedIcon != "ui-icon-triangle-1-e" || HeaderSelectedIcon != "ui-icon-triangle-1-s")
                opts.Add(string.Format("icons:{{\"header\":\"{0}\",\"headerSelected\":\"{1}\"}}", HeaderUnselectedIcon, HeaderSelectedIcon));

            if(Animation == Effect.EffectType.None)
                opts.Add("animated:false");
            else
            {
                Effect.SetScripts(Page, Animation);
                opts.Add(string.Format("animated:\"{0}\"", Effect.TypeString(Animation)));
            }

            string changeEvt =string.Format("$(\"#{0}_actIdx\").val(ui.newHeader.attr(\"oa\"));", ClientID);
            if (AutoPostBackActiveChanged)
                changeEvt += Page.ClientScript.GetPostBackEventReference(this, "activeChanged");
            if(_clientEvents != null)
            {
                if(!string.IsNullOrEmpty(_clientEvents.Create))
                    opts.Add(string.Format("create:function(event,ui){{{0};}}", _clientEvents.Create));
                if(!string.IsNullOrEmpty(_clientEvents.ChangeStart))
                    opts.Add(string.Format("changestart:function(event,ui){{{0};}}", _clientEvents.ChangeStart));
                if(!string.IsNullOrEmpty(_clientEvents.Change))
                    opts.Add(string.Format("change:function(event,ui){{{0}{1};}}", changeEvt, _clientEvents.Change));
                else
                    opts.Add(string.Format("change:function(event,ui){{{0}}}", changeEvt));
            }
            else
                opts.Add(string.Format("change:function(event,ui){{{0}}}", changeEvt));

            if(Sortable)
            {
                Script.AddResourceScript(Page, "jquery.ui.sortable.js");
                StringBuilder sb = new StringBuilder();
                sb.Append("axis:\"y\",delay:300,handle:\"h3\",containment:\"parent\",stop:function(event,ui){");
                sb.Append("var hid=$(\"#\"+$(this).attr(\"id\")+\"_order\");");
                sb.Append("var ord=\"\";");
                sb.Append("$(this).find(\"h3\").each(function(){ord+=$(this).attr(\"oa\")+\";\";});");
                sb.Append("ord=ord.slice(0,-1);");
                if(AutoPostBackOrderChanged)
                {
                    sb.Append("if(ord!=hid.val()){");
                    sb.Append("hid.val(ord);");
                    sb.Append(Page.ClientScript.GetPostBackEventReference(this, "OrderChanged"));
                }
                else
                    sb.Append("hid.val(ord);");
                sb.Append("}");
                Script.AddStartupScript(this, ClientID + "_sort", string.Format("$(\"#{0}\").sortable({{{1}}});", ClientID, sb.ToString()));
                sb.Clear();
                sb.AppendFormat("{0}", string.Join(";",SortOrder));
                ScriptManager.RegisterHiddenField(Page, ClientID + "_order", sb.ToString());
            }
            Script.AddStartupScript(this, ClientID, "accordion", opts);
            ScriptManager.RegisterHiddenField(this, ClientID + "_actIdx", ActiveSectionIndex.ToString());
        }

        ///
        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            for (int i = 0; i < Sections.Count; ++i)
            {
                int idx = SortOrder[i];
                Sections[idx].Render(writer, idx);
            }

            writer.RenderEndTag();
        }

        #endregion

        #region ViewState

        ///
        protected override void TrackViewState()
        {
            base.TrackViewState();
            if(_sections != null)
                _sections.TrackViewState();
            if(_clientEvents != null)
                _clientEvents.TrackViewState();
        }

        ///
        protected override void LoadViewState(object savedState)
        {
            object[] states = (object[])savedState;
            if(states[0] != null) base.LoadViewState(states[0]);
            if(states[1] != null) Sections.LoadViewState(states[1]);
            if(states[2] != null) ClientSideEvents.LoadViewState(states[2]);
        }

        ///
        protected override object SaveViewState()
        {
            if (_sortOrder != null)
                ViewState["SortOrder"] = _sortOrder;

            object[] states = new object[3];
            states[0] =  base.SaveViewState();
            if(_sections != null) states[1] = _sections.SaveViewState();
            if(_clientEvents != null) states[2] = _clientEvents.SaveViewState();
            return states;
        }

        #endregion

        #region PostBack

        /// <summary>
        /// The order of the sections has changed
        /// </summary>
        public event EventHandler OrderChanged;

        /// <summary>
        /// The selected section changed
        /// </summary>
        public event EventHandler ActiveSectionChanged;

        ///
        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            _orderChanged = _activeSectionChanged = false;

            if (!string.IsNullOrEmpty(postCollection[ClientID + "_actIdx"]))
            {
                int idx = 0;
                if (int.TryParse(postCollection[ClientID + "_actIdx"], out idx))
                {
                    if (idx != ActiveSectionIndex)
                    {
                        ActiveSectionIndex = idx;
                        _activeSectionChanged = true;
                    }
                }
            }

            if (!string.IsNullOrEmpty(postCollection[ClientID + "_order"]))
            {
                string newVals = postCollection[ClientID + "_order"].TrimEnd(';');
                string original = string.Join(";", SortOrder);
                if (newVals != original)
                {
                    try
                    {
                        List<int> newOrder = new List<int>();
                        string[] vals = newVals.Split(';');
                        for (int i = 0; i < Sections.Count; ++i)
                            newOrder.Add(int.Parse(vals[i]));

                        SortOrder = newOrder;
                        _orderChanged = true;
                    }
                    catch (Exception)
                    {
                        // Should I throw an exception?
                        // Something messed up on the client side
                        // Ignoring so as not to cause problems
                    }
                }
            }

            return (_orderChanged || _activeSectionChanged);
        }

        ///
        public void RaisePostDataChangedEvent()
        {
            if (_activeSectionChanged && ActiveSectionChanged != null)
                ActiveSectionChanged(this, new EventArgs());

            if (_orderChanged && OrderChanged != null)
                OrderChanged(this, new EventArgs());
        }

        #endregion

        #region Private

        private AccordionSectionList _sections;
        private AccordionClientEvents _clientEvents;
        private List<int> _sortOrder;
        private bool _orderChanged;
        private bool _activeSectionChanged;

        #endregion
    }

    /// <summary>
    /// The client side events for Accordion
    /// </summary>
    [ParseChildren(true)]
    public class AccordionClientEvents : ViewStateBase
    {
        /// <summary>
        /// Javascript to call on creation
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string Create
        {
            get
            {
                if(ViewState["Create"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Create"];
            }
            set { ViewState["Create"] = value; }
        }

        /// <summary>
        /// Javascript to call when changed section has happened
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string Change
        {
            get
            {
                if(ViewState["Change"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Change"];
            }
            set { ViewState["Change"] = value; }
        }

        /// <summary>
        /// Javascript to call when changeing section starts
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string ChangeStart
        {
            get
            {
                if(ViewState["ChangeStart"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["ChangeStart"];
            }
            set { ViewState["ChangeStart"] = value; }
        }
    }
}
