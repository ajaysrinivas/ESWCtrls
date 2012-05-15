using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{
    using Internal;


    /// <summary>
    /// Javascript Timepicker
    /// </summary>
    [ToolboxData("<{0}:timepicker runat=\"server\"></{0}:timepicker>")]
    public class TimePicker : WebControl, IPostBackDataHandler
    {
        #region Properties

        /// <summary>
        /// The format to display the time in
        /// </summary>
        [Category("Behaviour"), DefaultValue("HH:mm")]
        public string TimeFormat
        {
            get
            {
                if (ViewState["TimeFormat"] != null)
                    return (string)ViewState["TimeFormat"];
                else
                    return "HH:mm";
            }
            set
            { ViewState["TimeFormat"] = value; }
        }

        /// <summary>
        /// The maximum time
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(TimeSpan?), null)]
        public TimeSpan? MaxTime
        {
            get
            {
                if (ViewState["MaxTime"] == null)
                    return null;
                else
                    return (TimeSpan?)ViewState["MaxTime"];
            }
            set { ViewState["MaxTime"] = value; }
        }

        /// <summary>
        /// The minimum time
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(TimeSpan?), null)]
        public TimeSpan? MinTime
        {
            get
            {
                if (ViewState["MinTime"] == null)
                    return null;
                else
                    return (TimeSpan?)ViewState["MinTime"];
            }
            set { ViewState["MinTime"] = value; }
        }

        /// <summary>
        /// A timepicker to set the minimum time on
        /// </summary>
        [Category("Behaviour"), DefaultValue(null)]
        public string MinTimeControl
        {
            get
            {
                if (ViewState["MinTimeControl"] != null)
                    return (string)ViewState["MinTimeControl"];
                else
                    return null;
            }
            set { ViewState["MinTimeControl"] = value; }
        }

        /// <summary>
        /// The Minimum time mode
        /// </summary>
        [Category("Behaviour"), DefaultValue(RangeMode.Fixed)]
        public RangeMode MinTimeControlMode
        {
            get
            {
                if (ViewState["MinTimeControlMode"] != null)
                    return (RangeMode)ViewState["MinTimeControlMode"];
                else
                    return RangeMode.Fixed;
            }
            set { ViewState["MinTimeControlMode"] = value; }
        }

        /// <summary>
        /// A timepicker to set the maximum time on
        /// </summary>
        [Category("Behaviour"), DefaultValue(null)]
        public string MaxTimeControl
        {
            get
            {
                if (ViewState["MaxTimeControl"] != null)
                    return (string)ViewState["MaxTimeControl"];
                else
                    return null;
            }
            set { ViewState["MaxTimeControl"] = value; }
        }

        /// <summary>
        /// The Maximum time mode
        /// </summary>
        [Category("Behaviour"), DefaultValue(RangeMode.Fixed)]
        public RangeMode MaxTimeControlMode
        {
            get
            {
                if (ViewState["MaxTimeControlMode"] != null)
                    return (RangeMode)ViewState["MaxTimeControlMode"];
                else
                    return RangeMode.Fixed;
            }
            set { ViewState["MaxTimeControlMode"] = value; }
        }

        /// <summary>
        /// The number of minutes between values
        /// </summary>
        [Category("Behaviour"), DefaultValue(30)]
        public int Interval
        {
            get
            {
                if (ViewState["Interval"] != null)
                    return (int)ViewState["Interval"];
                else
                    return 30;
            }
            set { ViewState["Interval"] = value; }
        }

        /// <summary>
        /// Whether to show the dropdown
        /// </summary>
        [Category("Behaviour"), DefaultValue(true)]
        public bool ShowDropDown
        {
            get
            {
                if (ViewState["DropDown"] != null)
                    return (bool)ViewState["DropDown"];
                else
                    return true;
            }
            set { ViewState["DropDown"] = value; }
        }

        /// <summary>
        /// Whether to show the scrollbar
        /// </summary>
        [Category("Behaviour"), DefaultValue(false)]
        public bool ShowScrollbar
        {
            get
            {
                if (ViewState["Scrollbar"] != null)
                    return (bool)ViewState["Scrollbar"];
                else
                    return false;
            }
            set { ViewState["Scrollbar"] = value; }
        }

        /// <summary>
        /// Whether to postback when a date is picked
        /// </summary>
        [Category("Behaviour"), DefaultValue(false)]
        public bool AutoPostBack
        {
            get
            {
                if (ViewState["AutoPostBack"] != null)
                    return (bool)ViewState["AutoPostBack"];
                else
                    return false;
            }
            set { ViewState["AutoPostBack"] = value; }
        }

        /// <summary>
        /// Code to execute client side on change of datepicker
        /// </summary>
        [Category("Behaviour"), DefaultValue(null)]
        public string OnClientChange
        {
            get
            {
                if (ViewState["OnClientChange"] != null)
                    return (string)ViewState["OnClientChange"];
                else
                    return null;
            }
            set { ViewState["OnClientChange"] = value; }
        }

        /// <summary>
        /// The time in the control
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(TimeSpan?), null)]
        public TimeSpan? CurrentTime
        {
            get
            {
                if (ViewState["CurrentTime"] != null)
                    return (TimeSpan?)ViewState["CurrentTime"];
                else
                    return null;
            }
            set { ViewState["CurrentTime"] = value; }
        }

        #endregion

        #region Control Events

        ///
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Script.AddResourceScript(Page, "jquery.timepicker.js");
            List<string> opts = new List<string>();

            TimeSpan current = CurrentTime.GetValueOrDefault(DateTime.Now.TimeOfDay);

            opts.Add(string.Format("timeFormat:'{0}'", TimeFormat));
            opts.Add(string.Format("startTime:new Date(0,0,0,{0},{1},{2})", current.Hours, current.Minutes, current.Seconds, current.Milliseconds));
            opts.Add(string.Format("interval:{0}", Interval));
            if (!ShowDropDown)
                opts.Add("dropdown:false");
            if (ShowScrollbar)
                opts.Add("scrollbar:true");

            if (MinTime.HasValue)
                opts.Add(string.Format("minTime:new Date(0,0,0,{0},{1},{2})", MinTime.Value.Hours, MinTime.Value.Minutes, MinTime.Value.Seconds, MinTime.Value.Milliseconds));
            if (MaxTime.HasValue)
                opts.Add(string.Format("maxTime:new Date(0,0,0,{0},{1},{2})", MaxTime.Value.Hours, MaxTime.Value.Minutes, MaxTime.Value.Seconds, MaxTime.Value.Milliseconds));

            StringBuilder evt = new StringBuilder();

            if (!string.IsNullOrEmpty(OnClientChange))
                evt.Append(OnClientChange + ";");

            if (!string.IsNullOrEmpty(MaxTimeControl) || !string.IsNullOrEmpty(MinTimeControl))
            {
                evt.Append("ls_tp_change(time,this,");

                if (!string.IsNullOrEmpty(MinTimeControl))
                {
                    Control c = Util.FindControlRecursiveOut(this.Parent, MinTimeControl, this) as TimePicker;
                    if (c != null)
                        evt.AppendFormat("'#{0}','{1}',", c.ClientID, MinTimeControlMode.ToString());
                    else
                        evt.Append("null,null,");
                }
                else
                    evt.Append("null,null,");

                if (!string.IsNullOrEmpty(MaxTimeControl))
                {
                    Control c = Util.FindControlRecursiveOut(this.Parent, MaxTimeControl, this) as TimePicker;
                    if (c != null)
                        evt.AppendFormat("'#{0}','{1}');", c.ClientID, MaxTimeControlMode.ToString());
                    else
                        evt.Append("null,null);");
                }
                else
                    evt.Append("null,null);");
            }

            if (AutoPostBack)
                evt.AppendFormat("{0};", Page.ClientScript.GetPostBackEventReference(this, "timeChanged"));

            if (evt.Length > 0)
                opts.Add("change:function(time){{{0}}}");

            Script.AddStartupScript(this, ClientID, "timepicker", opts);
        }

        ///
        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            if (CurrentTime.HasValue)
                writer.AddAttribute(HtmlTextWriterAttribute.Value, CurrentTime.Value.ToString(TimeFormat));
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        #endregion

        #region Postback

        /// <summary>
        /// Events signals the time has changed
        /// </summary>
        public event EventHandler TimeChanged;


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
                    TimeSpan rst = new TimeSpan();
                    DateTime inter = DateTime.MinValue;
                    if (TimeSpan.TryParseExact(Page.Request.Params[UniqueID], TimeFormat, null, out rst))
                    {
                        rst = inter.TimeOfDay;

                        if (CurrentTime.HasValue)
                        {
                            if (CurrentTime.Value != rst)
                                changed = true;
                        }
                        else
                            changed = true;
                        CurrentTime = rst;
                    }
                    else
                    {
                        if (CurrentTime.HasValue)
                        {
                            changed = true;
                            CurrentTime = null;
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
            if (TimeChanged != null)
                TimeChanged(this, new EventArgs());
        }

        #endregion
    }
}
