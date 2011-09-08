using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// A validator for checking multiple fields for validation, with conditions.
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal), ToolboxData("<{0}:MultiOrValidator runat=server />")]
    public class MultiValidator : BaseValidator
    {
        #region Disabled Properties

        /// <summary>
        /// Disbaling property
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool SetFocusOnError
        {
            get { return false; }
            set { throw new NotSupportedException("SetFocusOnError is not supported"); }
        }

        /// <summary>
        /// Disabling property
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new string ControlToValidate
        {
            get { return string.Empty; }
            set { throw new NotSupportedException("ControlToValidate is not supported"); }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The controls to validate, a semi colon seprated list of control IDs
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue("")]
        public string ControlsToValidate
        {
            get
            {
                if(ViewState["Controls"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["Controls"];
            }
            set { ViewState["Controls"] = value; }
        }

        /// <summary>
        /// The client side IDs of the controls to validate
        /// </summary>
        public string ClientsideControlsToValidate
        {
            get
            {
                if(string.IsNullOrEmpty(ControlsToValidate)) return string.Empty;
                string[] ctrls = ControlsToValidate.Split(";".ToCharArray());
                string rst = string.Empty;
                foreach(string ctrl in ctrls)
                {
                    if(!string.IsNullOrEmpty(ctrl.Trim()))
                        rst += ";" + GetControlRenderID(ctrl.Trim());
                }

                return rst.Remove(0, 1);
            }
        }

        /// <summary>
        /// The condition of the validation
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(Condition.OR)]
        public Condition Condition
        {
            get
            {
                if(ViewState["Condition"] == null)
                    return Condition.OR;
                else
                    return (Condition)ViewState["Condition"];
            }
            set { ViewState["Condition"] = value; }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Wether the validation is valid
        /// </summary>
        protected override bool EvaluateIsValid()
        {
            if(string.IsNullOrEmpty(ControlsToValidate)) return false;
            string[] parts = ControlsToValidate.Replace(" ", "").Split(";".ToCharArray());
            switch(Condition)
            {
                case Condition.OR:
                    foreach(string part in parts)
                    {
                        var value = GetControlValidationValue(part);
                        if(value == null)
                        {
                            Control ctrl = Util.FindControlRecursive(Page, part);
                            if(ctrl != null & ctrl is CheckBox)
                                if(((CheckBox)ctrl).Checked) return true;
                        }
                        else
                            if(!string.IsNullOrEmpty(value)) return true;
                    }

                    return false;
                case Condition.XOR:
                    bool found = false;
                    foreach(string part in parts)
                    {
                        var value = GetControlValidationValue(part);
                        if(value == null)
                        {
                            Control ctrl = Util.FindControlRecursive(Page, part);
                            if(ctrl != null & ctrl is CheckBox)
                            {
                                if(((CheckBox)ctrl).Checked)
                                {
                                    if(found) return false;
                                    found = true;
                                }
                            }
                        }
                        else
                        {
                            if(!string.IsNullOrEmpty(value))
                            {
                                if(found) return false;
                                found = true;
                            }
                        }
                    }

                    return found;
                case Condition.AND:
                    foreach(string part in parts)
                    {
                        var value = GetControlValidationValue(part);
                        if(value == null)
                        {
                            Control ctrl = Util.FindControlRecursive(Page, part);
                            if(ctrl != null & ctrl is CheckBox)
                                if(!((CheckBox)ctrl).Checked) return false;
                        }
                        else
                            if(string.IsNullOrEmpty(value)) return false;
                    }

                    return true;
            }
            return false;
        }

        /// <summary>
        /// checks to see if the properties of the control are valid
        /// </summary>
        /// <returns>True if valid</returns>
        protected override bool ControlPropertiesValid()
        {
            if(string.IsNullOrEmpty(ControlsToValidate))
                throw new HttpException(string.Format("The ControlsToValidate property of {0} cannot be empty", ID));

            string[] ctrls = ControlsToValidate.Split(";".ToCharArray());
            if(ctrls.Length < 2)
                throw new HttpException(string.Format("The ConstrolsToValidate property of {0} must have at least two IDs", ID));

            foreach(string ctrl in ctrls)
            {
                if(Util.FindControlRecursive(Page, ctrl) == null)
                    throw new HttpException(string.Format("The Control {0} listed in the ControlsToValidate property of {1} must exist", ctrl, ID));
            }

            return true;
        }

        /// <summary>
        /// The attributes to render
        /// </summary>
        /// <param name="writer">The html writer</param>
        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            base.SetFocusOnError = false;
            base.AddAttributesToRender(writer);
            if((RenderUplevel))
            {
                ScriptManager.RegisterExpandoAttribute(this, ClientID, "evaluationfunction", "ESW_MultiValidator", false);
                ScriptManager.RegisterExpandoAttribute(this, ClientID, "controlstovalidate", ClientsideControlsToValidate, false);
                ScriptManager.RegisterExpandoAttribute(this, ClientID, "condition", PropertyConverter.EnumToString(typeof(Condition), Condition), false);
            }
        }

        /// <summary>
        /// Called before rendering
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
            if((base.RenderUplevel))
            {
                Script.AddResourceScript(Page, "Validators.js");
                Script.AddStartupScript(this, string.Format("ESW_MultiValidatorSetup({0});", ClientID));
            }
        }

        #endregion

    }

}