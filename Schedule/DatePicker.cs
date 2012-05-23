using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
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
        #region Enums

        /// <summary>
        /// The mode of the datepicker
        /// </summary>
        public enum DateTimeMode
        {
            /// <summary>
            /// Just the timepicker itself
            /// </summary>
            Date,
            /// <summary>
            /// Just the time picker
            /// </summary>
            Time,
            /// <summary>
            /// The date plus time picker
            /// </summary>
            DateTime
        }

        #endregion

        #region Properties

        #region Behaviour

        /// <summary>
        /// The mode of the timepicker
        /// </summary>
        [Category("Behaviour"), DefaultValue(DateTimeMode.Date)]
        public DateTimeMode Mode
        {
            get
            {
                if(ViewState["Mode"] != null)
                    return (DateTimeMode)ViewState["Mode"];
                else
                    return DateTimeMode.Date;
            }
            set { ViewState["Mode"] = value; }
        }

        /// <summary>
        /// When a button is used, only show the datepicker when its clicked, not
        /// with focus
        /// </summary>
        [Category("Behaviour"), DefaultValue(false)]
        public bool ShowOnButtonOnly
        {
            get
            {
                if(ViewState["ShowOnButtonOnly"] != null)
                    return (bool)ViewState["ShowOnButtonOnly"];
                else
                    return false;
            }
            set { ViewState["ShowOnButtonOnly"] = value; }
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
        /// A datepicker to set the minimum date on
        /// </summary>
        [Category("Behaviour"), DefaultValue(null)]
        public string MinDateControl
        {
            get
            {
                if(ViewState["minDateCtrl"] != null)
                    return (string)ViewState["minDateCtrl"];
                else
                    return null;
            }
            set { ViewState["minDateCtrl"] = value; }
        }

        /// <summary>
        /// The Minimum date mode
        /// </summary>
        [Category("Behaviour"), DefaultValue(DateRangeMode.Fixed)]
        public DateRangeMode MinDateControlMode
        {
            get
            {
                if(ViewState["MinDateControlMode"] != null)
                    return (DateRangeMode)ViewState["MinDateControlMode"];
                else
                    return DateRangeMode.Fixed;
            }
            set { ViewState["MinDateControlMode"] = value; }
        }

        /// <summary>
        /// A datepicker to set the maximum date on
        /// </summary>
        [Category("Behaviour"), DefaultValue(null)]
        public string MaxDateControl
        {
            get
            {
                if(ViewState["maxDateCtrl"] != null)
                    return (string)ViewState["maxDateCtrl"];
                else
                    return null;
            }
            set { ViewState["maxDateCtrl"] = value; }
        }

        /// <summary>
        /// The Maximum date mode
        /// </summary>
        [Category("Behaviour"), DefaultValue(DateRangeMode.Fixed)]
        public DateRangeMode MaxDateControlMode
        {
            get
            {
                if(ViewState["MaxDateControlMode"] != null)
                    return (DateRangeMode)ViewState["MaxDateControlMode"];
                else
                    return DateRangeMode.Fixed;
            }
            set { ViewState["MaxDateControlMode"] = value; }
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
                    return (DateTime?)ViewState["DefaultDate"];
                else
                    return null;
            }
            set { ViewState["DefaultDate"] = value; }
        }

        /// <summary>
        /// Whether to allow a blank datefield
        /// </summary>
        [Category("Behaviour"), DefaultValue(true)]
        public bool AllowBlank
        {
            get
            {
                if(ViewState["AllowBlank"] != null)
                    return (bool)ViewState["AllowBlank"];
                else
                    return true;
            }
            set { ViewState["AllowBlank"] = value; }
        }

        /// <summary>
        /// Whether to allow manually entering of a date
        /// </summary>
        [Category("Behaviour"), DefaultValue(true)]
        public bool AllowManualEntry
        {
            get
            {
                if(ViewState["AllowManualEntry"] != null)
                    return (bool)ViewState["AllowManualEntry"];
                else
                    return true;
            }
            set { ViewState["AllowManualEntry"] = value; }
        }


        /// <summary>
        /// Whether to postback when a date is picked
        /// </summary>
        [Category("Behaviour"), DefaultValue(false)]
        public bool AutoPostBack
        {
            get
            {
                if(ViewState["AutoPostBack"] != null)
                    return (bool)ViewState["AutoPostBack"];
                else
                    return false;
            }
            set { ViewState["AutoPostBack"] = value; }
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

        #region Time Section

        /// <summary>
        /// The steps for hour slider (0 Smooth)
        /// </summary>
        [Category("Behaviour"), DefaultValue(0)]
        public int StepHour
        {
            get
            {
                if(ViewState["StepHour"] != null)
                    return (int)ViewState["StepHour"];
                else
                    return 0;
            }
            set { ViewState["StepHour"] = value; }
        }

        /// <summary>
        /// The steps for minute slider (0 Smooth)
        /// </summary>
        [Category("Behaviour"), DefaultValue(0)]
        public int StepMinute
        {
            get
            {
                if(ViewState["StepMin"] != null)
                    return (int)ViewState["StepMin"];
                else
                    return 0;
            }
            set { ViewState["StepMin"] = value; }
        }

        /// <summary>
        /// The steps for second slider (0 Smooth)
        /// </summary>
        [Category("Behaviour"), DefaultValue(0)]
        public int StepSecond
        {
            get
            {
                if(ViewState["StepSec"] != null)
                    return (int)ViewState["StepSec"];
                else
                    return 0;
            }
            set { ViewState["StepSec"] = value; }
        }

        /// <summary>
        /// The steps for millisecond slider (0 Smooth)
        /// </summary>
        [Category("Behaviour"), DefaultValue(0)]
        public int StepMillisecond
        {
            get
            {
                if(ViewState["StepMil"] != null)
                    return (int)ViewState["StepMil"];
                else
                    return 0;
            }
            set { ViewState["StepMil"] = value; }
        }

        #endregion

        #endregion

        #region Appearance

        /// <summary>
        /// The date format to use
        /// </summary>
        [Category("Appearance"), DefaultValue("dd/MM/yyyy")]
        public string DateFormat
        {
            get
            {
                if(ViewState["DateFormat"] != null)
                    return (string)ViewState["DateFormat"];
                else
                    return "dd/MM/yyyy";
            }
            set
            { ViewState["DateFormat"] = value; }
        }

        /// <summary>
        /// A button image to use to pop up the calendar
        /// </summary>
        [Category("Appearance"), DefaultValue(null)]
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
        [Category("Appearance"), DefaultValue(null)]
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
        [Category("Appearance"), DefaultValue(1)]
        public int NumberOfMonths
        {
            get
            {
                if(ViewState["NumberOfMonths"] != null)
                    return (int)ViewState["NumberOfMonths"];
                else
                    return 1;
            }
            set { ViewState["NumberOfMonths"] = value; }
        }

        /// <summary>
        /// Whether to show the button panel
        /// </summary>
        [Category("Appearance"), DefaultValue(false)]
        public bool ShowButtonPanel
        {
            get
            {
                if(ViewState["ShowButtonPanel"] != null)
                    return (bool)ViewState["ShowButtonPanel"];
                else
                    return false;
            }
            set { ViewState["ShowButtonPanel"] = value; }
        }

        /// <summary>
        /// In a multi-month display, where the current month should appear
        /// </summary>
        [Category("Appearance"), DefaultValue(0)]
        public int ShowCurrentMonthAtPosition
        {
            get
            {
                if(ViewState["ShowCurrentMonthAtPosition"] != null)
                    return (int)ViewState["ShowCurrentMonthAtPosition"];
                else
                    return 0;
            }
            set { ViewState["ShowCurrentMonthAtPosition"] = value; }
        }

        /// <summary>
        /// Whether to show the current week numbers
        /// </summary>
        [Category("Appearance"), DefaultValue(false)]
        public bool ShowWeek
        {
            get
            {
                if(ViewState["ShowWeek"] != null)
                    return (bool)ViewState["ShowWeek"];
                else
                    return false;
            }
            set { ViewState["ShowWeek"] = value; }
        }

        /// <summary>
        /// The year range to use in dropdown. has no effect on selectable dates
        /// </summary>
        [Category("Appearance"), DefaultValue(10)]
        public int YearRange
        {
            get
            {
                if(ViewState["YearRange"] != null)
                    return (int)ViewState["YearRange"];
                else
                    return 10;
            }
            set { ViewState["YearRange"] = value; }
        }

        /// <summary>
        /// The way the datepicker appears/disappears
        /// </summary>
        /// <remarks>Speed and callback are ignored</remarks>
        [Category("Appearance"), DefaultValue(null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public Effect Effect
        {
            get
            {
                if(ViewState["Effect"] != null)
                    return (Effect)ViewState["Effect"];
                else
                    return null;
            }
            set { ViewState["Effect"] = value; }
        }

        #region Time Section

        /// <summary>
        /// The time format to use
        /// </summary>
        [Category("Appearance"), DefaultValue("HH:mm")]
        public string TimeFormat
        {
            get
            {
                if(ViewState["TimeFormat"] != null)
                    return (string)ViewState["TimeFormat"];
                else
                    return "HH:mm";
            }
            set { ViewState["TimeFormat"] = value; }
        }

        /// <summary>
        /// Separator between date and time
        /// </summary>
        [Category("Appearance"), DefaultValue(" ")]
        public string Separator
        {
            get
            {
                if(ViewState["Separator"] != null)
                    return (string)ViewState["Separator"];
                else
                    return " ";
            }
            set { ViewState["Separator"] = value; }
        }

        /// <summary>
        /// Use 12 hour clock with am pm.
        /// </summary>
        [Category("Appearance"), DefaultValue(false)]
        public bool AMPM
        {
            get
            {
                if(ViewState["AMPM"] != null)
                    return (bool)ViewState["AMPM"];
                else
                    return false;
            }
            set { ViewState["AMPM"] = value; }
        }

        /// <summary>
        /// Show the hour slider
        /// </summary>
        [Category("Appearance"), DefaultValue(true)]
        public bool ShowHour
        {
            get
            {
                if(ViewState["ShowHour"] != null)
                    return (bool)ViewState["ShowHour"];
                else
                    return true;
            }
            set { ViewState["ShowHour"] = value; }
        }

        /// <summary>
        /// Show the minute slider
        /// </summary>
        [Category("Appearance"), DefaultValue(true)]
        public bool ShowMinute
        {
            get
            {
                if(ViewState["ShowMin"] != null)
                    return (bool)ViewState["ShowMin"];
                else
                    return true;
            }
            set { ViewState["ShowMin"] = value; }
        }

        /// <summary>
        /// Show the second slider
        /// </summary>
        [Category("Appearance"), DefaultValue(false)]
        public bool ShowSecond
        {
            get
            {
                if(ViewState["ShowSec"] != null)
                    return (bool)ViewState["ShowSec"];
                else
                    return false;
            }
            set { ViewState["ShowSec"] = value; }
        }

        /// <summary>
        /// Show the Millisecond slider
        /// </summary>
        [Category("Appearance"), DefaultValue(false)]
        public bool ShowMillisecond
        {
            get
            {
                if(ViewState["ShowMil"] != null)
                    return (bool)ViewState["ShowMil"];
                else
                    return false;
            }
            set { ViewState["ShowMil"] = value; }
        }

        /// <summary>
        /// Show num ticks under hour slider
        /// </summary>
        [Category("Appearance"), DefaultValue(false)]
        public bool ShowHourGrid
        {
            get
            {
                if(ViewState["ShowHourGrid"] != null)
                    return (bool)ViewState["ShowHourGrid"];
                else
                    return false;
            }
            set { ViewState["ShowHourGrid"] = value; }
        }

        /// <summary>
        /// Show num ticks under minute slider
        /// </summary>
        [Category("Appearance"), DefaultValue(false)]
        public bool ShowMinuteGrid
        {
            get
            {
                if(ViewState["ShowMinuteGrid"] != null)
                    return (bool)ViewState["ShowMinuteGrid"];
                else
                    return false;
            }
            set { ViewState["ShowMinuteGrid"] = value; }
        }

        /// <summary>
        /// Show num ticks under second slider
        /// </summary>
        [Category("Appearance"), DefaultValue(false)]
        public bool ShowSecondGrid
        {
            get
            {
                if(ViewState["ShowSecondGrid"] != null)
                    return (bool)ViewState["ShowSecondGrid"];
                else
                    return false;
            }
            set { ViewState["ShowSecondGrid"] = value; }
        }

        /// <summary>
        /// Show num ticks under millisecond slider
        /// </summary>
        [Category("Appearance"), DefaultValue(false)]
        public bool ShowMilliSecondGrid
        {
            get
            {
                if(ViewState["ShowMilliSecondGrid"] != null)
                    return (bool)ViewState["ShowMilliSecondGrid"];
                else
                    return false;
            }
            set { ViewState["ShowMilliSecondGrid"] = value; }
        }

        #endregion

        #endregion

        #region Data

        /// <summary>
        /// The current date value
        /// </summary>
        [Bindable(true), Category("Data"), DefaultValue(typeof(DateTime?), null)]
        public DateTime? CurrentDate
        {
            get
            {
                if(ViewState["CurrentDate"] != null)
                    return (DateTime?)ViewState["CurrentDate"];
                else
                    return null;
            }
            set { ViewState["CurrentDate"] = value; }
        }

        #endregion

        #endregion

        #region Control Events

        ///
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Script.AddResourceScript(Page, "jquery.ui.datepicker.js", "jquery.datepicker.js");

            List<string> opts = new List<string>();

            opts.Add(string.Format("dateFormat:\"{0}\"", jqFormat()));

            if(Mode != DateTimeMode.Date)
            {
                Script.AddResourceScript(Page, "jquery.timepicker.js");
                Util.addStyleSheet("timepicker.css", "TimePickerCSS", Page, this);

                opts.Add(string.Format("timeFormat:\"{0}\"", TimeFormat.Replace("H", "h")));

                if(StepHour > 0)
                    opts.Add(string.Format("stepHour:{0}", StepHour));
                if(StepMinute > 0)
                    opts.Add(string.Format("stepMinute:{0}", StepMinute));
                if(StepSecond > 0)
                    opts.Add(string.Format("stepSecond:{0}", StepSecond));
                if(StepMillisecond > 0)
                    opts.Add(string.Format("stepMillisec:{0}", StepMillisecond));

                if(Separator != " ")
                    opts.Add(string.Format("separator:\"{0}\"", Separator));

                if(AMPM)
                    opts.Add("ampm:true");

                if(!ShowHour)
                    opts.Add("showHour:false");
                if(!ShowMinute)
                    opts.Add("showMinute:false");
                if(ShowSecond)
                    opts.Add("showSecond:true");
                if(ShowMillisecond)
                    opts.Add("showMillisec:true");

                if(ShowHourGrid)
                    opts.Add("hourGrid:true");
                if(ShowMinuteGrid)
                    opts.Add("minuteGrid:true");
                if(ShowSecondGrid)
                    opts.Add("secondGrid:true");
                if(ShowMilliSecondGrid)
                    opts.Add("millisecGrid:true");
            }

            if(Mode != DateTimeMode.Time)
            {
                opts.Add("firstDay:1");
                opts.Add("gotoCurrent:true");
                opts.Add("showOtherMonths:true");
                opts.Add("selectOtherMonths:true");

                if(ChangeMonth)
                    opts.Add("changeMonth:true");
                if(ChangeYear)
                    opts.Add("changeYear:true");

                if(NumberOfMonths > 1)
                {
                    opts.Add(string.Format("numberOfMonths:{0}", NumberOfMonths));
                    if(ShowCurrentMonthAtPosition > 0)
                        opts.Add(string.Format("showCurrentAtPos:{0}", ShowCurrentMonthAtPosition));
                }

                if(ShowWeek)
                    opts.Add("showWeek:true");
            }


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

            if(ShowButtonPanel)
                opts.Add("showButtonPanel:true");

            if(DefaultDate.HasValue)
                opts.Add(string.Format("defaultDate:{0}", jsDate(DefaultDate.Value)));

            StringBuilder create = new StringBuilder();
            StringBuilder close = new StringBuilder();

            close.AppendFormat("ls_dp_close(dateText,inst,'{0}',{1},", Mode, AllowBlank.ToString().ToLower());

            MinMaxDateRange(ref create, ref close);

            if(MinDate.HasValue)
                opts.Add(string.Format("minDate:\"{0}\"", jsDate(MinDate.Value)));
            if(MaxDate.HasValue)
                opts.Add(string.Format("maxDate:\"{0}\"", jsDate(MaxDate.Value)));

            // Auto postback
            if(AutoPostBack)
                close.AppendFormat("if(dateText!=inst.lastVal){0};", Page.ClientScript.GetPostBackEventReference(this, "dateChanged"));

            ClientSideEvents.PreRender(opts, create.ToString(), close.ToString());



            if(YearRange != 10)
                opts.Add(string.Format("yearRange:\"c-{0}:c+{0}\"", YearRange));

            if(Effect != null)
            {
                Effect.SetScripts(Page, Effect.Type);
                opts.Add(string.Format("showAnim:\"{0}\"", Effect.TypeString(Effect.Type)));
                if(Effect.Options.Count > 0)
                    opts.Add(string.Format("showOptions:{0}", Effect.RenderOptions()));
            }

            if(Mode == DateTimeMode.Date)
                Script.AddStartupScript(this, ClientID, "datepicker", opts);
            else if(Mode == DateTimeMode.Time)
                Script.AddStartupScript(this, ClientID, "timepicker", opts);
            else
                Script.AddStartupScript(this, ClientID, "datetimepicker", opts);
        }

        ///
        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            if(!AllowManualEntry)
                writer.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "readonly");
            if(CurrentDate.HasValue)
                writer.AddAttribute(HtmlTextWriterAttribute.Value, CurrentDate.Value.ToString(FullFormat()));
            else if(!AllowBlank)
                writer.AddAttribute(HtmlTextWriterAttribute.Value, DefaultDate.GetValueOrDefault(DateTime.Now).ToString(FullFormat()));
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

        //Converts current ASP.NET format to jQuery format
        private string jqFormat()
        {
            string rst = DateFormat.Replace("dddd", "DD").Replace("ddd", "D");

            if(rst.Contains("MMMM"))
                rst = rst.Replace("MMMM", "MM");
            else if(rst.Contains("MMM"))
                rst = rst.Replace("MMM", "M");
            else
                rst = rst.Replace("M", "m");

            rst = rst.Replace("yy", "y");

            return rst;
        }

        private string jsDate(DateTime dt)
        {
            return "new Date(" + dt.ToString("yyyy, M, d, h, m, s") + ")";
        }

        private string FullFormat()
        {
            string ff = "";

            if(Mode != DateTimeMode.Time)
                ff = DateFormat;

            if(Mode == DateTimeMode.DateTime)
                ff += Separator;

            if(Mode != DateTimeMode.Date)
            {
                if(AMPM)
                    ff += TimeFormat.Replace("H", "h");
                else
                    ff += TimeFormat.Replace("h", "H");
            }

            return ff;
        }

        private void MinMaxDateRange(ref StringBuilder create, ref StringBuilder close)
        {
            if(!string.IsNullOrEmpty(MinDateControl))
            {
                DatePicker altCtrl = Util.FindControlRecursiveOut(this.Parent, MinDateControl, this) as DatePicker;
                if(altCtrl != null)
                {
                    close.AppendFormat("\"#{0}\",", altCtrl.ClientID);

                    if(MinDateControlMode == DateRangeMode.Fixed)
                    {
                        if(CurrentDate.HasValue)
                            create.AppendFormat("$(\"#{0}\").datepicker(\"option\",\"minDate\",{1});", altCtrl.ClientID, jsDate(CurrentDate.Value));
                        else if(!AllowBlank)
                            create.AppendFormat("$(\"#{0}\").datepicker(\"option\",\"minDate\",{1});", altCtrl.ClientID, jsDate(DefaultDate.GetValueOrDefault(DateTime.Now)));
                    }
                }
                else
                {
                    close.AppendFormat("\"{0}\",", MinDateControl);

                    if(MinDateControlMode == DateRangeMode.Fixed)
                    {
                        if(CurrentDate.HasValue)
                            create.AppendFormat("$(\"{0}\").datepicker(\"option\",\"minDate\",{1});", MinDateControl, jsDate(CurrentDate.Value));
                        else if(!AllowBlank)
                            create.AppendFormat("$(\"{0}\").datepicker(\"option\",\"minDate\",{1});", MinDateControl, jsDate(DefaultDate.GetValueOrDefault(DateTime.Now)));
                    }
                }

                close.AppendFormat("\"{0}\",", MinDateControlMode.ToString());
            }
            else
                close.AppendFormat("null,null,");

            if(!string.IsNullOrEmpty(MaxDateControl))
            {
                DatePicker altCtrl = Util.FindControlRecursiveOut(this.Parent, MaxDateControl, this) as DatePicker;
                if(altCtrl != null)
                {
                    close.AppendFormat("\"#{0}\",", altCtrl.ClientID);
                    if(MaxDateControlMode == DateRangeMode.Fixed)
                    {
                        if(CurrentDate.HasValue)
                            create.AppendFormat("$(\"#{0}\").datepicker(\"option\",\"maxDate\",{1});", altCtrl.ClientID, jsDate(CurrentDate.Value));
                        else if(!AllowBlank)
                            create.AppendFormat("$(\"#{0}\").datepicker(\"option\",\"maxDate\",{1});", altCtrl.ClientID, jsDate(DefaultDate.GetValueOrDefault(DateTime.Now)));
                    }
                }
                else
                {
                    close.AppendFormat("\"{0}\",", MaxDateControl);
                    if(MaxDateControlMode == DateRangeMode.Fixed)
                    {
                        if(CurrentDate.HasValue)
                            create.AppendFormat("$(\"{0}\").datepicker(\"option\",\"maxDate\",{1});", MaxDateControl, jsDate(CurrentDate.Value));
                        else if(!AllowBlank)
                            create.AppendFormat("$(\"{0}\").datepicker(\"option\",\"maxDate\",{1});", MaxDateControl, jsDate(DefaultDate.GetValueOrDefault(DateTime.Now)));
                    }
                }

                close.AppendFormat("\"{0}\");", MaxDateControlMode.ToString());
            }
            else
                close.Append("null,null);");
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
            if(postCollection[UniqueID] != null)
            {
                if(!string.IsNullOrEmpty(postCollection[UniqueID]))
                {
                    DateTime rst = DateTime.MinValue;
                    if(DateTime.TryParseExact(Page.Request.Params[UniqueID], FullFormat(), CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out rst))
                    {
                        if(CurrentDate.HasValue)
                        {
                            if(CurrentDate.Value != rst)
                                changed = true;
                        }
                        else
                            changed = true;
                        CurrentDate = rst;
                    }
                    else
                    {
                        if(CurrentDate.HasValue)
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

        internal void PreRender(List<string> opts, string intCreate, string close)
        {
            if(!string.IsNullOrEmpty(BeforeShow))
                opts.Add(string.Format("beforeShow:function(input,inst){{{0}}}", BeforeShow));
            if(!string.IsNullOrEmpty(BeforeShowDay))
                opts.Add(string.Format("beforeShowDay:function(date){{{0}}}", BeforeShowDay));
            if(!string.IsNullOrEmpty(OnChangeMonthYear))
                opts.Add(string.Format("onChangeMonthYear:function(year,month,inst){{{0}}}", OnChangeMonthYear));
            if(!string.IsNullOrEmpty(OnSelect))
                opts.Add(string.Format("onSelect:function(dateText,inst){{{0}}}", OnSelect));

            if(string.IsNullOrEmpty(intCreate))
            {
                if(!string.IsNullOrEmpty(Create))
                    opts.Add(string.Format("create:function(){{{0}}}", Create));
            }
            else
            {
                if(!string.IsNullOrEmpty(Create))
                    opts.Add(string.Format("create:function(){{{0};{1}}}", Create, intCreate));
                else
                    opts.Add(string.Format("create:function(){{{0}}}", intCreate));
            }

            if(string.IsNullOrEmpty(close))
            {
                if(!string.IsNullOrEmpty(OnClose))
                    opts.Add(string.Format("onClose:function(dateText,inst){{{0}}}", OnClose));
            }
            else
            {

                if(!string.IsNullOrEmpty(OnClose))
                    opts.Add(string.Format("onClose:function(dateText,inst){{{0};{1}}}", OnClose, close));
                else
                    opts.Add(string.Format("onClose:function(dateText,inst){{{0}}}", close));
            }
        }

        #endregion

    }
}
