using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{
    /// <summary>
    /// Password box control, fore entering passwords
    /// </summary>
    [DefaultProperty("Text"), ToolboxData("<{0}:password runat=\"server\"></{0}:password>")]
    public class PasswordEntry : WebControl, IPostBackDataHandler, IValidator
    {

        #region Properties

        #region Appearance

        /// <summary>
        /// The style for the password line
        /// </summary>
        [Category("Appearance"), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public TableItemStyle PasswordLineStyle
        {
            get
            {
                if(_pwdLineStyle == null)
                {
                    _pwdLineStyle = new TableItemStyle();
                    if(IsTrackingViewState)
                        ((IStateManager)_pwdLineStyle).TrackViewState();
                }
                return _pwdLineStyle;
            }
        }

        /// <summary>
        /// The text for the password label (default: Password)
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("Password"), Localizable(true)]
        public string PasswordLabel
        {
            get
            {
                if(ViewState["PasswordLabel"] == null)
                    return "Password";
                else
                    return (string)ViewState["PasswordLabel"];
            }
            set { ViewState["PasswordLabel"] = value; }
        }

        /// <summary>
        /// The style for the password label
        /// </summary>
        [Category("Appearance"), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public TableItemStyle PasswordLabelStyle
        {
            get
            {
                if(_pwdLblStyle == null)
                {
                    _pwdLblStyle = new TableItemStyle();
                    if(IsTrackingViewState)
                        ((IStateManager)_pwdLblStyle).TrackViewState();
                }
                return _pwdLblStyle;
            }
        }

        /// <summary>
        /// The style for the password text box
        /// </summary>
        [Category("Appearance"), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public TableItemStyle PasswordBoxStyle
        {
            get
            {
                if(_pwdBoxStyle == null)
                {
                    _pwdBoxStyle = new TableItemStyle();
                    if(IsTrackingViewState)
                        ((IStateManager)_pwdBoxStyle).TrackViewState();
                }
                return _pwdBoxStyle;
            }
        }

        /// <summary>
        /// The confirmation line style
        /// </summary>
        [Category("Appearance"), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public TableItemStyle ConfirmLineStyle
        {
            get
            {
                if(_confLineStyle == null)
                {
                    _confLineStyle = new TableItemStyle();
                    if(IsTrackingViewState)
                        ((IStateManager)_confLineStyle).TrackViewState();
                }
                return _confLineStyle;
            }
        }

        /// <summary>
        /// The text for the confirm label (Default: Confirm Password)
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("Confirm Password"), Localizable(true)]
        public string ConfirmLabel
        {
            get
            {
                if(ViewState["ConfirmLabel"] == null)
                    return "Confirm Password";
                else
                    return (string)ViewState["ConfirmLabel"];
            }
            set { ViewState["ConfirmLabel"] = value; }
        }

        /// <summary>
        /// The style for the Confirm label
        /// </summary>
        [Category("Appearance"), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public TableItemStyle ConfirmLabelStyle
        {
            get
            {
                if(_confLblStyle == null)
                {
                    _confLblStyle = new TableItemStyle();
                    if(IsTrackingViewState)
                        ((IStateManager)_confLblStyle).TrackViewState();
                }
                return _confLblStyle;
            }
        }

        /// <summary>
        /// The style for the Confirm text box
        /// </summary>
        [Category("Appearance"), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public TableItemStyle ConfirmBoxStyle
        {
            get
            {
                if(_confBoxStyle == null)
                {
                    _confBoxStyle = new TableItemStyle();
                    if(IsTrackingViewState)
                        ((IStateManager)_confBoxStyle).TrackViewState();
                }
                return _confBoxStyle;
            }
        }

        /// <summary>
        /// The error message to display if the password and the confirmation do not match (Default: The passwords must match)
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("The passwords must match"), Localizable(true)]
        public string NonMatchError
        {
            get
            {
                if(ViewState["NonMatchText"] == null)
                    return "The passwords must match";
                else
                    return (string)ViewState["NonMatchText"];
            }
            set { ViewState["NonMatchText"] = value; }
        }

        /// <summary>
        /// The error message to display if the password is required an not filled in (Default: The password is required)
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("The password is required"), Localizable(true)]
        public string RequiredError
        {
            get
            {
                if(ViewState["RequiredError"] == null)
                    return "The password is required";
                else
                    return (string)ViewState["RequiredError"];
            }
            set { ViewState["RequiredError"] = value; }
        }

        /// <summary>
        /// The error message to display of the password is less than the minimum required the (Default: The minimum length of a password is {0})
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("The password is less than the minimum length"), Localizable(true)]
        public string MinLengthError
        {
            get
            {
                if(ViewState["MinLenError"] == null)
                    return string.Format("The minimum length of a password is {0}", MinimumLength);
                else
                    return string.Format((string)ViewState["MinLenError"], MinimumLength);
            }
            set { ViewState["MinLenError"] = value; }
        }

        /// <summary>
        /// The style of the error messages
        /// </summary>
        [Category("Appearance"), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public TableItemStyle ErrorStyle
        {
            get
            {
                if(_errorStyle == null)
                {
                    _errorStyle = new TableItemStyle();
                    if(IsTrackingViewState)
                        ((IStateManager)_errorStyle).TrackViewState();
                }
                return _errorStyle;
            }
        }

        /// <summary>
        /// How the errors are to be displayed
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(ValidatorDisplay.Static)]
        public ValidatorDisplay ErrorDisplay
        {
            get
            {
                if(ViewState["ValidDisplay"] == null)
                    return ValidatorDisplay.Static;
                else
                    return (ValidatorDisplay)ViewState["ValidDisplay"];
            }
            set { ViewState["ValidDisplay"] = value; }
        }

        #endregion

        #region Behaviour

        /// <summary>
        /// Whether the password is to act filled in before rendering
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(false), Localizable(true)]
        public bool Filled
        {
            get
            {
                if(ViewState["PwdFilled"] == null)
                    return false;
                else
                    return (bool)ViewState["PwdFilled"];
            }
            set { ViewState["PwdFilled"] = value; }
        }

        /// <summary>
        /// True if the password is required
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
        /// The minimum length of the password
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(0)]
        public int MinimumLength
        {
            get
            {
                if(ViewState["MinLen"] == null)
                    return 0;
                else
                    return (int)ViewState["MinLen"];
            }
            set { ViewState["MinLen"] = value; }
        }

        #endregion

        /// <summary>
        /// The password as set by the user 
        /// </summary>
        public string Password
        {
            get
            {
                if(_password == "        ")
                    return string.Empty;
                else
                    return _password;
            }
        }

        #endregion

        #region Control Events

        ///
        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            _valid = true;
            Page.Validators.Add(this);
        }

        ///
        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
            Page.RegisterRequiresPostBack(this);
            ScriptManager.RegisterClientScriptResource(Page, this.GetType(), "ESWCtrls.ResEmbed.Scripts.Password.js");
            ScriptManager.RegisterExpandoAttribute(this, ClientID, "required", Required.ToString(), false);
            ScriptManager.RegisterExpandoAttribute(this, ClientID, "minLen", MinimumLength.ToString(), false);
            ScriptManager.RegisterExpandoAttribute(this, ClientID, "nmMsg", NonMatchError, false);
            ScriptManager.RegisterExpandoAttribute(this, ClientID, "reqMsg", RequiredError, false);
            ScriptManager.RegisterExpandoAttribute(this, ClientID, "mlMsg", MinLengthError, false);
            ScriptManager.RegisterExpandoAttribute(this, ClientID, "errDisplay", PropertyConverter.EnumToString(typeof(ValidatorDisplay), ErrorDisplay), false);
            ScriptManager.RegisterOnSubmitStatement(Page, this.GetType(), ClientID, "ESW_Password_Valid( " + ClientID + ");");
        }

        ///
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {

            string pwdText = string.Empty;
            if(Filled) pwdText = "        ";

            base.AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            //Password Line
            PasswordLineStyle.AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            PasswordLabelStyle.AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.For, ClientID + "_pwd");
            writer.RenderBeginTag(HtmlTextWriterTag.Label);
            writer.WriteEncodedText(PasswordLabel);
            writer.RenderEndTag();

            PasswordBoxStyle.AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_pwd");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_pwd");
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "password");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, pwdText);
            writer.AddAttribute(HtmlTextWriterAttribute.Onchange, "ESW_Password_Valid( " + ClientID + " );");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            writer.RenderEndTag();

            //Confirm Line
            ConfirmLineStyle.AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            ConfirmLabelStyle.AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.For, ClientID + "_conf");
            writer.RenderBeginTag(HtmlTextWriterTag.Label);
            writer.WriteEncodedText(ConfirmLabel);
            writer.RenderEndTag();

            ConfirmBoxStyle.AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_conf");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_conf");
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "password");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, pwdText);
            writer.AddAttribute(HtmlTextWriterAttribute.Onchange, "ESW_Password_Valid( " + ClientID + " );");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            writer.RenderEndTag();

            //Error Line
            ErrorStyle.AddAttributesToRender(writer);
            if(IsValid)
            {
                if(ErrorDisplay == ValidatorDisplay.Static)
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Visibility, "hidden");
                else
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_err");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.WriteEncodedText(ErrorMessage);
            writer.RenderEndTag();

            writer.RenderEndTag();
        }

        ///
        protected override void OnUnload(System.EventArgs e)
        {
            Page.Validators.Remove(this);
            base.OnUnload(e);
        }

        #endregion

        #region Post back events

        /// <summary>Text Changed in password event</summary>
        public event TextChangedEventHandler TextChanged;
        /// <summary>Text changed event hander</summary>
        public delegate void TextChangedEventHandler(object sender, System.EventArgs e);

        /// <summary>
        /// Loads the post back data
        /// </summary>
        /// <param name="postDataKey">The key for the postback data</param>
        /// <param name="postCollection">The collection of postback data</param>
        /// <returns></returns>
        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            bool rst = false;

            string anteValue = _password;
            string postValue = postCollection[postDataKey + "_pwd"];
            if(string.IsNullOrEmpty(anteValue) || (!anteValue.Equals(postValue)))
            {
                _password = postValue;
                rst = true;
            }

            postValue = postCollection[postDataKey + "_conf"];
            if(string.IsNullOrEmpty(anteValue) || (!anteValue.Equals(postValue)))
            {
                _confTest = postValue;
                rst = true;
            }

            return rst;
        }

        /// <summary>Raises the post back data changed</summary>
        public void RaisePostDataChangedEvent()
        {
            if(TextChanged != null)
                TextChanged(this, new EventArgs());
        }

        #endregion

        #region Validation

        /// <summary>
        /// Whether the passwords entered are valid
        /// </summary>
        public bool IsValid
        {
            get { return _valid; }
            set { _valid = value; }
        }

        /// <summary>
        /// The validation control group for the control
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue("")]
        public string ValidationGroup
        {
            get
            {
                if(ViewState["ValidationGroup"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["ValidationGroup"];
            }
            set { ViewState["ValidationGroup"] = value; }
        }

        /// <summary>
        /// Validates the password
        /// </summary>
        public void Validate()
        {
            if(!this.Visible)
            {
                IsValid = true;
                return;
            }

            _errorMsg = string.Empty;
            if(Filled)
            {
                if(_password == "        " && _password == _confTest)
                {
                    IsValid = true;
                    return;
                }
            }

            if(Required && (_password == null || _password.Length == 0))
            {
                IsValid = false;
                _errorMsg = RequiredError;
            }
            else if(_password != _confTest)
            {
                IsValid = false;
                _errorMsg = NonMatchError;
            }
            else if(_password.Length < MinimumLength)
            {
                IsValid = false;
                _errorMsg = MinLengthError;
            }
            else
                IsValid = true;
        }

        /// <summary>
        /// The error message to display
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                if(string.IsNullOrEmpty(_errorMsg)) Validate();
                return _errorMsg;
            }
            set { _errorMsg = value; }
        }

        #endregion

        #region Private Variables

        private string _password;
        private string _confTest;
        private string _errorMsg;
        private bool _valid;
        private TableItemStyle _pwdLineStyle;
        private TableItemStyle _pwdLblStyle;
        private TableItemStyle _pwdBoxStyle;
        private TableItemStyle _confLineStyle;
        private TableItemStyle _confLblStyle;
        private TableItemStyle _confBoxStyle;
        private TableItemStyle _errorStyle;

        #endregion

    }

}