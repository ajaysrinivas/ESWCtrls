using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.Services;

namespace ESWCtrls
{
    using Internal;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// Autocomplete control, based on jQuery Control
    /// </summary>
    [ToolboxData("<{0}:autocomplete runat=\"server\" />")]
    public class AutoComplete : WebControl, IPostBackDataHandler
    {
        #region Properties

        #region Apperance

        /// <summary>
        /// Gets the position.
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Positioning), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public Positioning Position
        {
            get
            {
                if(_pos == null)
                {
                    _pos = new Positioning();
                    if(IsTrackingViewState) ((IStateManager)_pos).TrackViewState();
                }
                return _pos;
            }
        }

        #endregion

        #region Behaviour

        /// <summary>
        /// Which element the menu should be appended to  (default is body)
        /// </summary>
        [DefaultValue("body"), Category("Behaviour"), IDReferenceProperty()]
        public string AppendTo
        {
            get
            {
                if(ViewState["AppendTo"] != null)
                    return (string)ViewState["AppendTo"];
                else
                    return "body";
            }
            set
            {
                if(value != "body")
                    ViewState["AppendTo"] = value;
                else
                    ViewState.Remove("AppendTo");
            }
        }

        /// <summary>
        /// If true the first item will be automatically focused
        /// </summary>
        [DefaultValue(false), Category("Behaviour")]
        public bool AutoFocus
        {
            get
            {
                if(ViewState["AutoFocus"] != null)
                    return (bool)ViewState["AutoFocus"];
                else
                    return false;
            }
            set
            {
                if(value != false)
                    ViewState["AutoFocus"] = value;
                else
                    ViewState.Remove("AutoFocus");
            }
        }

        /// <summary>
        /// The delay in milliseconds the autocomplete waits before activating
        /// </summary>
        [DefaultValue(300), Category("Behaviour")]
        public int Delay
        {
            get
            {
                if(ViewState["Delay"] != null)
                    return (int)ViewState["Delay"];
                else
                    return 300;
            }
            set
            {
                if(value != 300)
                    ViewState["Delay"] = value;
                else
                    ViewState.Remove("Delay");
            }
        }

        /// <summary>
        /// The minimum number of characters a user has to type before the autocomplete activates
        /// </summary>
        [DefaultValue(1), Category("Behaviour")]
        public int MinLength
        {
            get
            {
                if(ViewState["MinLength"] != null)
                    return (int)ViewState["MinLength"];
                else
                    return 1;
            }
            set
            {
                if(value != 1)
                    ViewState["MinLength"] = value;
                else
                    ViewState.Remove("MinLength");
            }
        }

        /// <summary>
        /// The client side events
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(AutoCompleteClientEvents), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public AutoCompleteClientEvents ClientSideEvents
        {
            get
            {
                if(_clientEvents == null)
                {
                    _clientEvents = new AutoCompleteClientEvents();
                    if(IsTrackingViewState) ((IStateManager)_clientEvents).TrackViewState();
                }
                return _clientEvents;
            }
        }

        #endregion

        #region Data

