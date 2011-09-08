using System;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace ESWCtrls.Graphic
{
    /// <summary>
    /// The base class for WebGfx Implementations
    /// </summary>
    /// <remarks>
    /// This class is for handling on the fly graphics. The most important part is the Image function,
    /// which any child class has to implement. This function should return the image that is generated on the fly.
    /// 
    /// For this class and its children to work you need to add the <see cref="WebGfxHandler">WebGfxHandler</see> to the application.
    /// </remarks>
    [ToolboxData("<{0}:WebGfx runat=\"server\"></{0}:WebGfx>")]
    public abstract class WebGfx : WebControl
    {
        /// <summary>
        /// Standard Constructor
        /// </summary>
        public WebGfx()
        {
            _guid = Guid.NewGuid().ToString("N");
            _saveTime = DateTime.MinValue;
        }

        #region Properties

        /// <summary>
        /// The time before the image should be removed, even if it hasn't been called.
        /// </summary>
        [Category("Data")]
        public TimeSpan ExpireSpan
        {
            get
            {
                if(ViewState["ExpireSpan"] == null)
                    return TimeSpan.FromMinutes(10);
                else
                    return (TimeSpan)ViewState["ExpireSpan"];
            }
            set { ViewState["ExpireSpan"] = value; }
        }

        /// <summary>
        /// The file name (without extension) that will be used by the image.
        /// </summary>
        /// <remarks>
        /// This is usually used to provide reaptable images rather than truly dynamic images.
        /// allowing the Clients web browser to cache the images, and saving in memory and
        /// execution time for the server.
        /// </remarks>
        [Category("Data"), DefaultValue("")]
        public virtual string FileName
        {
            get
            {
                if(ViewState["FileName"] == null)
                    return _guid;
                else
                    return (string)ViewState["FileName"];
            }
            set { ViewState["FileName"] = value; }
        }

        /// <summary>
        /// The image format that should be used to send the image in
        /// </summary>
        public virtual ImageFormat ImageFormat
        {
            get
            {
                if(ViewState["ImageFormat"] == null)
                    return ImageFormat.Gif;
                else
                    return (ImageFormat)ViewState["ImageFormat"];
            }
            set { ViewState["ImageFormat"] = value; }
        }

        /// <summary>
        /// The quality setting used for jpegs
        /// </summary>
        [Category("Data"), DefaultValue(100)]
        public long Quality
        {
            get
            {
                if(ViewState["Quality"] == null)
                    return 100;
                else
                    return (long)ViewState["Quality"];
            }
            set
            {
                if(value < 0)
                    ViewState["Quality"] = 0;
                else if(value > 100)
                    ViewState["Quality"] = 100;
                else
                    ViewState["Quality"] = value;
            }
        }

        /// <summary>
        /// The alternative text for the image if there is any problems with the image.
        /// </summary>
        [Category("Appearance"), DefaultValue("")]
        public string AltText
        {
            get
            {
                if(ViewState["AltText"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["AltText"];
            }
            set { ViewState["AltText"] = value; }
        }

        /// <summary>
        /// The title of the image, appears as a tooltip in most web browsers
        /// </summary>
        [Category("Appearance"), DefaultValue("")]
        public string Title
        {
            get
            {
                if(ViewState["Title"] == null)
                    return "";
                else
                    return (string)ViewState["Title"];
            }
            set { ViewState["Title"] = value; }
        }

        #endregion

        /// <summary>
        /// The dynamic image that the child class has generated
        /// </summary>
        public abstract System.Drawing.Image Image();

        /// <summary>
        /// Renders the image to the output
        /// </summary>
        /// <param name="writer">The writer to oupput to</param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            if(string.IsNullOrEmpty(Title) == false) writer.AddAttribute(HtmlTextWriterAttribute.Title, Title);
            if(string.IsNullOrEmpty(AltText) == false) writer.AddAttribute(HtmlTextWriterAttribute.Alt, AltText);
            _saveTime = DateTime.Now;
            writer.AddAttribute(HtmlTextWriterAttribute.Src, FileName + Extension);
            Context.Application["wgx_" + FileName] = this;
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
        }

        /// <summary>
        /// Checks to see if this object has expired
        /// </summary>
        /// <returns>True if it has expired</returns>
        public virtual bool CheckExpire()
        {
            if(DateTime.Now.Subtract(_saveTime) > ExpireSpan)
                return true;
            else
                return false;
        }

        /// <summary>
        /// This save the control at this point for the later recall
        /// </summary>
        /// <remarks>
        /// This is used for when your not using the class as a dynamic control but as just a way to recall dynamic images for
        /// a background image for other controls. using the filename + ".wgx" as the file.
        /// </remarks>
        protected virtual void Save()
        {
            _saveTime = DateTime.Now;
            Context.Application["wgx_" + FileName] = this;
        }

        /// <summary>
        /// The extension to use for the graphic, defaults to wgx
        /// </summary>
        public static string Extension
        {
            get
            {
                if(s_wgxExt == null)
                {
                    // Not using system.configuration as its a bit cumbersome, and doesn't work with iis7, and there is no equivalent for iis7
                    // that can be used everywhere, since they decided not to put the dll's in .net but with iis7 itself. which makes them almost
                    // completly useless to everyone. I don't think they're even in the test server.
                    XmlDocument doc = new XmlDocument();
                    doc.Load(HttpContext.Current.Server.MapPath("~/web.config"));

                    // look for iis6 and below section
                    XmlNodeList nodes = doc.SelectNodes("//system.web/httpHandlers/add[@type='ESWCtrls.Graphic.WebGfxHandler, ESWCtrls']");
                    if(nodes.Count == 0) // now check iis7
                        nodes = doc.SelectNodes("//system.webServer/handlers/add[@type='ESWCtrls.Graphic.WebGfxHandler, ESWCtrls']");

                    if(nodes.Count > 0)
                    {
                        XmlAttribute a = nodes[0].Attributes["path"];
                        if(a != null)
                            s_wgxExt = a.Value.Substring(1);
                    }
                    else
                    {
                        // Can't find them mark as unknown
                        s_wgxExt = "";
                    }

                }
                return s_wgxExt;
            }
        }

        #region Private

        private string _guid;
        private DateTime _saveTime;
        private static string s_wgxExt = ".wgx";

        #endregion
    }
}
