using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{
    /// <summary>
    /// Extended Button class that uses jquery button
    /// </summary>
    [DefaultProperty("Text"), ToolboxData("<{0}:JButton runat=server />")]
    public class JButton : WebControl, IPostBackEventHandler, IPostBackDataHandler
    {
        #region Appearance

        /// <summary>
        /// The text to display
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string Text
        {
            get
            {
                if (ViewState["Text"] == null)
                    return "";
                else
                    return (string)ViewState["Text"];
            }
            set { ViewState["Text"] = value; }
        }

        /// <summary>
        /// The Active Text to display
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string ActiveText
        {
            get
            {
                if (ViewState["ActiveText"] == null)
                    return Text;
                else
                    return (string)ViewState["ActiveText"];
            }
            set { ViewState["ActiveText"] = value; }
        }

        /// <summary>
        /// Whether to show the text on the button
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(true)]
        public bool ShowText
        {
            get
            {
                if (ViewState["ShowText"] == null)
                    return true;
                else
                    return (bool)ViewState["ShowText"];
            }
            set { ViewState["ShowText"] = value; }
        }

        /// <summary>
        /// Whether to highlight an active button
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(false)]
        public bool HighlightActive
        {
            get
            {
                if (ViewState["HighlightActive"] == null)
                    return false;
                else
                    return (bool)ViewState["HighlightActive"];
            }
            set { ViewState["HighlightActive"] = value; }
        }

        /// <summary>
        /// The primary icon to show
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(JButtonIcon.None)]
        public JButtonIcon Primary
        {
            get
            {
                if (ViewState["Primary"] == null)
                    return JButtonIcon.None;
                else
                    return (JButtonIcon)ViewState["Primary"];
            }
            set { ViewState["Primary"] = value; }
        }

        /// <summary>
        /// The Secondary icon to show
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(JButtonIcon.None)]
        public JButtonIcon Secondary
        {
            get
            {
                if (ViewState["Secondary"] == null)
                    return JButtonIcon.None;
                else
                    return (JButtonIcon)ViewState["Secondary"];
            }
            set { ViewState["Secondary"] = value; }
        }

        /// <summary>
        /// The Active Primary icon to show
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(JButtonIcon.None)]
        public JButtonIcon ActivePrimary
        {
            get
            {
                if (ViewState["ActivePrimary"] == null)
                    return Primary;
                else
                    return (JButtonIcon)ViewState["ActivePrimary"];
            }
            set { ViewState["ActivePrimary"] = value; }
        }

        /// <summary>
        /// The ActiveSecondary icon to show
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(JButtonIcon.None)]
        public JButtonIcon ActiveSecondary
        {
            get
            {
                if (ViewState["ActiveSecondary"] == null)
                    return Secondary;
                else
                    return (JButtonIcon)ViewState["ActiveSecondary"];
            }
            set { ViewState["ActiveSecondary"] = value; }
        }

        #endregion

        #region Behaviour Properties

        /// <summary>
        /// Whenther this is a toggle button
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(false)]
        public bool Toggle
        {
            get
            {
                if (ViewState["Toogle"] == null)
                    return false;
                else
                    return (bool)ViewState["Toogle"];
            }
            set { ViewState["Toogle"] = value; }
        }

        /// <summary>
        /// Whenther the button is currently active
        /// </summary>
        [Bindable(true), Category("Data"), DefaultValue(false)]
        public bool Active
        {
            get
            {
                if (ViewState["Active"] == null)
                    return false;
                else
                    return (bool)ViewState["Active"];
            }
            set
            {
                if (Toggle) ViewState["Active"] = value;
            }
        }

        /// <summary>
        /// The command name for the button
        /// </summary>
        [Bindable(true), Category("Data"), DefaultValue("")]
        public string CommandName
        {
            get
            {
                if (ViewState["CommandName"] == null)
                    return "";
                else
                    return (string)ViewState["CommandName"];
            }
            set { ViewState["CommandName"] = value; }
        }

        /// <summary>
        /// The command argument for the button
        /// </summary>
        [Bindable(true), Category("Data"), DefaultValue("")]
        public string CommandArgument
        {
            get
            {
                if (ViewState["CommandArgument"] == null)
                    return "";
                else
                    return (string)ViewState["CommandArgument"];
            }
            set { ViewState["CommandArgument"] = value; }
        }

        /// <summary>
        /// A client side script to call when clicked
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue("")]
        public string OnClientClick
        {
            get
            {
                if (ViewState["OnClientClick"] == null)
                    return "";
                else
                    return (string)ViewState["OnClientClick"];
            }
            set { ViewState["OnClientClick"] = value; }
        }

        /// <summary>
        /// Whenther to postback when the button is clicked
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(true)]
        public bool Postback
        {
            get
            {
                if (ViewState["Postback"] == null)
                    return true;
                else
                    return (bool)ViewState["Postback"];
            }
            set { ViewState["Postback"] = value; }
        }

        #endregion

        #region Page events

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Page.RegisterRequiresPostBack(this);

            Script.AddResourceScript(Page, "jquery.ui.button.js");
            List<string> opts = new List<string>();

            if (!ShowText)
                opts.Add("text:false");

            if (Toggle)
            {
                if (!Postback && (Text != ActiveText || Primary != ActivePrimary || Secondary != ActiveSecondary))
                {
                    List<string> actOpts = CalcOpts(ActiveText, ActivePrimary, ActiveSecondary);
                    List<string> natOpts = CalcOpts(Text, Primary, Secondary);

                    if (Active)
                        opts.AddRange(actOpts);
                    else
                        opts.AddRange(natOpts);

                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("$(\"#{0}\").button({{{1}}})", ClientID, string.Join(",", opts));
                    sb.AppendFormat(".click(function(){{var opts;var act;if($(this).text()==\"{0}\"){{opts={{{1}}};act=true;}}else{{opts={{{2}}};act=false;}}$(this).button(\"option\",opts);$(\"#{3}_act\").val(act);return false;}});",
                        Text, string.Join(",", actOpts), string.Join(",", natOpts), UniqueID);
                    Script.AddStartupScript(this, ClientID + "_tog", sb.ToString());

                    Page.ClientScript.RegisterHiddenField(UniqueID + "_act", Active.ToString().ToLower());
                    return;
                }
                else
                {
                    if (Active)
                        opts.AddRange(CalcOpts(ActiveText, ActivePrimary, ActiveSecondary));
                    else
                        opts.AddRange(CalcOpts(Text, Primary, Secondary));
                }
            }
            else
            {
                opts.AddRange(CalcOpts(null, Primary, Secondary));
            }

            Script.AddStartupScript(this, ClientID, "button", opts);
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);

            if (Toggle && (HighlightActive || (Text == ActiveText && Primary == ActivePrimary && Secondary == ActiveSecondary)))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");
                if (Active)
                    writer.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");

                if (Postback)
                {
                    if (!string.IsNullOrEmpty(OnClientClick))
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, OnClientClick + ";" + Page.ClientScript.GetPostBackEventReference(this, "Clicked"));
                    else
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.ClientScript.GetPostBackEventReference(this, "Clicked"));
                }

                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
                writer.AddAttribute(HtmlTextWriterAttribute.For, ClientID);
                writer.RenderBeginTag(HtmlTextWriterTag.Label);
                if (Active)
                    writer.Write(ActiveText);
                else
                    writer.Write(Text);
                writer.RenderEndTag();
            }
            else
            {
                if (Postback)
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "submit");
                if (!string.IsNullOrEmpty(OnClientClick))
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, OnClientClick);
                writer.RenderBeginTag(HtmlTextWriterTag.Button);
                if (Active)
                    writer.Write(ActiveText);
                else
                    writer.Write(Text);
                writer.RenderEndTag();
            }
        }

        #endregion

        #region Postback Events

        /// <summary>
        /// Event Handler for Clicks
        /// </summary>
        public event EventHandler Click;
        /// <summary>
        /// Event Handler for Commands
        /// </summary>
        public event CommandEventHandler Command;

        /// <summary>
        /// When implemented by a class, enables a server control to process an event raised when a form is posted to the server.
        /// </summary>
        /// <param name="eventArgument">A <see cref="T:System.String"/> that represents an optional event argument to be passed to the event handler.</param>
        public void RaisePostBackEvent(string eventArgument)
        {
            if (Toggle) Active = !Active;
            if (Click != null)
                Click(this, new EventArgs());

            if (!string.IsNullOrEmpty(CommandName))
            {
                CommandEventArgs args = new CommandEventArgs(CommandName, CommandArgument);
                if (Command != null)
                    Command(this, args);

                RaiseBubbleEvent(this, args);
            }
        }

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
            if (postCollection[postDataKey + "_act"] != null)
            {
                if (postCollection[postDataKey + "_act"] != Active.ToString().ToLower())
                    Page.RegisterRequiresRaiseEvent(this);
            }
            else if (postCollection[postDataKey] != null)
                Page.RegisterRequiresRaiseEvent(this);
            return false;
        }

        /// <summary>
        /// When implemented by a class, signals the server control to notify the ASP.NET application that the state of the control has changed.
        /// </summary>
        public void RaisePostDataChangedEvent()
        {
            // Nothing To Do Here
        }

        #endregion

        #region Private

        private List<string> CalcOpts(string text, JButtonIcon primary, JButtonIcon secondary)
        {
            List<string> rst = new List<string>();

            if (!string.IsNullOrEmpty(text))
                rst.Add(string.Format("label:\"{0}\"", text));

            if (primary != JButtonIcon.None)
            {
                if (secondary != JButtonIcon.None)
                    rst.Add(string.Format("icons:{{primary:\"ui-icon-{0}\",secondary:\"ui-icon-{1}\"}}", Constants.IconStrings[(int)primary], Constants.IconStrings[(int)secondary]));
                else
                    rst.Add(string.Format("icons:{{primary:\"ui-icon-{0}\"}}", Constants.IconStrings[(int)primary]));
            }
            else if (secondary != JButtonIcon.None)
                rst.Add(string.Format("icons:{{secondary:\"ui-icon-{0}\"}}", Constants.IconStrings[(int)secondary]));

            return rst;
        }

        #endregion
    }
}
