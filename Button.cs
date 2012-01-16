using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace ESWCtrls
{
    using Graphic;
    using Internal;

    /// <summary>
    /// The control is a more advanced button than normal. Doing images, hovers, and toggling.
    /// </summary>
    /// <remarks>The button also does text over images by generating button images on the fly.</remarks>
    [DefaultProperty("Text"), ToolboxData("<{0}:Button runat=server></{0}:Button>")]
    public class Button : WebControl, IPostBackEventHandler, IPostBackDataHandler, IButtonControl
    {

        #region Appearance Properties

        /// <summary>
        /// Whether the button is generated and what type to use
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(GenerateStyle.None)]
        public GenerateStyle GenerateStyle
        {
            get
            {
                if (ViewState["GenerateStyle"] == null)
                    return GenerateStyle.None;
                else
                    return (GenerateStyle)ViewState["GenerateStyle"];
            }
            set { ViewState["GenerateStyle"] = value; }
        }

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
        /// The text to display when the button is active
        /// </summary>
        /// <remarks>When not set returns the normal text</remarks>
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
        /// The alternative text to display when an image is unavailable
        /// </summary>
        /// <remarks>Will default to text if its not been set</remarks>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string AlternateText
        {
            get
            {
                if (ViewState["AlternateText"] == null)
                    return Text;
                else
                    return (string)ViewState["AlternateText"];
            }
            set { ViewState["AlternateText"] = value; }
        }

        /// <summary>
        /// The image to display for the button
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(""), UrlProperty(), Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        public string ImageUrl
        {
            get
            {
                if (ViewState["ImageUrl"] == null)
                    return "";
                else
                    return (string)ViewState["ImageUrl"];
            }
            set { ViewState["ImageUrl"] = value; }
        }

        /// <summary>
        /// The image to display when the button is active
        /// </summary>
        /// <remarks>When not set returns the normal imageurl</remarks>
        [Bindable(true), Category("Appearance"), DefaultValue(""), UrlProperty(), Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        public string ActiveImageUrl
        {
            get
            {
                if (ViewState["ActiveImageUrl"] == null)
                    return ImageUrl;
                else
                    return (string)ViewState["ActiveImageUrl"];
            }
            set { ViewState["ActiveImageUrl"] = value; }
        }

        /// <summary>
        /// Sets the image alignment
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(ImageAlign.NotSet)]
        public ImageAlign ImageAlign
        {
            get
            {
                if (ViewState["ImageAlign"] == null)
                    return ImageAlign.NotSet;
                else
                    return (ImageAlign)ViewState["ImageAlign"];
            }
            set { ViewState["ImageAlign"] = value; }
        }

        /// <summary>
        /// The custom normal background image
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(""), UrlProperty(), Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        public string CustomNormalImageUrl
        {
            get
            {
                if(ViewState["CustomNormalImageUrl"] == null)
                    return "";
                else
                    return (string)ViewState["CustomNormalImageUrl"];
            }
            set { ViewState["CustomNormalImageUrl"] = value; }
        }

        /// <summary>
        /// The custom hover background image
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(""), UrlProperty(), Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        public string CustomHoverImageUrl
        {
            get
            {
                if(ViewState["CustomHoverImageUrl"] == null)
                    return CustomNormalImageUrl;
                else
                    return (string)ViewState["CustomHoverImageUrl"];
            }
            set { ViewState["CustomHoverImageUrl"] = value; }
        }

        /// <summary>
        /// The custom clicked background image
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(""), UrlProperty(), Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        public string CustomClickedImageUrl
        {
            get
            {
                if(ViewState["CustomClickedImageUrl"] == null)
                    return CustomNormalImageUrl;
                else
                    return (string)ViewState["CustomClickedImageUrl"];
            }
            set { ViewState["CustomClickedImageUrl"] = value; }
        }

        /// <summary>
        /// The custom disabled background image
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(""), UrlProperty(), Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        public string CustomDisabledImageUrl
        {
            get
            {
                if(ViewState["CustomDisabledImageUrl"] == null)
                    return CustomNormalImageUrl;
                else
                    return (string)ViewState["CustomDisabledImageUrl"];
            }
            set { ViewState["CustomDisabledImageUrl"] = value; }
        }

        /// <summary>
        /// Custom disabled overlay color
        /// </summary>
        [Bindable(true), Category("Appearance"),DefaultValue(typeof(Color), null)]
        public Color CustomDisabledOverlayColor
        {
            get
            {
                if(ViewState["CustomDisabledOverlayColor"] == null)
                    return Color.Empty;
                else
                    return (Color)ViewState["CustomDisabledOverlayColor"];
            }
            set { ViewState["CustomDisabledOverlayColor"] = value; }
        }

        #endregion

        #region Data Behaviour Properties

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
        /// If generating images whether to cache them
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(false)]
        public string CacheName
        {
            get
            {
                if (ViewState["CacheName"] == null)
                    return null;
                else
                    return (string)ViewState["CacheName"];
            }
            set { ViewState["CacheName"] = value; }
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
        /// The postback url for the button if different from the current page
        /// </summary>
        [Bindable(true), Category("behaviour"), DefaultValue("")]
        public string PostBackUrl
        {
            get
            {
                if (ViewState["PostBackUrl"] == null)
                    return "";
                else
                    return (string)ViewState["PostBackUrl"];
            }
            set { ViewState["PostBackUrl"] = value; }
        }

        /// <summary>
        /// The validation group for the button.
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue("")]
        public string ValidationGroup
        {
            get
            {
                if (ViewState["ValidationGroup"] == null)
                    return "";
                else
                    return (string)ViewState["ValidationGroup"];
            }
            set { ViewState["ValidationGroup"] = value; }
        }

        #endregion

        #region Other Properties

        /// <summary>
        /// Whether the button cause validation
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(false)]
        public bool CausesValidation
        {
            get
            {
                if (ViewState["CausesValidation"] == null)
                    return false;
                else
                    return (bool)ViewState["CausesValidation"];
            }
            set { ViewState["CausesValidation"] = value; }
        }

        #endregion

        #region Render

        /// <summary>
        /// The rendering of a normal text button
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        private void RenderButton(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "submit");
            string str = Text;
            if (Active)
                str = ActiveText;

            writer.AddAttribute(HtmlTextWriterAttribute.Value, str);
        }

        /// <summary>
        /// The rendering of an image button
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        private void RenderImageButton(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "image");
            if (ImageAlign != ImageAlign.NotSet) writer.AddAttribute(HtmlTextWriterAttribute.Align, ImageAlign.ToString());

            string altText = "";
            string curImg = ImageUrl;
            altText = AlternateText;
            if (Active)
            {
                curImg = ActiveImageUrl;
                altText = ActiveText;
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Src, Page.ResolveUrl(curImg));
            if (!string.IsNullOrEmpty(altText))
                writer.AddAttribute(HtmlTextWriterAttribute.Alt, altText);

        }

        /// <summary>
        /// Rendering of a generated image button
        /// </summary>
        /// <param name="writer">The writer to render the html to</param>
        private void RenderGeneratedButton(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "image");
            if (ImageAlign != ImageAlign.NotSet)
                writer.AddAttribute(HtmlTextWriterAttribute.Align, ImageAlign.ToString());

            Size size = new Size();
            bool writeWidth = true;
            bool writeHeight = true;

            if (!this.Width.IsEmpty)
            {
                size.Width = (int)this.Width.Value;
                writeWidth = false;
            }
            else if (Style["width"] != null)
            {
                int width;
                if (int.TryParse(Style["width"].Replace("px", ""), out width))
                    size.Width = width;
                writeWidth = false;
            }

            if (!this.Height.IsEmpty)
            {
                size.Height = (int)this.Height.Value;
                writeHeight = false;
            }
            else if (Style["height"] != null)
            {
                int height;
                if (int.TryParse(Style["height"].Replace("px", ""), out height))
                    size.Height = height;
                writeHeight = false;
            }

            Color fColor = Color.Black;
            if (GenerateStyle == GenerateStyle.Vista)
                fColor = Color.White;

            if (!ForeColor.IsEmpty)
                fColor = ForeColor;
            else if (Style["color"] != null)
            {
                ColorConverter conv = new ColorConverter();
                fColor = (Color)conv.ConvertFrom(Style["color"]);
            }

            string altText = AlternateText;
            string text = Text;
            string imgPath = string.Empty;

            if(Active)
            {
                if(!string.IsNullOrEmpty(ActiveImageUrl))
                    imgPath = Page.Server.MapPath(ActiveImageUrl);

                if(string.IsNullOrEmpty(altText))
                    altText = ActiveText;

                    text = ActiveText;
            }
            else
            {
                if(!string.IsNullOrEmpty(ImageUrl))
                    imgPath = Page.Server.MapPath(ImageUrl);

                if(string.IsNullOrEmpty(altText))
                    altText = Text;
            }

            Font textfont = Util.ConvertFont(Font);
            if (this.IsEnabled)
            {

                GenerateImage normImg = new GenerateImage(GenerateStyle, GenerateImage.ButtonState.Normal, size, text, textfont, fColor, imgPath, CacheName,Page.Server.MapPath(CustomNormalImageUrl), Color.Empty);
                writer.AddAttribute(HtmlTextWriterAttribute.Src, normImg.FullPath);
                writer.AddAttribute("onmouseout", "this.src='" + normImg.FullPath + "';");

                GenerateImage hovImg = new GenerateImage(GenerateStyle, GenerateImage.ButtonState.Hover, size, text, textfont, fColor, imgPath, CacheName, Page.Server.MapPath(CustomHoverImageUrl), Color.Empty);
                writer.AddAttribute("onmouseover", "this.src='" + hovImg.FullPath + "';");

                GenerateImage clickImg = new GenerateImage(GenerateStyle, GenerateImage.ButtonState.Clicked, size, text, textfont, fColor, imgPath, CacheName, Page.Server.MapPath(CustomClickedImageUrl), Color.Empty);
                writer.AddAttribute("onmousedown", "this.src='" + clickImg.FullPath + "';");

                if (!string.IsNullOrEmpty(OnClientClick))
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, OnClientClick);

                size = normImg.Size;
            }
            else
            {
                GenerateImage disImg = new GenerateImage(GenerateStyle, GenerateImage.ButtonState.Disabled, size, text, textfont, fColor, imgPath, CacheName,  Page.Server.MapPath(CustomDisabledImageUrl), CustomDisabledOverlayColor);
                writer.AddAttribute(HtmlTextWriterAttribute.Src, disImg.FullPath);

                size = disImg.Size;
            }

            if (writeWidth)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, size.Width.ToString() + "px");

            if (writeHeight)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Height, size.Height.ToString() + "px");

            if (!string.IsNullOrEmpty(altText))
                writer.AddAttribute(HtmlTextWriterAttribute.Alt, altText);
        }


        #endregion

        #region Control events

        /// <summary>
        /// The pre rendering of the button
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnPreRender(System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(ImageUrl) || GenerateStyle != GenerateStyle.None)
                Page.RegisterRequiresPostBack(this);

            base.OnPreRender(e);
        }

        /// <summary>
        /// The actual rendering of the button
        /// </summary>
        /// <param name="writer">The writer to write the button to</param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);

            if (!this.IsEnabled)
                writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");


            if (GenerateStyle == GenerateStyle.None)
            {
                if (!string.IsNullOrEmpty(OnClientClick))
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, OnClientClick);

                if (string.IsNullOrEmpty(ImageUrl))
                    RenderButton(writer);
                else
                    RenderImageButton(writer);
            }
            else
                RenderGeneratedButton(writer);

            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        #endregion

        #region Post back events

        private int _x;
        private int _y;

        /// <summary>
        /// Event Handler for Click on The Image Gives Position where clicked
        /// </summary>
        public event ImageClickEventHandler ImageClick;
        /// <summary>
        /// Event Handler for Clicks
        /// </summary>
        public event EventHandler Click;
        /// <summary>
        /// Event Handler for Commands
        /// </summary>
        public event CommandEventHandler Command;

        /// <summary>
        /// Handles the post back events
        /// </summary>
        /// <param name="eventArgument">The arguments for the post back event</param>
        public void RaisePostBackEvent(string eventArgument)
        {
            if (Toggle) Active = !Active;
            if (Click != null)
                Click(this, new EventArgs());

            if (!string.IsNullOrEmpty(ImageUrl) || GenerateStyle != GenerateStyle.None)
            {
                if (ImageClick != null)
                    ImageClick(this, new ImageClickEventArgs(_x, _y));
            }

            if (!string.IsNullOrEmpty(CommandName))
            {
                CommandEventArgs args = new CommandEventArgs(CommandName, CommandArgument);
                if (Command != null)
                    Command(this, args);

                RaiseBubbleEvent(this, args);

            }
        }

        #endregion

        #region Post Data events

        /// <summary>
        /// Post back data handling for image clicks
        /// </summary>
        /// <param name="postDataKey">The data key</param>
        /// <param name="postCollection">The posted information</param>
        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            if (postCollection[UniqueID + ".x"] != null)
            {
                _x = int.Parse(postCollection[UniqueID + ".x"]);
                _y = int.Parse(postCollection[UniqueID + ".y"]);
                Page.RegisterRequiresRaiseEvent(this);
            }
            else if(postCollection[UniqueID] != null)
                Page.RegisterRequiresRaiseEvent(this);
            return false;
        }

        /// <summary>
        /// Handled for Interface but does no work
        /// </summary>
        public void RaisePostDataChangedEvent()
        {
            //Nothing to do here
        }

        #endregion

        private class GenerateImage : WebGfx
        {

            public enum ButtonState
            {
                Normal,
                Hover,
                Clicked,
                Disabled
            }

            private GenerateStyle _genStyle;
            private ButtonState _btnState;
            private Size _size;
            private Size _defSize;
            private Size _txtSize;
            private Size _txtMseSize;
            private Color _foreColor;
            private Size _imgSize;
            private string _text;
            private Font _textFont;
            private string _foreImagePath;
            private string _cacheName;

            private string _customBgd;
            private Color _customDisOverlay;


            public GenerateImage(GenerateStyle generateStyle, ButtonState buttonState, Size size, string text, Font font, Color foreColor, string foreImagePath, string cacheName, string customBgdPath, Color customDisOverlay)
            {
                _genStyle = generateStyle;
                _btnState = buttonState;
                _text = text;
                _textFont = font;
                _foreColor = foreColor;
                _foreImagePath = foreImagePath;
                _defSize = size;
                _cacheName = cacheName;
                _customBgd = customBgdPath;
                _customDisOverlay = customDisOverlay;

                if(generateStyle == GenerateStyle.Custom && string.IsNullOrEmpty(_customBgd))
                    _genStyle = GenerateStyle.XP;

                calculateSize();

                if (!size.IsEmpty)
                {
                    if (size.Width > 0)
                        _size.Width = size.Width;

                    if (size.Height > 0)
                        _size.Height = size.Height;
                }

                Image();
                ExpireSpan = TimeSpan.FromMinutes(20);
                Save();
            }
            
            public override System.Drawing.Imaging.ImageFormat ImageFormat
            {
                get { return System.Drawing.Imaging.ImageFormat.Png; }
            }

            public override string FileName
            {
                get
                {
                    if (ViewState["FileName"] == null)
                    {
                        string name = _genStyle.ToString() + _btnState;

                        if(_genStyle == GenerateStyle.Custom)
                            name += _customBgd.Replace(" ", "");

                        if (!string.IsNullOrEmpty(_text))
                        {
                            if (!string.IsNullOrEmpty(_foreImagePath))
                                name += "_txt-img_" + _text.Replace(" ", "") + "-" + _foreImagePath.Replace(" ", "").Replace("\\", "").Replace(":", "");
                            else
                                name += "_txt_" + _text.Replace(" ", "");
                        }
                        else if (!string.IsNullOrEmpty(_foreImagePath))
                            name += "_img_" + _foreImagePath.Replace(" ", "").Replace("\\", "").Replace(":", "");
                        else
                            name += "_blank";

                        if (_genStyle == GenerateStyle.XP && !(_foreColor == Color.Black))
                            name += "_" + _foreColor.ToString();

                        if (_genStyle == GenerateStyle.Vista && !(_foreColor == Color.White))
                            name += "_" + _foreColor.ToString();

                        if (_textFont.Name != "Verdana")
                            name += "_" + _textFont.Name;

                        if (_textFont.Unit != GraphicsUnit.Pixel)
                            name += "_" + _textFont.Unit.ToString();

                        if (_textFont.Size != 12)
                            name += "_" + _textFont.Size.ToString();

                        if (!_defSize.IsEmpty)
                            name += "_" + _defSize.Width.ToString() + "x" + _defSize.Height.ToString();

                        SHA1Managed hash = new SHA1Managed();
                        byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(name));
                        ViewState["FileName"] = Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "a");
                    }

                    return (string)ViewState["FileName"];
                }
            }

            public override System.Drawing.Image Image()
            {
                string imagePath = Path.GetTempPath() + _cacheName + "\\" + FileName + ".png";
                if (File.Exists(imagePath))
                    return new Bitmap(imagePath);

                System.Drawing.Image bgdImage = Properties.Resources.xp_normal;
                switch (_genStyle)
                {
                    case GenerateStyle.XP:
                        switch (_btnState)
                        {
                            case ButtonState.Hover:
                                bgdImage = Properties.Resources.xp_hover;
                                break;
                            case ButtonState.Clicked:
                                bgdImage = Properties.Resources.xp_clicked;
                                break;
                            case ButtonState.Disabled:
                                bgdImage = Properties.Resources.xp_disabled;
                                break;
                        }
                        break;
                    case GenerateStyle.XPSilver:
                        switch (_btnState)
                        {
                            case ButtonState.Normal:
                                bgdImage = Properties.Resources.xpsil_normal;
                                break;
                            case ButtonState.Hover:
                                bgdImage = Properties.Resources.xpsil_hover;
                                break;
                            case ButtonState.Clicked:
                                bgdImage = Properties.Resources.xpsil_clicked;
                                break;
                            case ButtonState.Disabled:
                                bgdImage = Properties.Resources.xpsil_disabled;
                                break;
                        }
                        break;
                    case GenerateStyle.Vista:
                        switch (_btnState)
                        {
                            case ButtonState.Normal:
                                bgdImage = Properties.Resources.vista_normal;
                                break;
                            case ButtonState.Hover:
                                bgdImage = Properties.Resources.vista_hover;
                                break;
                            case ButtonState.Clicked:
                                bgdImage = Properties.Resources.vista_clicked;
                                break;
                            case ButtonState.Disabled:
                                bgdImage = Properties.Resources.vista_disabled;
                                break;
                        }
                        break;
                    case GenerateStyle.Oxygen:
                        switch (_btnState)
                        {
                            case ButtonState.Normal:
                                bgdImage = Properties.Resources.oxy_normal;
                                break;
                            case ButtonState.Hover:
                                bgdImage = Properties.Resources.oxy_hover;
                                break;
                            case ButtonState.Clicked:
                                bgdImage = Properties.Resources.oxy_clicked;
                                break;
                            case ButtonState.Disabled:
                                bgdImage = Properties.Resources.oxy_disabled;
                                break;
                        }
                        break;
                    case GenerateStyle.Calypso:
                        switch (_btnState)
                        {
                            case ButtonState.Normal:
                                bgdImage = Properties.Resources.cal_normal;
                                break;
                            case ButtonState.Hover:
                                bgdImage = Properties.Resources.cal_hover;
                                break;
                            case ButtonState.Clicked:
                                bgdImage = Properties.Resources.cal_clicked;
                                break;
                            case ButtonState.Disabled:
                                bgdImage = Properties.Resources.cal_disabled;
                                break;
                        }
                        break;
                    case GenerateStyle.Custom:
                        bgdImage = new Bitmap(_customBgd);
                        break;
                }


                Bitmap foreImg = null;

                if (!string.IsNullOrEmpty(_foreImagePath))
                    foreImg = new Bitmap(_foreImagePath);

                Bitmap rstImage = new Bitmap(_size.Width, _size.Height);
                using (Graphics gfx = Graphics.FromImage(rstImage))
                {
                    gfx.CompositingQuality = CompositingQuality.HighQuality;
                    gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gfx.SmoothingMode = SmoothingMode.HighQuality;
                    gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                    if (_size.Width == 22 && Height == 22)
                        gfx.DrawImageUnscaled(bgdImage, 0, 0);
                    else
                    {
                        //The four corners
                        gfx.DrawImage(bgdImage, 0, 0, new Rectangle(0, 0, 4, 4), GraphicsUnit.Pixel);
                        gfx.DrawImage(bgdImage, _size.Width - 4, 0, new Rectangle(18, 0, 4, 4), GraphicsUnit.Pixel);
                        gfx.DrawImage(bgdImage, 0, _size.Height - 4, new Rectangle(0, 18, 4, 4), GraphicsUnit.Pixel);
                        gfx.DrawImage(bgdImage, _size.Width - 4, _size.Height - 4, new Rectangle(18, 18, 4, 4), GraphicsUnit.Pixel);
                        //The extensions for the width
                        gfx.DrawImage(bgdImage, new Rectangle(4, 0, _size.Width - 8, 4), new Rectangle(4, 0, 14, 4), GraphicsUnit.Pixel);
                        gfx.DrawImage(bgdImage, new Rectangle(4, _size.Height - 4, _size.Width - 8, 4), new Rectangle(4, 18, 14, 4), GraphicsUnit.Pixel);
                        //The extensions for the height
                        gfx.DrawImage(bgdImage, new Rectangle(0, 4, 4, _size.Height - 8), new Rectangle(0, 4, 4, 14), GraphicsUnit.Pixel);
                        gfx.DrawImage(bgdImage, new Rectangle(_size.Width - 4, 4, 4, _size.Height - 8), new Rectangle(18, 4, 4, 14), GraphicsUnit.Pixel);
                        //The center
                        gfx.DrawImage(bgdImage, new Rectangle(4, 4, _size.Width - 8, _size.Height - 8), new Rectangle(4, 4, 14, 14), GraphicsUnit.Pixel);
                    }

                    int offsetX = (int)(_size.Width - (_imgSize.Width + _txtSize.Width)) / 2;

                    if (!string.IsNullOrEmpty(_foreImagePath))
                    {
                        if (!string.IsNullOrEmpty(_text))
                            offsetX += 4;

                        Point pos = new Point(offsetX + 3, (int)(_size.Height - foreImg.Height) / 2);

                        gfx.DrawImage(foreImg, pos.X, pos.Y, foreImg.Width, foreImg.Height);
                        if (_btnState == ButtonState.Disabled)
                        {
                            Rectangle area = new Rectangle(pos.X - 1, pos.Y - 1, foreImg.Width + 1, foreImg.Height + 1);
                            if (_genStyle == GenerateStyle.Vista)
                                gfx.FillRectangle(new SolidBrush(Color.FromArgb(165, 79, 79, 79)), area);
                            else
                                gfx.FillRectangle(new SolidBrush(Color.FromArgb(165, 245, 244, 234)), area);
                        }
                    }

                    if (!string.IsNullOrEmpty(_text))
                    {
                        if (!string.IsNullOrEmpty(_foreImagePath))
                            offsetX += (_imgSize.Width - 7);

                        Point pos = new Point(offsetX + 6, (int)(_size.Height - _txtMseSize.Height) / 2);


                        if (_btnState == ButtonState.Disabled)
                        {
                            if (_genStyle == GenerateStyle.Vista)
                            {
                                gfx.DrawString(_text, _textFont, new SolidBrush(Color.DarkGray), pos.X - 0.5f, pos.Y - 0.5f);
                                gfx.DrawString(_text, _textFont, new SolidBrush(Color.White), pos.X + 0.5f, pos.Y + 0.5f);
                                gfx.DrawString(_text, _textFont, new SolidBrush(Color.LightGray), pos);
                            }
                            else
                            {
                                gfx.DrawString(_text, _textFont, new SolidBrush(Color.DarkGray), pos.X - 0.5f, pos.Y - 0.5f);
                                gfx.DrawString(_text, _textFont, new SolidBrush(Color.White), pos.X + 0.5f, pos.Y + 0.5f);
                                gfx.DrawString(_text, _textFont, new SolidBrush(Color.Black), pos);
                            }

                            Rectangle area = new Rectangle(pos.X - 1, pos.Y - 1, _txtMseSize.Width + 1, _txtMseSize.Height + 1);
                            if (_genStyle == GenerateStyle.XP || _genStyle == GenerateStyle.XPSilver)
                                gfx.FillRectangle(new SolidBrush(Color.FromArgb(165, 245, 244, 234)), area);
                            else if(_genStyle == GenerateStyle.Custom)
                                gfx.FillRectangle(new SolidBrush(_customDisOverlay), area);
                            else
                                gfx.FillRectangle(new SolidBrush(Color.FromArgb(165, 79, 79, 79)), area);
                        }
                        else
                            gfx.DrawString(_text, _textFont, new SolidBrush(_foreColor), pos);
                    }
                }

                if (!string.IsNullOrEmpty(_cacheName))
                {
                    if (!Directory.Exists(Path.GetTempPath() + _cacheName))
                        Directory.CreateDirectory(Path.GetTempPath() + _cacheName);

                    rstImage.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
                }
                return rstImage;
            }

            private void calculateSize()
            {
                Size minSize = new Size(22, 22);
                Size padding = new Size(6, 6);
                Size txtPadding = new Size(12, 6);

                string imagePath = Path.GetTempPath() + "\\" + _cacheName + "\\" + FileName + ".png";
                if (File.Exists(imagePath))
                {
                    Bitmap img = new Bitmap(imagePath);
                    _size = img.Size;
                    return;
                }

                //Calc Text Size
                if (string.IsNullOrEmpty(_text))
                    _txtSize = new Size();
                else
                {
                    Bitmap img = new Bitmap(1, 1);
                    using (Graphics gfxs = Graphics.FromImage(img))
                    {
                        _txtMseSize = gfxs.MeasureString(_text, _textFont).ToSize();
                        _txtSize = _txtMseSize + txtPadding;
                        if (_txtSize.Width < minSize.Width)
                            _txtSize.Width = minSize.Width;

                        if (_txtSize.Height < minSize.Height)
                            _txtSize.Height = minSize.Height;
                    }
                }

                //Calc foreImage Size
                if (string.IsNullOrEmpty(_foreImagePath))
                    _imgSize = new Size();
                else
                {
                    Bitmap img = new Bitmap(_foreImagePath);
                    _imgSize = img.Size + padding;
                    if (_imgSize.Width < minSize.Width)
                        _imgSize.Width = minSize.Width;

                    if (_imgSize.Height < minSize.Height)
                        _imgSize.Height = minSize.Height;
                }

                //Calculate overall size
                if (!_txtSize.IsEmpty)
                {
                    if (!_imgSize.IsEmpty)
                    {
                        _txtSize.Width -= padding.Width / 2;
                        _size.Width = _txtSize.Width + _imgSize.Width;

                        if (_txtSize.Height > _imgSize.Height)
                            _size.Height = _txtSize.Height;
                        else
                            _size.Height = _imgSize.Height;
                    }
                    else
                        _size = _txtSize;
                }
                else
                {
                    if (!_imgSize.IsEmpty)
                        _size = _imgSize;
                    else
                        _size = minSize;
                }
            }

            public Size Size
            {
                get { return _size; }
            }

        }

    }
}
