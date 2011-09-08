using System.ComponentModel;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{

    /// <summary>
    /// A regular expression validator
    /// </summary>
    /// <remarks>
    /// This is an extension of the standard regular expression validator with an a addition for required
    /// </remarks>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal), ToolboxData("<{0}:RegExValidator runat=server />")]
    public class RegExValidator : BaseValidator
    {
        /// <summary>
        /// If a value is required
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(false)]
        public bool Required
        {
            get
            {
                if(ViewState["Required"] == null)
                    return false;
                else
                    return (bool)ViewState["Required"];
            }
            set { ViewState["Required"] = value; }
        }

        /// <summary>
        /// The validation expression
        /// </summary>
        /// <value>String</value>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string),null)]
        public string ValidationExpression
        {
            get
            {
                if(ViewState["ValidationExpression"] == null)
                    return null;
                else
                    return (string)ViewState["ValidationExpression"];
            }
            set { ViewState["ValidationExpression"] = value; }
        }

        /// <summary>
        /// Checks if the value is valid
        /// </summary>
        /// <returns>Boolean</returns>
        protected override bool EvaluateIsValid()
        {
            string value = GetControlValidationValue(ControlToValidate);
            if(string.IsNullOrEmpty(value))
            {
                if(Required)
                    return false;
                else
                    return true;
            }

            if(string.IsNullOrEmpty(ValidationExpression)) return true;

            return Regex.IsMatch(value, ValidationExpression);
        }

        ///
        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            if((RenderUplevel))
            {
                ScriptManager.RegisterExpandoAttribute(this, ClientID, "evaluationfunction", "ESW_RegExValidator", false);
                ScriptManager.RegisterExpandoAttribute(this, ClientID, "required", Required.ToString().ToLower(), false);
                ScriptManager.RegisterExpandoAttribute(this, ClientID, "validationexpression", ValidationExpression, false);
            }
        }

        ///
        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
            if((base.RenderUplevel))
            {
                Script.AddResourceScript(Page, "Validators.js");
            }
        }

    }
}