using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{

    /// <summary>
    /// A number validation class with "AsYouType" client side checking
    /// </summary>
    /// <remarks>
    /// This class performs number validation on the selected control, and has several
    /// properties for controlling exactly how the text should look, as well as limiting
    /// user input in the first instance.
    /// </remarks>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal), ToolboxData("<{0}:NumberValidator runat=server />")]
    public class NumberValidator : BaseValidator
    {
        #region NumberRange

        /// <summary>
        /// Class is internal to NumberValidation used for handling
        /// range values, can have infinite values at either end of the range
        /// </summary>
        [Serializable()]
        public class NumberRange
        {
            /// <summary>
            /// Creates a NumberRange with no actual range set
            /// </summary>
            public NumberRange()
            {
                _min = 0;
                _max = 0;
                _minUsed = false;
                _maxUsed = false;
            }

            /// <summary>
            /// Creates a number range with the specified values
            /// </summary>
            /// <param name="minimum">The minimum value of the range</param>
            /// <param name="maximum">The maximim value of the range</param>
            public NumberRange(double minimum, double maximum)
            {
                _min = minimum;
                _max = maximum;
                _minUsed = true;
                _maxUsed = true;
            }

            /// <summary>
            /// Gets or sets the maximum value of the range
            /// </summary>
            public double Maximum
            {
                get { return _max; }
                set
                {
                    _max = value;
                    _maxUsed = true;
                }
            }

            /// <summary>
            /// Gets or sets whether the maximum value is used
            /// </summary>
            public bool MaximumUsed
            {
                get { return _maxUsed; }
                set { _maxUsed = value; }
            }

            /// <summary>
            /// Get or sets the minimum value of the range
            /// </summary>
            public double Minimum
            {
                get { return _min; }
                set
                {
                    _min = value;
                    _minUsed = true;
                }
            }

            /// <summary>
            /// Gets or sets whether the minimum value is used
            /// </summary>
            public bool MinimumUsed
            {
                get { return _minUsed; }
                set { _minUsed = value; }
            }

            /// <summary>
            /// Checks to see if the supplied value is within the range
            /// </summary>
            /// <param name="value">The value to check</param>
            /// <returns>If the vaalue is within range</returns>
            public bool InRange(double value)
            {
                if(_minUsed & value < _min) return false;
                if(_maxUsed & value > _max) return false;
                return true;
            }

            private double _min;
            private double _max;
            private bool _minUsed;
            private bool _maxUsed;
        }
        #endregion

        #region properties

        /// <summary>
        /// Gets or sets if the number is required
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(false)]
        public bool Required
        {
            get
            {
                if((ViewState["Required"] == null))
                    return false;
                else
                    return (bool)ViewState["Required"];
            }
            set { ViewState["Required"] = value; }
        }

        /// <summary>
        /// Gets or sets a prefix to allow
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(char),null)]
        public char Prefix
        {
            get
            {
                if((ViewState["Prefix"] == null))
                    return '\0';
                else
                    return (char)ViewState["Prefix"];
            }
            set { ViewState["Prefix"] = value; }
        }

        /// <summary>
        /// Gets or sets the number of decimal places to allow
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(0)]
        public int DecimalPlaces
        {
            get
            {
                if((ViewState["DecimalPlaces"] == null))
                    return 0;
                else
                    return (int)ViewState["DecimalPlaces"];
            }
            set { ViewState["DecimalPlaces"] = value; }
        }

        /// <summary>
        /// Gets or set whether to pad out the decimal places
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(true)]
        public bool PadDecimalPlaces
        {
            get
            {
                if(ViewState["PadDecimalPlaces"] == null)
                    return true;
                else
                    return (bool)ViewState["PadDecimalPlaces"];
            }
            set { ViewState["PadDecimalPlaces"] = value; }
        }


        /// <summary>
        /// Gets or sets if the thousands separator is allowed
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(false)]
        public bool ThoudandSeparators
        {
            get
            {
                if((ViewState["ThousandSeparator"] == null))
                    return false;
                else
                    return (bool)ViewState["ThousandSeparator"];
            }
            set { ViewState["ThousandSeparator"] = value; }
        }

        /// <summary>
        /// Gets or sets the minimum value to allow
        /// </summary>
        [Bindable(true), Category("Behaviour")]
        public double MinimumValue
        {
            get
            {
                if((ViewState["ValidRange"] == null))
                    return 0;
                else
                    return ((NumberRange)ViewState["ValidRange"]).Minimum;
            }
            set
            {
                NumberRange nr = default(NumberRange);
                if((ViewState["ValidRange"] == null))
                    nr = new NumberRange();
                else
                    nr = (NumberRange)ViewState["ValidRange"];

                nr.Minimum = value;
                ViewState["ValidRange"] = nr;
            }
        }

        /// <summary>
        /// Gets or sets the maximum value to allow
        /// </summary>
        [Bindable(true), Category("Behaviour")]
        public double MaxmimumValue
        {
            get
            {
                if((ViewState["ValidRange"] == null))
                    return 0;
                else
                    return ((NumberRange)ViewState["ValidRange"]).Maximum;
            }
            set
            {
                NumberRange nr = default(NumberRange);
                if((ViewState["ValidRange"] == null))
                    nr = new NumberRange();
                else
                    nr = (NumberRange)ViewState["ValidRange"];

                nr.Maximum = value;
                ViewState["ValidRange"] = nr;
            }
        }

        /// <summary>
        /// Gets or sets the number range to allow <see cref="NumberRange"/>
        /// </summary>
        public NumberRange ValidRange
        {
            get
            {
                if((ViewState["ValidRange"] == null))
                    return new NumberRange();
                else
                    return (NumberRange)ViewState["ValidRange"];
            }
            set { ViewState["ValidRange"] = value; }
        }

        #endregion

        #region control events

        ///
        protected override bool EvaluateIsValid()
        {
            string text = Regex.Replace(GetControlValidationValue(ControlToValidate).Replace(",", ""), "\\s+", "");

            if(string.IsNullOrEmpty(text))
            {
                if(Required)
                    return false;
                else
                    return true;
            }

            bool neg = false;
            if(text.StartsWith("-"))
            {
                neg = true;
                text = text.Substring(1);
            }

            if(char.IsControl(Prefix) == false & text.StartsWith(Prefix.ToString())) text = text.Substring(1);

            double val = 0;
            if( !double.TryParse(text,out val))
                return false;

            if(neg) val = 0 - val;

            if(ValidRange.InRange(val) == false)
                return false;

            string fmt = "0";
            if(ThoudandSeparators) fmt = "#,##0";
            if(char.IsControl(Prefix) == false) fmt = Prefix.ToString() + fmt;
            if(DecimalPlaces > 0)
            {
                fmt += ".";
                for(int i = 0; i <= (DecimalPlaces - 1); i++)
                    fmt += "0";
            }

            Control ctrl = Page.FindControl(ControlToValidate);
            if(ctrl is TextBox)
            {
                TextBox tb = (TextBox)ctrl;
                tb.Text = val.ToString(fmt);
            }

            return true;
        }

        ///
        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            if((RenderUplevel))
            {

                ScriptManager.RegisterExpandoAttribute(this, ClientID, "evaluationfunction", "ESW_NumberValidator", false);
                if(char.IsControl(Prefix) == false)
                    ScriptManager.RegisterExpandoAttribute(this, ClientID, "prefix", Prefix.ToString(), false);

                ScriptManager.RegisterExpandoAttribute(this, ClientID, "required", Required.ToString().ToLower(), false);
                ScriptManager.RegisterExpandoAttribute(this, ClientID, "decPlaces", DecimalPlaces.ToString(), false);
                ScriptManager.RegisterExpandoAttribute(this, ClientID, "padDecPlaces", PadDecimalPlaces.ToString().ToLower(), false);
                ScriptManager.RegisterExpandoAttribute(this, ClientID, "thouSeps", ThoudandSeparators.ToString().ToLower(), false);
                if(ValidRange.MinimumUsed)
                    ScriptManager.RegisterExpandoAttribute(this, ClientID, "minimum", ValidRange.Minimum.ToString(), false);

                if(ValidRange.MaximumUsed)
                    ScriptManager.RegisterExpandoAttribute(this, ClientID, "maximum", ValidRange.Maximum.ToString(), false);
            }
        }

        ///
        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
            if((base.RenderUplevel))
            {
                Script.AddResourceScript(Page, "NumberValidator.js");
                Script.AddStartupScript(this,string.Format("ESW_NumberValidatorSetup({0});", ClientID));
            }
        }

        #endregion

    }

}