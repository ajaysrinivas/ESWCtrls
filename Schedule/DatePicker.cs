using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// Javascript datepicker
    /// </summary>
    [ToolboxData("<{0}:datepicker runat=\"server\"></{0}:datepicker>")]
    public class DatePicker : WebControl, IPostBackDataHandler
    {
        #region Properties

        #region Behaviour

        /// <summary>
        /// When a button is used, only show the datepicker when its clicked, not
        /// with focus
        /// </summary>
        [Category("Behaviour"),DefaultValue(false)]
        public bool ShowOnButtonOnly
        {
            get
            {
                if(ViewState["ShowOnButtonOnly"] != null)
                    return(bool)ViewState["ShowOnButtonOnly"];
                else
                    return false;
            }
            set
            {
                if(value != false)
                    ViewState["ShowOnButtonOnly"] = value;
                else
                    ViewState.Remove("ShowOnButtonOnly");
            }
        }

        /// <summary>
        /// If button image is set, just show the image not on a button
        /// </summary>
        [Category("Behaviour"), DefaultValue(false)]
        public bool ButtonImageOnly
        {
            get
            {
                if(ViewState["ButtonImageOnly"] == null)
                    return false;
                else
                    return (bool)ViewState["ButtonImageOnly"];
            }
            set { ViewState["ButtonImageOnly"] = value; }
        }

        /// <summary>
        /// Shows a drop down for changing the month
        /// </summary>
        [Category("Behaviour"), DefaultValue(false)]
        public bool ChangeMonth
        {
            get
            {
                if(ViewState["ChangeMonth"] == null)
                    return false;
                else
                    return (bool)ViewState["ChangeMonth"];
            }
            set { ViewState["ChangeMonth"] = value; }
        }

        /// <summary>
        /// Shows a drop down for changing the year
        /// </summary>
        [Category("Behaviour"), DefaultValue(false)]
        public bool ChangeYear
        {
            get
            {
                if(ViewState["ChangeYear"] == null)
                    return false;
                else
                    return (bool)ViewState["ChangeYear"];
            }
            set { ViewState["ChangeYear"] = value; }
        }

        /// <summary>
        /// The maximum date
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(DateTime?), null)]
        public DateTime? MaxDate
        {
            get
            {
                if(ViewState["MaxDate"] == null)
                    return null;
                else
                    return (DateTime?)ViewState["MaxDate"];
            }
            set { ViewState["MaxDate"] = value; }
        }

        /// <summary>
        /// The minimum date
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(DateTime?), null)]
        public DateTime? MinDate
        {
            get
            {
                if(ViewState["MinDate"] == null)
                    return null;
                else
                    return (DateTime?)ViewState["MinDate"];
            }
            set { ViewState["MinDate"] = value; }
        }

        /// <summary>
        /// Restricts the input to the standard format
        /// </summary>
        [Category("Behaviour"), DefaultValue(true)]
        public bool RestrictInput
        {
            get { return ((ViewState["RestrictInput"] == null) || ((bool)ViewState["RestrictInput"])); }
            set { ViewState["RestrictInput"] = value; }
        }

        /// <summary>
        /// The default date to use, defaults itself to today
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(DateTime?), null)]
        public DateTime? DefaultDate
        {
            get
            {
                if(ViewState["DefaultDate"] != null)
                    return(DateTime?)ViewState["DefaultDate"];
                else
                    return null;
            }
            set
            {
                if(value != null)
                    ViewState["DefaultDate"] = value;
                else
                    ViewState.Remove("DefaultDate");
            }
        }

        /// <summary>
        /// The date format to use
        /// </summary>
        [Category("Behaviour"), DefaultValue("dd/MM/yyyy")]
        public string DateFormat
        {
            get
            {
                if(ViewState["DateFormat"] != null)
                    return(string)ViewState["DateFormat"];
                else
                    return "dd/MM/yyyy";
            }
            set
            {
                if(value != "dd/MM/yyyy")
                    ViewState["DateFormat"] = value;
                else
                    ViewState.Remove("DateFormat");
            }
        }

        /// <summary>
        /// Whether to postback when a date is picked
        /// </summary>
        [Category("Behaviour"),DefaultValue(false)]
        public bool AutoPostBack
        {
            get
            {
                if(ViewState["AutoPostBack"] != null)
                    return(bool)ViewState["AutoPostBack"];
                else
                    return false;
            }
            set
            {
                if(value != false)
                    ViewState["AutoPostBack"] = value;
                else
                    ViewState.Remove("AutoPostBack");
            }
        }

        /// <summary>
        /// The client side events
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(DatePickerClientEvents), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public DatePickerClientEvents ClientSideEvents
        {
            get
            {
                if(_clientEvents == null)
                {
                    _clientEvents = new DatePickerClientEvents();
                    if(IsTrackingViewState) ((IStateManager)_clientEvents).TrackViewState();
                }
                return _clientEvents;
            }
        }

        #endregion

        #region Appearance

        /// <summary>
        /// A button image to use to pop up the calendar
        /// </summary>
        [Category("Behaviour"), DefaultValue(null)]
        public string ButtonImage
        {
            get
            {
                if(ViewState["ButtonImage"] == null)
                    return null;
                else
                    return (string)ViewState["ButtonImage"];
            }
            set { ViewState["ButtonImage"] = value; }
        }

        /// <summary>
        /// The text to use for a button to open the calendar
        /// </summary>
        [Category("Behaviour"), DefaultValue(null)]
        public string ButtonText
        {
            get
            {
                if(ViewState["ButtonText"] == null)
                    return null;
                else
                    return (string)ViewState["ButtonText"];
            }
            set { ViewState["ButtonText"] = value; }
        }

        /// <summary>
        /// The number of months to show
        /// </summary>
        [Category("Appearance"),DefaultValue(1)]
        public int NumberOfMonths
        {
            get
            {
                if(ViewState["NumberOfMonths"] != null)
                    return(int)ViewState["NumberOfMonths"];
                else
                    return 1;
            }
            set
            {
                if(value != 1)
                    ViewState["NumberOfMonths"] = value;
                else
                    ViewState.Remove("NumberOfMonths");
            }
        }

        /// <summary>
        /// Whether to show the button panel
        /// </summary>
        [Category("Appearance"),DefaultValue(false)]
        public bool ShowButtonPanel
        {
            get
            {
                if(ViewState["ShowButtonPanel"] != null)
                    return(bool)ViewState["ShowButtonPanel"];
                else
                    return false;
            }
            set
            {
                if(value != false)
                    ViewState["ShowButtonPanel"] = value;
                else
                    ViewState.Remove("ShowButtonPanel");
            }
        }

        /// <summary>
        /// In a multi-month display, where the current month should appear
        /// </summary>
        [Category("Appearance"),DefaultValue(0)]
        public int ShowCurrentMonthAtPosition
        {
            get
            {
                if(ViewState["ShowCurrentMonthAtPosition"] != null)
                    return(int)ViewState["ShowCurrentMonthAtPosition"];
                else
                    return 0;
            }
            set
            {
                if(value != 0)
                    ViewState["ShowCurrentMonthAtPosition"] = value;
                else
                    ViewState.Remove("ShowCurrentMonthAtPosition");
            }
        }

        /// <summary>
        /// Whether to show the current week numbers
        /// </summary>
        [Category("Appearance"),DefaultValue(false)]
        public bool ShowWeek
        {
            get
            {
                if(ViewState["ShowWeek"] != null)
                    return(bool)ViewState["ShowWeek"];
                else
                    return false;
            }
            set
            {
                if(value != false)
                    ViewState["ShowWeek"] = value;
                else
                    ViewState.Remove("ShowWeek");
            }
        }

        /// <summary>
        /// The year range to use in dropdown. has no effect on selectable dates
        /// </summary>
        [Category("Appearance"),DefaultValue(10)]
        public int YearRange
        {
            get
            {
                if(ViewState["YearRange"] != null)
                    return(int)ViewState["YearRange"];
                else
                    return 10;
            }
            set
            {
                if(value != 10)
                    ViewState["YearRange"] = value;
                else
                    ViewState.Remove("YearRange");
            }
        }

        /// <summary>
        /// The way the datepicker appears/disappears
        /// </summary>
        /// <remarks>Speed and callback are ignored</remarks>
        [Category("Appearance"),DefaultValue(null),MergableProperty(false),PersistenceMode(PersistenceMode.InnerProperty)]
        public Effect Effect
        {
            get
            {
                if(ViewState["Effect"] != null)
                    return (Effect)ViewState["Effect"];
                else
                    return null;
            }
            set
            {
                if(value != null)
                    ViewState["Effect"] = value;
                else
                    ViewState.Remove("Effect");
            }
        }
        
        #endregion

        #region Data

        /// <summary>
        /// The current date value
        /// </summary>
        [Bindable(true),Category("Data"),DefaultValue(typeof(DateTime?), null)]
        public DateTime? CurrentDate
        {
            get
            {
                if(ViewState["CurrentDate"] != null)
                    return(DateTime?)ViewState["CurrentDate"];
                else
                    return null;
            }
            set
            {
                if(value != null)
                    ViewState["CurrentDate"] = value;
                else
                    ViewState.Remove("CurrentDate");
            }
        }

        #endregion

        #endregion

        #region Control Events

        ///
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Script.AddResourceScript(Page, "jquery.ui.datepicker.js");
            List<string> opts = new List<string>();

            opts.Add(string.Format("dateFormat:\"{0}\"",jqFormat()));
            opts.Add("firstDay:1");
            opts.Add("gotoCurrent:true");
            opts.Add("showOtherMonths:true");
            opts.Add("selectOtherMonths:true");

            if(ChangeMonth)
                opts.Add("changeMonth:true");
            if(ChangeYear)
                opts.Add("changeYear:true");
            if(!RestrictInput)
                opts.Add("constrainInput:false");
            if(!Enabled)
                opts.Add("disabled:true");

            if(!string.IsNullOrEmpty(ButtonImage) || !string.IsNullOrEmpty(ButtonText))
            {
                if(ShowOnButtonOnly)
                    opts.Add("showOn:\"button\"");
                else
                    opts.Add("showOn:\"both\"");
                if(!string.IsNullOrEmpty(ButtonImage))
                    opts.Add(string.Format("buttonImage:\"{0}\"", ButtonImage));
                if(ButtonImageOnly)
                    opts.Add("buttonImageOnly:true");
                if(!string.IsNullOrEmpty(ButtonText))
                    opts.Add(string.Format("buttonText:\"{0}\"", ButtonText));
            }

            if(MinDate.HasValue)
                opts.Add(string.Format("minDate:new Date({0:yyyy,M,d})", MinDate.Value));
            if(MaxDate.HasValue)
                opts.Add(string.Format("maxDate:new Date({0:yyyy,M,d})", MaxDate.Value));
            if(DefaultDate.HasValue)
                opts.Add(string.Format("defaultDate:new Date({0:yyyy,M,d})", DefaultDate.Value));

            if(NumberOfMonths > 1)
            {
                opts.Add(string.Format("numberOfMonths:{0}", NumberOfMonths));
                if(ShowCurrentMonthAtPosition > 0)
                    opts.Add(string.Format("showCurrentAtPos:{0}", ShowCurrentMonthAtPosition));
            }

            if(ShowButtonPanel)
                opts.Add("showButtonPanel:true");

            if(ShowWeek)
                opts.Add("showWeek:true");

            string postBack = null;
            if(AutoPostBack)
            {
                if(CurrentDate.HasValue)
                    postBack = string.Format("if(dateText != \"{0}\"){1};", CurrentDate.Value.ToString(DateFormat), Page.ClientScript.GetPostBackEventReference(this, "dateChanged"));
                else
                    postBack = string.Format("if(dateText != \"\"){0};",  Page.ClientScript.GetPostBackEventReference(this, "dateChanged"));
            }

            ClientSideEvents.PreRender(opts, postBack);

            if(YearRange != 10)
                opts.Add(string.Format("yearRange:\"c-{0}:c+{0}\"", YearRange));

            if(Effect != null)
            {
                Effect.SetScripts(Page, Effect.Type);
                opts.Add(string.Format("showAnim:\"{0}\"", Effect.TypeString(Effect.Type)));
                if(Effect.Options.Count > 0)
                    opts.Add(string.Format("showOptions:{0}", Effect.RenderOptions()));
            }

            Script.AddStartupScript(this, "datepicker", opts);
        }

        ///
        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            if(CurrentDate.HasValue)
                writer.AddAttribute(HtmlTextWriterAttribute.Value, CurrentDate.Value.ToString(DateFormat));
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        #endregion

        #region Protected

        ///
        protected override void TrackViewState()
        {
            base.TrackViewState();
            if(_clientEvents != null) _clientEvents.TrackViewState();
        }

        ///
        protected override void LoadViewState(object savedState)
        {
            object[] states = (object[])savedState;
            if(states != null)
            {
                if(states[0] != null) base.LoadViewState(states[0]);
                if(states[1] != null) ClientSideEvents.LoadViewState(states[1]);
            }
        }

        ///
        protected override object SaveViewState()
        {
            object[] states = new object[2];
            states[0] = base.SaveViewState();
            if(_clientEvents != null) states[1] = _clientEvents.SaveViewState();
            return states;
        }

        #endregion

        #region Private

        private string jqFormat()
        {
            string rst = DateFormat.Replace("dddd","DD").Replace("ddd","D");

            if(rst.Contains("MMMM"))
                rst = rst.Replace("MMMM", "MM");
            else if(rst.Contains("MMM"))
                rst = rst.Replace("MMM", "M");
            else
                rst = rst.Replace("M", "m");

            rst = rst.Replace("yy", "y");

            return rst;
        }
        private DatePickerClientEvents _clientEvents;

        #endregion

        #region Postback

        /// <summary>
        /// Events signals the date has changed
        /// </summary>
        public event EventHandler DateChanged;


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
            bool changed = false;
            if (postCollection[UniqueID] != null)
            {
                if (!string.IsNullOrEmpty(postCollection[UniqueID]))
                {
                    DateTime rst = DateTime.MinValue;
                    if (DateTime.TryParseExact(Page.Request.Params[UniqueID], DateFormat, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out rst))
                    {
                        if (CurrentDate.HasValue)
                        {
                            if (CurrentDate.Value != rst)
                                changed = true;
                        }
                        else
                            changed = true;
                        CurrentDate = rst;
                    }
                    else
                    {
                        if (CurrentDate.HasValue)
                        {
                            changed = true;
                            CurrentDate = null;
                        }
                    }
                }
            }
            return changed;
        }


        /// <summary>
        /// When implemented by a class, signals the server control to notify the ASP.NET application that the state of the control has changed.
        /// </summary>
        public void RaisePostDataChangedEvent()
        {
            if(DateChanged != null)
                DateChanged(this, new EventArgs());
        }

        #endregion
    }

    /// <summary>
    /// The client events for the tree
    /// </summary>
    [ParseChildren(true)]
    public class DatePickerClientEvents : ViewStateBase
    {
        /// <summary>
        /// Javascript to call when datepicker is created
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
        /// Javascript to call before showing the datepicker
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string BeforeShow
        {
            get
            {
                if(ViewState["BeforeShow"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["BeforeShow"];
            }
            set { ViewState["BeforeShow"] = value; }
        }

        /// <summary>
        /// Javascript to call before showing a day
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string BeforeShowDay
        {
            get
            {
                if(ViewState["BeforeShowDay"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["BeforeShowDay"];
            }
            set { ViewState["BeforeShowDay"] = value; }
        }

        /// <summary>
        /// Javascript to call when changing month/year
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string OnChangeMonthYear
        {
            get
            {
                if(ViewState["OnChangeMonthYear"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["OnChangeMonthYear"];
            }
            set { ViewState["OnChangeMonthYear"] = value; }
        }

        /// <summary>
        /// Javascript to call when datepicker is closed
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string OnClose
        {
            get
            {
                if(ViewState["OnClose"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["OnClose"];
            }
            set { ViewState["OnClose"] = value; }
        }

        /// <summary>
        /// Javascript to call when a date is selected
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string OnSelect
        {
            get
            {
                if(ViewState["OnSelect"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["OnSelect"];
            }
            set { ViewState["OnSelect"] = value; }
        }

        #region Internal

        internal void PreRender(List<string> opts, string postBack)
        {
            if(!string.IsNullOrEmpty(Create))
                opts.Add(string.Format("create:function(){{{0}}}", Create));
            if(!string.IsNullOrEmpty(BeforeShow))
                opts.Add(string.Format("beforeShow:function(input,inst){{{0}}}", BeforeShow));
            if(!string.IsNullOrEmpty(BeforeShowDay))
                opts.Add(string.Format("beforeShowDay:function(date){{{0}}}", BeforeShowDay));
            if(!string.IsNullOrEmpty(OnChangeMonthYear))
                opts.Add(string.Format("onChangeMonthYear:function(year,month,inst){{{0}}}", OnChangeMonthYear));
            if(!string.IsNullOrEmpty(OnSelect))
                opts.Add(string.Format("onSelect:function(dateText,inst){{{0}}}", OnSelect));

            if(string.IsNullOrEmpty(postBack))
            {
                if(!string.IsNullOrEmpty(OnClose))
                    opts.Add(string.Format("onClose:function(dateText,inst){{{0}}}", OnClose));
            }
            else
            {

                if(!string.IsNullOrEmpty(OnClose))
                    opts.Add(string.Format("onClose:function(dateText,inst){{{0};{1}}}", OnClose, postBack));
                else
                    opts.Add(string.Format("onClose:function(dateText,inst){{{0}}}", postBack));
            }
        }

        #endregion

    }
}
