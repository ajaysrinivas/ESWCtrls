using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{
    /// <summary>
    /// Keeps the scroll position of a page after postback
    /// </summary>
    /// <remarks>
    /// Keeps the scroll position of a page after postback. Can dynamically told not to do this
    /// through IgnoreScroll function.
    /// </remarks>
    [ToolboxData("<{0}:PagePosition runat=\"server\" />")]
    public class PagePosition : WebControl, IPostBackDataHandler
    {
        /// <summary>
        /// Tells control not to repostion page after postback
        /// </summary>
        public void IgnoreScroll()
        {
            Position = 0;
        }

        /// <summary>
        /// Forces a specific postition for page scrolling
        /// </summary>
        public int Position
        {
            get;
            set;
        }

        #region Control events

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnPreRender(System.EventArgs e)
        {
            Script.AddResourceScript(Page, "jquery.js");

            if(Position > 0 && Page.IsPostBack)
                Script.AddStartupScript(this, ClientID + "_move", string.Format("$(window).scrollTop({0});", Position));

            Script.AddStartupScript(this, ClientID, string.Format("$(window).scroll(function() {{ $(\"#{0}\").val($(window).scrollTop()); }});", ClientID));
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, Position.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        #endregion

        #region Postback events

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
            if(postCollection[postDataKey] != null)
            {
                int pos = 0;
                if(int.TryParse(postCollection[postDataKey], out pos))
                    Position = pos;
            }

            return false;
        }

        /// <summary>
        /// When implemented by a class, signals the server control to notify the ASP.NET application that the state of the control has changed.
        /// </summary>
        public void RaisePostDataChangedEvent()
        {
            // No events are launched from this control
        }

        #endregion

    }
}
