using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// The effects available to controls
    /// </summary>
    [ParseChildren(true,"Options")]
    public class Effect : ViewStateBase
    {
        #region Enums

        /// <summary>
        /// The different effects
        /// </summary>
        public enum EffectType
        {
            /// <summary>No effect</summary>
            None,
            /// <summary>The standard show effect</summary>
            Show,
            /// <summary>The standard face in effect</summary>
            FadeIn,
            /// <summary>The standard slide in effect</summary>
            SlideDown,
            /// <summary>Rolls up like a blind</summary>
            Blind,
            /// <summary>Bounce Effect</summary>
            Bounce,
            /// <summary>Shrinking to Center effect</summary>
            Clip,
            /// <summary>Dropped into place</summary>
            Drop,
            /// <summary>Explodes in/out of position</summary>
            Explode,
            /// <summary>Fades in</summary>
            Fade,
            /// <summary>Folds up</summary>
            Fold,
            /// <summary>Highlights the control</summary>
            Highlight,
            /// <summary>Pulsates in and out</summary>
            Pulsate,
            /// <summary>uses the size to disappear</summary>
            Scale,
            /// <summary>Shakes the control</summary>
            Shake,
            /// <summary>Slides out</summary>
            Slide,
            /// <summary>Transfer effect</summary>
            Transfer
        }

        /// <summary>
        /// The speed of the effect
        /// </summary>
        public enum EffectSpeed
        {
            /// <summary>The standard speed</summary>
            Normal,
            /// <summary>Slow Speed</summary>
            Slow,
            /// <summary>Fast Speed</summary>
            Fast
        }

        #endregion

        #region Properties

        /// <summary>
        /// The type of effect for this animation
        /// </summary>
        [Bindable(false), DefaultValue(EffectType.None)]
        public EffectType Type
        {
            get
            {
                if(ViewState["Type"] != null)
                    return (EffectType)ViewState["Type"];
                else
                    return EffectType.None;
            }
            set { ViewState["Type"] = value; }
        }

        /// <summary>
        /// The options to supply to the effect
        /// </summary>
        [Bindable(false), DefaultValue(typeof(OptionList), null), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public OptionList Options
        {
            get
            {
                if(_options == null)
                {
                    _options = new OptionList();
                    if(IsTrackingViewState)
                        ((IStateManager)_options).TrackViewState();
                }
                return _options;
            }
        }

        /// <summary>
        /// The speed of the effect, used if SpeedMilliSeconds is 0
        /// </summary>
        [Bindable(false), DefaultValue(EffectSpeed.Normal)]
        public EffectSpeed Speed
        {
            get
            {
                if(ViewState["Speed"] != null)
                    return (EffectSpeed)ViewState["Speed"];
                else
                    return EffectSpeed.Normal;
            }
            set { ViewState["Speed"] = value; }
        }

        /// <summary>
        /// The speed in milliseconds, use Speed if this is 0
        /// </summary>
        [Bindable(false), DefaultValue(0)]
        public int SpeedMillisecond
        {
            get
            {
                if(ViewState["SpeedMillisecond"] != null)
                    return (int)ViewState["SpeedMillisecond"];
                else
                    return 0;
            }
            set { ViewState["SpeedMillisecond"] = value; }
        }

        /// <summary>
        /// Call back function to run after effect is complete
        /// </summary>
        [Bindable(false), DefaultValue(null)]
        public string Callback
        {
            get
            {
                if(ViewState["Callback"] != null)
                    return (string)ViewState["Callback"];
                else
                    return null;
            }
            set { ViewState["Callback"] = value; }
        }

        #endregion

        #region ViewState

        ///
        protected internal override void TrackViewState()
        {
            base.TrackViewState();
            if(_options != null) ((IStateManager)_options).TrackViewState();
        }

        ///
        protected internal override void LoadViewState(object state)
        {
            if(state != null)
            {
                object[] states = (object[])state;
                if(states != null)
                {
                    if(states[0] != null) base.LoadViewState(states[0]);
                    if(states[1] != null) ((IStateManager)Options).LoadViewState(states[1]);
                }
            }
        }

        ///
        protected internal override object SaveViewState()
        {
            object[] states = new object[2];
            states[0] = base.SaveViewState();
            if(_options != null) states[1] = ((IStateManager)_options).SaveViewState();
            return states;
        }

        #endregion

        #region Internal

        internal static void SetScripts(Page p, EffectType e)
        {
            if(e != EffectType.Show && e != EffectType.SlideDown && e != EffectType.FadeIn)
                Script.AddResourceScript(p, "jquery.effects." + e.ToString().ToLower() + ".js");
        }

        internal static string TypeString(EffectType e)
        {
            if(e == EffectType.None)
                return null;
            if(e == EffectType.SlideDown)
                return "slideDown";
            else if(e == EffectType.FadeIn)
                return "fadeIn";
            else
                return e.ToString().ToLower();
        }

        /// <summary>
        /// Creates a string with the options done
        /// </summary>
        internal string Render(Page page)
        {
            if(Type == EffectType.None)
                return null;

            SetScripts(page, Type);

            StringBuilder rst = new StringBuilder();
            rst.AppendFormat("{{effect:\"{0}\"", TypeString(Type));

            if(_options != null && _options.Count > 0)
                rst.AppendFormat(",options:{0}", RenderOptions());
            else
                rst.Append(",options:null");


            if(SpeedMillisecond != 0)
                rst.AppendFormat(",speed:{0}", SpeedMillisecond);
            else
                rst.AppendFormat(",speed:\"{0}\"", Speed.ToString().ToLower());

            if(!string.IsNullOrEmpty(Callback))
                rst.AppendFormat(",callback:function(){{{0};}}", Callback);
            else
                rst.Append(",callback:null");

            return rst.ToString() + "}";
        }

        internal string RenderOptions()
        {
            if(_options != null && _options.Count > 0)
            {
                List<string> opts = new List<string>();
                foreach(Option o in _options)
                    opts.Add(string.Format("{0}:{1}", o.name, o.value));
                return string.Format("{{{0}}}", string.Join(",", opts));
            }
            else
                return string.Empty;
        }

        #endregion

        #region Private

        private OptionList _options;

        #endregion
    }
}
