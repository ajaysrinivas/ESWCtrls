using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// Slider control uses jQuery
    /// </summary>
    [ToolboxData("<{0}:slider runat=\"server\"></{0}:slider>")]
    public class Slider : WebControl, IPostBackDataHandler
    {
        #region Properties

        #region Appearance

        /// <summary>
        /// The orientation of the slider
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(Orientation.Horizontal)]
        public Orientation Orientation
        {
            get
            {
                if(ViewState["Orientation"] != null)
                    return (Orientation)ViewState["Orientation"];
                else
                    return Orientation.Horizontal;
            }
            set { ViewState["Orientation"] = value; }
        }

        /// <summary>
        /// Whether the control should be animated for smoothing
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(false)]
        public bool Animate
        {
            get
            {
                if(ViewState["Animate"] != null)
                    return (bool)ViewState["Animate"];
                else
                    return false;
            }
            set { ViewState["Animate"] = value; }
        }

        /// <summary>
        /// The speed of the animate effect, if enabled and AnimateSpeedMilliSecond is 0
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(Effect.EffectSpeed.Normal)]
        public Effect.EffectSpeed AnimateSpeed
        {
            get
            {
                if(ViewState["AnimateSpeed"] != null)
                    return (Effect.EffectSpeed)ViewState["AnimateSpeed"];
                else
                    return Effect.EffectSpeed.Normal;
            }
            set { ViewState["AnimateSpeed"] = value; }
        }

        /// <summary>
        /// The speed to use for animation, if enabled, use AnimateSpeed if this is 0
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(0)]
        public int AnimateSpeedMillisecond
        {
            get
            {
                if(ViewState["AnimateSpeedMillisecond"] != null)
                    return (int)ViewState["AnimateSpeedMillisecond"];
                else
                    return 0;
            }
            set { ViewState["AnimateSpeedMillisecond"] = value; }
        }

        #endregion

        #region Behaviour

        /// <summary>
        /// The maximum value of the slider
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(100)]
        public int MaxValue
        {
            get
            {
                if(ViewState["MaxValue"] != null)
                    return (int)ViewState["MaxValue"];
                else
                    return 100;
            }
            set { ViewState["MaxValue"] = value; }
        }

        /// <summary>
        /// The minimum value of the slider
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(0)]
        public int MinValue
        {
            get
            {
                if(ViewState["MinValue"] != null)
                    return (int)ViewState["MinValue"];
                else
                    return 0;
            }
            set { ViewState["MinValue"] = value; }
        }

        /// <summary>
        /// The steps within the slider
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(1)]
        public int Step
        {
            get
            {
                if(ViewState["Step"] != null)
                    return (int)ViewState["Step"];
                else
                    return 1;
            }
            set { ViewState["Step"] = value; }
        }

        /// <summary>
        /// The range type for the slider
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(RangeType.None)]
        public RangeType Range
        {
            get
            {
                if(ViewState["Range"] != null)
                    return (RangeType)ViewState["Range"];
                else
                    return RangeType.None;
            }
            set { ViewState["Range"] = value; }
        }

        /// <summary>
        /// The client side events for slider
        /// </summary>
        [Category("Behaviour"), DefaultValue(typeof(SliderClientEvents), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public SliderClientEvents ClientEvents
        {
            get
            {
                if (_clientEvents == null)
                {
                    _clientEvents = new SliderClientEvents();
                    if (IsTrackingViewState) ((IStateManager)_clientEvents).TrackViewState();
                }
                return _clientEvents;
            }
        }

        /// <summary>
        /// Whether to post back when the value has changed
        /// </summary>
        [Category("Behaviour"), DefaultValue(false)]
        public bool AutoPostback
        {
            get
            {
                if(ViewState["AutoPostback"] != null)
                    return (bool)ViewState["AutoPostback"];
                else
                    return false;
            }
            set { ViewState["AutoPostback"] = value; }
        }

        #endregion

        #region Data

        /// <summary>
        /// The value of the slider, if theirs more than one handle the value of the first.
        /// </summary>
        [Bindable(true), Category("Data"), DefaultValue(0)]
        public int Value
        {
            get { return Values[0]; }
            set { Values[0] = value; }
        }

        /// <summary>
        /// If theirs multiple handles their values in a list
        /// </summary>
        [Browsable(false)]
        public List<int> Values
        {
            get
            {
                if (_values == null)
                {
                    if (ViewState["Values"] != null)
                        _values = (List<int>)ViewState["Values"];
                    else
                    {
                        _values = new List<int>();
                        _values.Add(0);
                    }
                }

                return _values;
            }
        }

        #endregion

        #endregion

        #region Control Events

        ///
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Page.RegisterRequiresPostBack(this);

            if (_values != null)
                ViewState["Values"] = _values;

            Script.AddResourceScript(Page, "jquery.ui.slider.js");

            List<string> opts = new List<string>();

            if (!Enabled)
                opts.Add("disabled:true");
            if (Orientation != Orientation.Horizontal)
                opts.Add("orientation:\"vertical\"");

            if (Animate)
            {
                if (AnimateSpeedMillisecond > 0)
                    opts.Add(string.Format("animate:{0}", AnimateSpeedMillisecond));
                else
                    opts.Add(string.Format("animate:\"{0}\"", AnimateSpeed.ToString().ToLower()));
            }

            if (MaxValue != 100)
                opts.Add(string.Format("max:{0}", MaxValue));
            if (MinValue != 0)
                opts.Add(string.Format("min:{0}", MinValue));
            if (Step != 1)
                opts.Add(string.Format("step:{0}", Step));

            switch (Range)
            {
                case RangeType.Range: opts.Add("range:true"); break;
                case RangeType.Minimum: opts.Add("range:\"min\""); break;
                case RangeType.Maximum: opts.Add("range:\"max\""); break;
                default: /* None */ break;
            }

            string setVals = null;
            if (Values.Count == 1)
            {
                opts.Add(string.Format("value:{0}", Values[0]));
                setVals = string.Format("$(\"#{0}_vals\").val(ui.value);", ClientID);
            }
            else if (Values.Count > 1)
            {
                opts.Add(string.Format("values:[{0}]", string.Join(",", Values)));
                setVals = string.Format("$(\"#{0}_vals\").val($(\"#{0}\").slider(\"option\",\"values\").join());", ClientID);
            }

            if (AutoPostback)
                setVals += Page.ClientScript.GetPostBackEventReference(this, "ValueChanged");

            if (_clientEvents != null)
            {
                if (!string.IsNullOrEmpty(_clientEvents.Change))
                    opts.Add(string.Format("create:function(event,ui){{{0};}}", _clientEvents.Create));
                if (!string.IsNullOrEmpty(_clientEvents.Start))
                    opts.Add(string.Format("start:function(event,ui){{{0};}}", _clientEvents.Start));
                if (!string.IsNullOrEmpty(_clientEvents.Slide))
                    opts.Add(string.Format("slide:function(event,ui){{{0};}}", _clientEvents.Slide));
                if (!string.IsNullOrEmpty(_clientEvents.Change))
                    opts.Add(string.Format("change:function(event,ui){{{0}{1};}}", setVals, _clientEvents.Change));
                else
                    opts.Add(string.Format("change:function(event,ui){{{0}}}", setVals));
                if (!string.IsNullOrEmpty(_clientEvents.Stop))
                    opts.Add(string.Format("stop:function(event,ui){{{0};}}", _clientEvents.Stop));
            }
            else
                opts.Add(string.Format("change:function(event,ui){{{0}}}", setVals));

            Script.AddStartupScript(this, ClientID, "slider", opts);
            ScriptManager.RegisterHiddenField(this, ClientID + "_vals", string.Join(",", Values));
        }

        ///
        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();
        }

        #endregion

        #region Private

        private List<int> _values;
        private SliderClientEvents _clientEvents;

        #endregion

        #region Postback Event

        /// <summary>
        /// Raised when the slider changes value
        /// </summary>
        public event EventHandler ValueChanged;

        ///
        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            bool changed = false;
            if (!string.IsNullOrEmpty(Page.Request.Params[ClientID + "_vals"]))
            {
                string[] vals = Page.Request.Params[ClientID + "_vals"].Split(',');
                int val = 0;
                if (vals.Length != Values.Count)
                {
                    changed = true;
                    if (vals.Length < Values.Count)
                        Values.RemoveRange(vals.Length, Values.Count - vals.Length);
                    else
                    {
                        for (int i = Values.Count; i < vals.Length; ++i)
                            Values.Add(0);
                    }
                }

                for (int i = 0; i < vals.Length; ++i)
                {
                    if (int.TryParse(vals[i], out val))
                    {
                        if (Values[i] != val)
                        {
                            Values[i] = val;
                            changed = true;
                        }
                    }
                }
            }

            return changed;
        }

        ///
        public void RaisePostDataChangedEvent()
        {
            if (ValueChanged != null)
                ValueChanged(this, new EventArgs());
        }

        #endregion

    }

    /// <summary>
    /// The client side events for Slider
    /// </summary>
    [ParseChildren(true)]
    public class SliderClientEvents : ViewStateBase
    {
        /// <summary>
        /// Javascript to call on creation
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string Create
        {
            get
            {
                if (ViewState["Create"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Create"];
            }
            set { ViewState["Create"] = value; }
        }

        /// <summary>
        /// Javascript to call when starting the slide with the mouse
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string Start
        {
            get
            {
                if (ViewState["Start"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Start"];
            }
            set { ViewState["Start"] = value; }
        }

        /// <summary>
        /// Javascript to call when sliding with the mouse
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string Slide
        {
            get
            {
                if (ViewState["Slide"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Slide"];
            }
            set { ViewState["Slide"] = value; }
        }

        /// <summary>
        /// Javascript to call when sliding has stopped or value is changed programtically
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string Change
        {
            get
            {
                if (ViewState["Change"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Change"];
            }
            set { ViewState["Change"] = value; }
        }

        /// <summary>
        /// Javascript to call when sliding has stopped with the mouse
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null)]
        public string Stop
        {
            get
            {
                if (ViewState["Stop"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Stop"];
            }
            set { ViewState["Stop"] = value; }
        }
    }
}