        /// <summary>
        /// Gets or sets the selected value.
        /// </summary>
        [Category("Data"),DefaultValue(null)]
        public string SelectedValue
        {
            get
            {
                if(ViewState["Value"] != null)
                    return (string)ViewState["Value"];
                else
                    return null;
            }
            set
            {
                if(Items.Count > 0)
                {
                    ListItem i = Items.FindByValue(value);
                    if(i != null)
                    {
                        ViewState["Value"] = value;
                        ViewState["Text"] = i.Text;
                    }
                    else
                        throw new ArgumentOutOfRangeException("When you have specified items, you must select one of the values.");
                }
                else
                {
                    if(value != null)
                        ViewState["Value"] = value;
                    else
                    {
                        ViewState.Remove("Value");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected text.
        /// </summary>
        [Category("Data"), DefaultValue(null)]
        public string SelectedText
        {
            get
            {
                if(ViewState["Text"] != null)
                    return (string)ViewState["Text"];
                else
                    return null;
            }
            set
            {
                if(Items.Count > 0)
                {
                    ListItem i = Items.FindByText(value);
                    if(i != null)
                    {
                        ViewState["Value"] = i.Value;
                        ViewState["Text"] = value;
                    }
                    else
                        throw new ArgumentOutOfRangeException("When you have specified items, you must select one of the values.");
                }
                else
                {
                    if(value != null)
                        ViewState["Text"] = value;
                    else
                    {
                        ViewState.Remove("Text");
                    }
                }
            }
        }


        /// <summary>
        /// Convenience function for setting value and text together
        /// </summary>
        public void setValueText(string value, string text)
        {
            if(Items.Count > 0)
            {
                ListItem i = Items.FindByValue(value);
                if(i == null)
                    i = Items.FindByText(text);

                if(i != null)
                {
                    ViewState["Value"] = i.Value;
                    ViewState["Text"] = i.Text;
                }
                else
                    throw new ArgumentOutOfRangeException("When you have specified items, you must select one of the values.");

            }
            else
            {
                ViewState["Value"] = value;
                ViewState["Text"] = value;
            }
        }

        /// <summary>
        /// The items to use in a pre build array for autocomplete
        /// </summary>
        [Category("Data"), DefaultValue(typeof(ListItemCollection)), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public ListItemCollection Items
        {
            get
            {
                if(_items == null)
                {
                    _items = new ListItemCollection();
                    if(IsTrackingViewState) ((IStateManager)_items).TrackViewState();
                }
                return _items;
            }
        }

        /// <summary>
        /// A source URL to get the auto complete values from, adds "term" with the term to search
        /// </summary>
        [DefaultValue(null), Category("Data")]
        public string SourceURL
        {
            get
            {
                if(ViewState["SourceURL"] != null)
                    return (string)ViewState["SourceURL"];
                else
                    return null;
            }
            set
            {
                if(value != null)
                    ViewState["SourceURL"] = value;
                else
                    ViewState.Remove("SourceURL");
            }
        }

        /// <summary>
        /// Delgate for Page Method soures
        /// </summary>
        /// <param name="term">The term.</param>
        public delegate ListItemCollection SourcePageMethodHandler(string term);


        /// <summary>
        /// PageMethod Source event, must be a static webMethod to be called from ajax
        /// </summary>
        public event SourcePageMethodHandler SourcePageMethod;

        #endregion

        #endregion

        #region Control Events

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Script.AddResourceScript(Page, "jquery.ui.autocomplete.js");
            List<string> opts = new List<string>();

            if(!Enabled)
                opts.Add("disabled:true");
            if(AutoFocus)
                opts.Add("autoFocus:true");
            if(Delay != 300)
                opts.Add(string.Format("delay:{0}", Delay));
            if(MinLength != 1)
                opts.Add(string.Format("minLength:{0}", MinLength));

            if(AppendTo != "body")
            {
                Control c = Util.FindControlRecursiveOut(this, AppendTo, null);
                if(c != null)
                    opts.Add(string.Format("appendTo:\"#{0}\"", c.ClientID));
                else
                    opts.Add(string.Format("appendTo:\"{0}\"", AppendTo));
            }

            if(Items.Count > 0)
            {
                List<string> itemList = new List<string>();
                foreach(ListItem i in Items)
                {
                    itemList.Add(string.Format("{{value:\"{0}\",label:\"{1}\"}}", i.Value, i.Text));
                }
                Script.AddStartupScript(this, string.Format("var {0}_src=[{1}];", ClientID, string.Join(",", itemList)));
                opts.Add(string.Format("source:{0}_src", ClientID));
            }
            else if(!string.IsNullOrEmpty(SourceURL))
                opts.Add(string.Format("source:\"{0}\"", SourceURL));
            else if(SourcePageMethod != null)
            {
                MethodInfo mi = SourcePageMethod.Method;
                bool found = false;
                foreach(Attribute a in mi.GetCustomAttributes(true))
                {
                    if(a is WebMethodAttribute)
                    {
                        found = true;
                        break;
                    }
                }
                if(!found || !mi.IsStatic || ! mi.IsPublic)
                    throw new ArgumentException("SourcePageMethod must be a static public web method.");

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("function(req,rsp){{$.ajax({{url:\"{0}/{1}\",data:\"{{{2}:\\\"\"+req.term+\"\\\"}}\",", System.IO.Path.GetFileName(Page.Request.CurrentExecutionFilePath), mi.Name, mi.GetParameters()[0].Name);
                sb.Append("dataType:\"json\",contentType:\"application/json; charset=utf-8\",type:\"POST\",");
                sb.Append("\nsuccess:function(msg){var rst=[];for(var i=0; i<msg.d.length;++i) rst.push({value:msg.d[i].Value,label:msg.d[i].Text});rsp(rst);},");
                sb.Append("error:function(msg,text){rsp();alert(msg+\"--\"+text);}});}");
                opts.Add(string.Format("source:{0}", sb.ToString()));
            }
            else
                throw new ArgumentNullException("No Source provided for auto complete control");

            opts.Add(string.Format("select:function(event,ui){{this.value=ui.item.label;$(\"#{0}_data\").val(ui.item.value);return false;}}", ClientID));

            Script.AddStartupScript(this, "autocomplete", opts);
            ScriptManager.RegisterHiddenField(this, ClientID + "_data", SelectedValue);
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            if(!string.IsNullOrEmpty(SelectedText))
                writer.AddAttribute(HtmlTextWriterAttribute.Value, SelectedText);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        #endregion

        #region Protected


        /// <summary>
        /// Causes the control to track changes to its view state so they can be stored in the object's <see cref="P:System.Web.UI.Control.ViewState"/> property.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();
            if(_items != null) ((IStateManager)_items).TrackViewState();
            if(_pos != null) _pos.TrackViewState();
            if(_clientEvents != null) _clientEvents.TrackViewState();
        }


        /// <summary>
        /// Restores view-state information from a previous request that was saved with the <see cref="M:System.Web.UI.WebControls.WebControl.SaveViewState"/> method.
        /// </summary>
        /// <param name="savedState">An object that represents the control state to restore.</param>
        protected override void LoadViewState(object savedState)
        {
            object[] states = (object[])savedState;
            if(states != null)
            {
                if(states[0] != null) base.LoadViewState(states[0]);
                if(states[1] != null) ((IStateManager)Items).LoadViewState(states[1]);
                if(states[2] != null) Position.LoadViewState(states[2]);
                if(states[3] != null) ClientSideEvents.LoadViewState(states[3]);
            }
        }


        /// <summary>
        /// Saves any state that was modified after the <see cref="M:System.Web.UI.WebControls.Style.TrackViewState"/> method was invoked.
        /// </summary>
        /// <returns>
        /// An object that contains the current view state of the control; otherwise, if there is no view state associated with the control, null.
        /// </returns>
        protected override object SaveViewState()
        {
            object[] states = new object[4];
            states[0] = base.SaveViewState();
            if(_items != null) states[1] = ((IStateManager)Items).SaveViewState();
            if(_pos != null) states[2] = _pos.SaveViewState();
            if(_clientEvents != null) states[4] = _clientEvents.SaveViewState();
            return states;
        }

        #endregion

        #region IPostBackDataHandler Members

        /// <summary>
        /// Occurs when the value or text has changed
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// When implemented by a class, processes postback data for an ASP.NET server control.
        /// </summary>
        /// <param name="postDataKey">The key identifier for the control.</param>
        /// <param name="postCollection">The collection of all incoming name values.</param>
        /// <returns>
        /// true if the server control's state changes as a result of the postback; otherwise, false.
        /// </returns>
        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            ListItem oldItem = new ListItem(SelectedText, SelectedValue);
            if(Items.Count > 0)
            {
                ListItem i = Items.FindByValue(SelectedValue);
                if(i == null)
                    Items.FindByText(SelectedText);

                if(i != null)
                    oldItem = i;
            }
            
            // Check to see if the main text is set
            if(postCollection[UniqueID] != null)
            {
                // See if we have the data value
                if(postCollection[UniqueID + "_data"] != null)
                {
                    // See if we have items, search for the selected one, and select it, or add it if it doesn't exist
                    if(Items.Count > 0)
                    {
                        ListItem i = Items.FindByValue(postCollection[UniqueID + "_data"]);
                        if(i != null)
                        {
                            SelectedValue = i.Value;
                            SelectedText = i.Text;
                        }
                        else
                        {
                            i = Items.FindByText(postCollection[UniqueID]);
                            if(i != null)
                            {
                                SelectedValue = i.Value;
                                SelectedText = i.Text;
                            }
                            else
                            {
                                // Not found so add an item
                                Items.Add(new ListItem(postCollection[UniqueID], postCollection[UniqueID + "_data"]));
                                SelectedValue = postCollection[UniqueID + "_data"];
                                SelectedText = postCollection[UniqueID];
                            }
                        }
                    }
                    else
                    {
                        SelectedValue = postCollection[UniqueID + "_data"];
                        SelectedText = postCollection[UniqueID];
                    }
                }
                else
                {
                    // See if we have items, search for the selected one, and select it, or add it if it doesn't exist
                    if(Items.Count > 0)
                    {
                        ListItem i = Items.FindByText(postCollection[UniqueID]);
                        if(i != null)
                        {
                            SelectedValue = i.Value;
                            SelectedText = i.Text;
                        }
                        else
                        {
                            // Not found so add an item
                            Items.Add(new ListItem(postCollection[UniqueID]));
                            SelectedValue = postCollection[UniqueID];
                            SelectedText = postCollection[UniqueID];
                        }
                    }
                    else
                    {
                        SelectedText = postCollection[UniqueID];
                    }
                }
            }
            else if(postCollection[UniqueID + "_data"] != null)
            {
                if(Items.Count > 0)
                {
                    ListItem i = Items.FindByValue(postCollection[UniqueID + "_data"]);
                    if(i != null)
                    {
                        SelectedValue = i.Value;
                        SelectedText = i.Text;
                    }
                    else
                    {
                        Items.Add(new ListItem(postCollection[UniqueID + "_data"]));
                        SelectedValue = postCollection[UniqueID + "_data"];
                        SelectedText = postCollection[UniqueID + "_data"];
                    }
                }
                else
                {
                    SelectedValue = postCollection[UniqueID + "_data"];
                    SelectedText = postCollection[UniqueID + "_data"];
                }
            }

            return (SelectedValue != oldItem.Value || SelectedText != oldItem.Text);
        }

        /// <summary>
        /// When implemented by a class, signals the server control to notify the ASP.NET application that the state of the control has changed.
        /// </summary>
        public void RaisePostDataChangedEvent()
        {
            if(SelectionChanged != null)
                SelectionChanged(this, new EventArgs());
        }

        #endregion

        #region Private
        
        private Positioning _pos;
        private AutoCompleteClientEvents _clientEvents;
        private ListItemCollection _items;

        #endregion
    }

    /// <summary>
    /// The AutoComplete Client Events
    /// </summary>
    [ParseChildren(true)]
    public class AutoCompleteClientEvents : ViewStateBase
    {
        /// <summary>
        /// Javascript to call when autocomplete control is created
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
        /// Javascript to call before the search request is started after delay and minlength are met
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string Search
        {
            get
            {
                if(ViewState["Search"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Search"];
            }
            set { ViewState["Search"] = value; }
        }

        /// <summary>
        /// Javascript to call when dropdown is opened
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string Open
        {
            get
            {
                if(ViewState["Open"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Open"];
            }
            set { ViewState["Open"] = value; }
        }

        /// <summary>
        /// Javascript to call when the textbox recieves focus
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string Focus
        {
            get
            {
                if(ViewState["Focus"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Focus"];
            }
            set { ViewState["Focus"] = value; }
        }

        /// <summary>
        /// Javascript to call when an item is selected from the dropdown
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string Select
        {
            get
            {
                if(ViewState["Select"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Select"];
            }
            set { ViewState["Select"] = value; }
        }

        /// <summary>
        /// Javascript to call when dropdown is closed
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string Close
        {
            get
            {
                if(ViewState["Close"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Close"];
            }
            set { ViewState["Close"] = value; }
        }

        /// <summary>
        /// Javascript to call when blur occurs and value has changed
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
    }
}
