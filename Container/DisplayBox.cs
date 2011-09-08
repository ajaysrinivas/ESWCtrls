using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{
    using Graphic;
    using Internal;

    /// <summary>
    /// Use for rounded boxes, drop shadow boxes or outer glow boxes or a mixture of the three
    /// </summary>
    [ToolboxData("<{0}:displaybox runat=\"server\"></{0}:displaybox>"), ParseChildren(true), PersistChildren(false)]
    public class DisplayBox : WebControl
    {
        #region Appearance Properties

        #region Radius

        /// <summary>
        /// Top Left Radius for rounded corner
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(int), null), NotifyParentProperty(true)]
        public int RadiusTopLeft
        {
            get
            {
                if(ViewState["TopLeft"] == null)
                    return 0;
                else
                    return (int)ViewState["TopLeft"];
            }
            set
            {
                if(value == 0)
                    ViewState.Remove("TopLeft");
                else
                    ViewState["TopLeft"] = value;
            }
        }

        /// <summary>
        /// Top Right Radius for rounded corner
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(int), null), NotifyParentProperty(true)]
        public int RadiusTopRight
        {
            get
            {
                if(ViewState["TopRight"] == null)
                    return 0;
                else
                    return (int)ViewState["TopRight"];
            }
            set
            {
                if(value == 0)
                    ViewState.Remove("TopRight");
                else
                    ViewState["TopRight"] = value;
            }
        }

        /// <summary>
        /// Bottom Left Radius for rounded corner
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(int), null), NotifyParentProperty(true)]
        public int RadiusBottomLeft
        {
            get
            {
                if(ViewState["BottomLeft"] == null)
                    return 0;
                else
                    return (int)ViewState["BottomLeft"];
            }
            set
            {
                if(value == 0)
                    ViewState.Remove("BottomLeft");
                else
                    ViewState["BottomLeft"] = value;
            }
        }

        /// <summary>
        /// Bottom Right Radius for rounded corner
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(int), null), NotifyParentProperty(true)]
        public int RadiusBottomRight
        {
            get
            {
                if(ViewState["BottomRight"] == null)
                    return 0;
                else
                    return (int)ViewState["BottomRight"];
            }
            set
            {
                if(value == 0)
                    ViewState.Remove("BottomRight");
                else
                    ViewState["BottomRight"] = value;
            }
        }

        /// <summary>
        /// Radius for rounded corners
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(int), null), NotifyParentProperty(true)]
        public int RadiusCorner
        {
            get { return Math.Max(RadiusTopLeft, Math.Max(RadiusTopRight, Math.Max(RadiusBottomLeft, RadiusBottomRight))); }
            set
            {
                RadiusTopLeft = value;
                RadiusTopRight = value;
                RadiusBottomLeft = value;
                RadiusBottomRight = value;
            }
        }

        #endregion

        /// <summary>
        /// The details of the drop shadow
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(typeof(DropShadow), null), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public DropShadow DropShadow
        {
            get
            {
                if(_dropShadow == null)
                {
                    _dropShadow = new DropShadow();
                    if(IsTrackingViewState) ((IStateManager)_dropShadow).TrackViewState();
                }

                return _dropShadow;
            }
        }

        /// <summary>
        /// The details of the outer glow
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(typeof(DropShadow), null), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public OuterGlow OuterGlow
        {
            get
            {
                if(_outerGlow == null)
                {
                    _outerGlow = new OuterGlow();
                    if(IsTrackingViewState) ((IStateManager)_outerGlow).TrackViewState();
                }

                return _outerGlow;
            }
        }

        /// <summary>
        /// The width of the border to use
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(int), null), NotifyParentProperty(true)]
        public new int BorderWidth
        {
            get
            {
                if(ViewState["BorderWidth"] == null)
                    return 0;
                else
                    return (int)ViewState["BorderWidth"];
            }
            set
            {
                if(value == 0)
                    ViewState.Remove("BorderWidth");
                else
                    ViewState["BorderWidth"] = value;
            }
        }

        /// <summary>
        /// The color of the border to use
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Color), null), NotifyParentProperty(true), TypeConverter(typeof(WebColorConverter))]
        public override Color BorderColor
        {
            get
            {
                if(ViewState["BorderColor"] == null)
                    return Color.Empty;
                else
                    return (Color)ViewState["BorderColor"];
            }
            set
            {
                if(value == Color.Transparent)
                    ViewState.Remove("BorderColor");
                else
                    ViewState["BorderColor"] = value;
            }
        }

        /// <summary>
        /// The color of the header
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Color), null), NotifyParentProperty(true), TypeConverter(typeof(WebColorConverter))]
        public Color HeaderColor
        {
            get
            {
                if(ViewState["HeaderColor"] == null)
                    return Color.Empty;
                else
                    return (Color)ViewState["HeaderColor"];
            }
            set
            {
                if(value == Color.Transparent)
                    ViewState.Remove("HeaderColor");
                else
                    ViewState["HeaderColor"] = value;
            }
        }

        /// <summary>
        /// The color of the content part
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Color), null), NotifyParentProperty(true), TypeConverter(typeof(WebColorConverter))]
        public Color ContentColor
        {
            get
            {
                if(ViewState["ContentColor"] == null)
                    return Color.Empty;
                else
                    return (Color)ViewState["ContentColor"];
            }
            set
            {
                if(value == Color.Transparent)
                    ViewState.Remove("ContentColor");
                else
                    ViewState["ContentColor"] = value;
            }
        }

        /// <summary>
        /// he color to use for IE 6 and below, if none is specified will default to square simple boxes
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Color), ""), NotifyParentProperty(true), TypeConverter(typeof(WebColorConverter))]
        public Color IEFixColor
        {
            get
            {
                if(ViewState["IEFixColor"] == null)
                    return Color.Empty;
                else
                    return (Color)ViewState["IEFixColor"];
            }
            set
            {
                if(value == Color.Transparent)
                    ViewState.Remove("IEFixColor");
                else
                    ViewState["IEFixColor"] = value;
            }
        }

        /// <summary>
        /// The style to apply to the header of the control
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(typeof(PaddingStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public PaddingStyle HeaderStyle
        {
            get
            {
                if(_headerStyle == null)
                {
                    _headerStyle = new PaddingStyle();
                    if(IsTrackingViewState)
                        ((IStateManager)_headerStyle).TrackViewState();
                }
                return _headerStyle;
            }
        }

        /// <summary>
        /// The style to apply to the content of the control
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(typeof(PaddingStyle), null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty), NotifyParentProperty(true)]
        public PaddingStyle ContentStyle
        {
            get
            {
                if(_contentStyle == null)
                {
                    _contentStyle = new PaddingStyle();
                    if(IsTrackingViewState)
                        ((IStateManager)_contentStyle).TrackViewState();
                }
                return _contentStyle;
            }
        }

        #region Hiding Unused

        ///
        public override System.Web.UI.WebControls.BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
        }

        ///
        public override System.Drawing.Color BackColor
        {
            get { return base.BackColor; }
        }

        #endregion

        #endregion

        #region Data Properties

        /// <summary>
        /// The contents of the box
        /// </summary>
        [Browsable(false), PersistenceMode(PersistenceMode.InnerProperty), TemplateInstance(TemplateInstance.Single)]
        public ITemplate Content
        {
            get { return _contentTemplate; }
            set
            {
                if(!DesignMode & _contentTemplate != null)
                    throw new InvalidOperationException("Could not set contents template on control " + ID);

                _contentTemplate = value;
                if(_contentTemplate != null)
                    CreateContents();
            }
        }

        /// <summary>
        /// The header of the box
        /// </summary>
        [Browsable(false), PersistenceMode(PersistenceMode.InnerProperty), TemplateInstance(TemplateInstance.Single)]
        public ITemplate Header
        {
            get { return _headerTemplate; }
            set
            {
                if(!DesignMode & _headerTemplate != null)
                    throw new InvalidOperationException("Could not set headers template on control " + ID);

                _headerTemplate = value;
                if(_headerTemplate != null)
                    CreateHeader();
            }
        }

        /// <summary>
        /// Replaces the default controls property to have a better handle on it
        /// </summary>
        public override sealed ControlCollection Controls
        {
            get { return base.Controls; }
        }

        /// <summary>
        /// whether to cache the generated images and under what name
        /// </summary>
        [Bindable(true), Category("Behaviour"), DefaultValue(typeof(string), null), NotifyParentProperty(true)]
        public string CacheName
        {
            get
            {
                if(ViewState["CacheName"] == null)
                    return null;
                else
                    return (string)ViewState["CacheName"];
            }
            set { ViewState["CacheName"] = value; }
        }

        #endregion

        #region Control Events

        ///
        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            if(_headerTemplateContainer == null)
            {
                _headerTemplateContainer = new Control();
                ChildControls.AddInternal(_headerTemplateContainer);
            }

            if(_contentTemplateContainer == null)
            {
                _contentTemplateContainer = new Control();
                ChildControls.AddInternal(_contentTemplateContainer);
            }
        }

        ///
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            CalculateBoxes();
            HttpBrowserCapabilities br = Page.Request.Browser;

            if(DropShadow.ShadowColor.IsEmpty && OuterGlow.GlowColor.IsEmpty)
            {
                if(RadiusCorner > 0)
                {
                    if(br.Browser == "IE" && br.MajorVersion < 7)
                    {
                        if(IEFixColor.IsEmpty)
                            RenderCSSVersion(writer, null, null);
                        else
                            RenderImgVersion(writer);
                    }
                    else if(br.Browser == "Firefox" && br.MajorVersion >= 3)
                        RenderCSSVersion(writer, "-moz-border-radius-{0}{1}", "-moz-box-shadow");
                    else if(br.Browser.Contains("Safari") && Page.Request.UserAgent.Contains("Version/4"))
                        RenderCSSVersion(writer, "-webkit-border-{0}-{1}-radius", "-webkit-box-shadow");
                    else
                        RenderImgVersion(writer);
                }
                else
                    RenderCSSVersion(writer, null, null);
            }
            else if(br.Browser == "IE" && br.MajorVersion < 7)
            {
                if(IEFixColor.IsEmpty)
                    RenderCSSVersion(writer, null, null);
                else
                    RenderImgVersion(writer);
            }
            else if(br.Browser == "Firefox" && (br.MajorVersion > 3 || (br.MajorVersion == 3 && br.MinorVersion >= 0.5)))
                RenderCSSVersion(writer, "-moz-border-radius-{0}{1}", "-moz-box-shadow");
            else if(br.Browser.Contains("Safari") && br.MajorVersion > 3)
                RenderCSSVersion(writer, "-webkit-border-{0}-{1}-radius", "-webkit-box-shadow");
            else
                RenderImgVersion(writer);
        }

        ///
        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
            Util.addStyleSheet("DispBox.css", "DispBoxCSS2", Page, this);
        }

        #endregion

        #region protected

        /// <summary>
        /// The contents header container
        /// </summary>
        [Browsable(false)]
        protected Control ContentTemplateContainer
        {
            get
            {
                if(_contentTemplateContainer == null)
                {
                    _contentTemplateContainer = new Control();
                    ChildControls.AddInternal(_contentTemplateContainer);
                }

                return _contentTemplateContainer;
            }
        }

        /// <summary>
        /// The headers template container
        /// </summary>
        [Browsable(false)]
        protected Control HeaderTemplateContainer
        {
            get
            {
                if(_headerTemplateContainer == null)
                {
                    _headerTemplateContainer = new Control();
                    ChildControls.AddInternal(_headerTemplateContainer);
                }

                return _headerTemplateContainer;
            }
        }

        ///
        protected override sealed System.Web.UI.ControlCollection CreateControlCollection()
        {
            return new LimitCollection(this, 2);
        }

        #endregion

        #region Private

        #region Rendering

        private void RenderCSSVersion(HtmlTextWriter writer, string prefixFormat, string shadowPrefix)
        {
            _padRect.X -= Math.Max(0, DropShadow.XOffset) + Math.Max(1, DropShadow.Blend);
            _padRect.Y -= Math.Max(0, DropShadow.YOffset);
            _padRect.Height -= Math.Max(0, DropShadow.YOffset);

            AddAttributesToRender(writer);
            string css = "ESW_DispBox";
            if(!string.IsNullOrEmpty(CssClass)) css += " " + CssClass;
            writer.AddAttribute(HtmlTextWriterAttribute.Class, css);

            if(!DropShadow.ShadowColor.IsEmpty)
            {
                writer.AddStyleAttribute(shadowPrefix, string.Format("{0}px {1}px {2}px {3}", DropShadow.XOffset, DropShadow.YOffset, DropShadow.Blend, ColorTranslator.ToHtml(DropShadow.ShadowColor)));
                if(DropShadow.YOffset > 0)
                    writer.AddStyleAttribute(HtmlTextWriterStyle.MarginBottom, string.Format("{0}px", DropShadow.YOffset + DropShadow.Blend));
                else if(DropShadow.YOffset < 0)
                    writer.AddStyleAttribute(HtmlTextWriterStyle.MarginTop, string.Format("{0}px", (0 - DropShadow.YOffset) + DropShadow.Blend));
                if(DropShadow.XOffset > 0)
                    writer.AddStyleAttribute(HtmlTextWriterStyle.MarginRight, string.Format("{0}px", DropShadow.XOffset + DropShadow.Blend));
                else if(DropShadow.XOffset < 0)
                    writer.AddStyleAttribute(HtmlTextWriterStyle.MarginLeft, string.Format("{0}px", (0 - DropShadow.XOffset) + DropShadow.Blend));
            }
            if(!string.IsNullOrEmpty(prefixFormat) && RadiusCorner > 0)
            {
                writer.AddStyleAttribute(string.Format(prefixFormat, "top", "left"), RadiusTopLeft.ToString() + "px");
                writer.AddStyleAttribute(string.Format(prefixFormat, "top", "right"), RadiusTopRight.ToString() + "px");
                writer.AddStyleAttribute(string.Format(prefixFormat, "bottom", "left"), RadiusBottomLeft.ToString() + "px");
                writer.AddStyleAttribute(string.Format(prefixFormat, "bottom", "right"), RadiusBottomRight.ToString() + "px");
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if(!string.IsNullOrEmpty(prefixFormat) && RadiusCorner > 0)
            {
                writer.AddStyleAttribute(string.Format(prefixFormat, "top", "left"), RadiusTopLeft.ToString() + "px");
                writer.AddStyleAttribute(string.Format(prefixFormat, "top", "right"), RadiusTopRight.ToString() + "px");
            }
            writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, string.Format("0 {0}px 0 {1}px", _padRect.Right, _padRect.Left));

            if(!BorderColor.IsEmpty)
            {
                writer.AddStyleAttribute("border", "solid " + ColorTranslator.ToHtml(BorderColor));
                writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, string.Format("{0}px {0}px 0", BorderWidth));
            }

            if(HeaderTemplateContainer.Controls.Count == 0)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingTop, _padRect.Top + "px");
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, ColorTranslator.ToHtml(ContentColor));
            }
            else
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, ColorTranslator.ToHtml(HeaderColor));

            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if(!HeaderStyle.IsEmpty)
            {
                HeaderStyle.AddAttributesToRender(writer);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                HeaderTemplateContainer.RenderControl(writer);
                writer.RenderEndTag();
            }
            else
                HeaderTemplateContainer.RenderControl(writer);

            writer.RenderEndTag();

            if(!string.IsNullOrEmpty(prefixFormat) && RadiusCorner > 0)
            {
                writer.AddStyleAttribute(string.Format(prefixFormat, "bottom", "left"), RadiusBottomLeft.ToString() + "px");
                writer.AddStyleAttribute(string.Format(prefixFormat, "bottom", "right"), RadiusBottomRight.ToString() + "px");
            }
            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, ColorTranslator.ToHtml(ContentColor));
            writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, string.Format("0 {0}px {1}px {2}px", _padRect.Right, _padRect.Bottom, _padRect.Left));

            if(!BorderColor.IsEmpty)
            {
                writer.AddStyleAttribute("border", "solid " + ColorTranslator.ToHtml(BorderColor));
                writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, string.Format("0 {0}px {0}px", BorderWidth));
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if(!ContentStyle.IsEmpty)
            {
                ContentStyle.AddAttributesToRender(writer);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                ContentTemplateContainer.RenderControl(writer);
                writer.RenderEndTag();
            }
            else
                ContentTemplateContainer.RenderControl(writer);

            writer.RenderEndTag();

            //Main
            writer.RenderEndTag();
        }

        private void RenderImgVersion(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            string css = "ESW_DispBox";
            if(!string.IsNullOrEmpty(CssClass)) css += " " + CssClass;
            writer.AddAttribute(HtmlTextWriterAttribute.Class, css);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            DispBoxImage gfxTR = new DispBoxImage(this, DispBoxImage.Position.TopRight);
            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, gfxTR.FileName + WebGfx.Extension);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ESW_DispBox_head");
            writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingRight, _padRect.Right + "px");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            DispBoxImage gfxTL = new DispBoxImage(this, DispBoxImage.Position.TopLeft);
            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, gfxTL.FileName + WebGfx.Extension);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ESW_DispBox_headLeft");
            writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, _padRect.Left + "px");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            DispBoxImage gfxT = new DispBoxImage(this, DispBoxImage.Position.Top);
            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, gfxT.FileName + WebGfx.Extension);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ESW_DispBox_headCtr");

            if(HeaderTemplateContainer.Controls.Count == 0)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingTop, _padRect.Top + "px");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                if(!HeaderStyle.IsEmpty)
                {
                    HeaderStyle.AddAttributesToRender(writer);
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.RenderEndTag();
                }
            }
            else
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingTop, _boxRect.Y + "px");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                if(!HeaderStyle.IsEmpty)
                {
                    HeaderStyle.AddAttributesToRender(writer);
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    HeaderTemplateContainer.RenderControl(writer);
                    writer.RenderEndTag();
                }
                else
                    HeaderTemplateContainer.RenderControl(writer);
            }

            writer.RenderEndTag();
            //headCtr
            writer.RenderEndTag();
            //headLeft
            writer.RenderEndTag();
            //head

            DispBoxImage gfxR = new DispBoxImage(this, DispBoxImage.Position.Right);
            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, gfxR.FileName + WebGfx.Extension);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ESW_DispBox_body");
            writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingRight, _padRect.Right + "px");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            DispBoxImage gfxL = new DispBoxImage(this, DispBoxImage.Position.Left);
            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, gfxL.FileName + WebGfx.Extension);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ESW_DispBox_bodyLeft");
            writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, _padRect.Left + "px");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, ColorTranslator.ToHtml(ContentColor));
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ESW_DispBox_bodyCtr");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if(!ContentStyle.IsEmpty)
            {
                ContentStyle.AddAttributesToRender(writer);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                ContentTemplateContainer.RenderControl(writer);
                writer.RenderEndTag();
            }
            else
                ContentTemplateContainer.RenderControl(writer);

            writer.RenderEndTag();
            //bodyCtr
            writer.RenderEndTag();
            //bodyLeft
            writer.RenderEndTag();
            //body

            DispBoxImage gfxBR = new DispBoxImage(this, DispBoxImage.Position.BotRight);
            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, gfxBR.FileName + WebGfx.Extension);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ESW_DispBox_foot");
            writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingRight, _padRect.Right + "px");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            DispBoxImage gfxBL = new DispBoxImage(this, DispBoxImage.Position.BotLeft);
            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, gfxBL.FileName + WebGfx.Extension);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ESW_DispBox_footLeft");
            writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, _padRect.Left + "px");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            DispBoxImage gfxB = new DispBoxImage(this, DispBoxImage.Position.Bottom);
            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, gfxB.FileName + WebGfx.Extension);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ESW_DispBox_footCtr");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Height, _padRect.Bottom + "px");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.Write(" ");

            writer.RenderEndTag();
            //footCtr
            writer.RenderEndTag();
            //footLeft
            writer.RenderEndTag();
            //foot

            //Main
            writer.RenderEndTag();
        }

        private void CalculateBoxes()
        {
            _boxRect = new Rectangle(OuterGlow.Blend, OuterGlow.Blend, Math.Max(RadiusTopLeft, RadiusBottomLeft) + Math.Max(RadiusTopRight, RadiusBottomRight) + Math.Abs(DropShadow.XOffset) + 20, Math.Max(RadiusTopLeft, RadiusTopRight) + Math.Max(RadiusBottomLeft, RadiusBottomRight) + Math.Abs(DropShadow.YOffset) + 200);
            _glwRect = new Rectangle(0, 0, _boxRect.Width + (OuterGlow.Blend * 2), _boxRect.Height + (OuterGlow.Blend * 2));
            _shdRect = new Rectangle(OuterGlow.Blend + DropShadow.XOffset - (DropShadow.Blend / 2), OuterGlow.Blend + DropShadow.YOffset - (DropShadow.Blend / 2), _boxRect.Width + (DropShadow.Blend), _boxRect.Height + (DropShadow.Blend));

            if(_shdRect.X < 0)
            {
                _boxRect.X += Math.Abs(_shdRect.X);
                _glwRect.X += Math.Abs(_shdRect.X);
                _shdRect.X = 0;
            }
            if(_shdRect.Y < 0)
            {
                _boxRect.Y += Math.Abs(_shdRect.Y);
                _glwRect.Y += Math.Abs(_shdRect.Y);
                _shdRect.Y = 0;
            }

            _imageSize = new Size(Math.Max(_glwRect.Right, _shdRect.Right) + 1, Math.Max(_glwRect.Bottom, _shdRect.Bottom) + 1);

            _padRect = new Rectangle();

            _padRect.X = Math.Max(RadiusTopLeft, RadiusBottomLeft) + _boxRect.X + Math.Max(0, this.DropShadow.XOffset) + Math.Max(1, this.DropShadow.Blend);
            _padRect.Y = Math.Max(RadiusTopLeft, RadiusTopRight) + _boxRect.Y + Math.Max(0, DropShadow.YOffset);
            _padRect.Width = (_imageSize.Width - _boxRect.Right) + Math.Max(RadiusTopRight, RadiusBottomRight) + Math.Abs(Math.Min(DropShadow.XOffset, 0)) - _padRect.X;
            _padRect.Height = (_imageSize.Height - _boxRect.Bottom) + Math.Max(RadiusBottomLeft, RadiusBottomRight) + Math.Abs(Math.Min(DropShadow.YOffset, 0)) - _padRect.Y;
        }

        #endregion

        #region Templating

        private void ClearHeader()
        {
            HeaderTemplateContainer.Controls.Clear();
            ChildControls.Remove(ContentTemplateContainer);
            _headerTemplateContainer = null;
        }

        private void ClearContent()
        {
            ContentTemplateContainer.Controls.Clear();
            ChildControls.Remove(ContentTemplateContainer);
            _contentTemplateContainer = null;
        }

        private void CreateHeader()
        {
            if(DesignMode) ClearHeader();

            if(_headerTemplateContainer == null)
            {
                _headerTemplateContainer = new Control();
                if(_headerTemplate != null)
                    _headerTemplate.InstantiateIn(_headerTemplateContainer);
                ChildControls.AddInternal(_headerTemplateContainer);
            }
            else if(_headerTemplate != null)
                _headerTemplate.InstantiateIn(_headerTemplateContainer);
        }

        private void CreateContents()
        {
            if(DesignMode) ClearContent();

            if(_contentTemplateContainer == null)
            {
                _contentTemplateContainer = new Control();
                if(_contentTemplate != null)
                    _contentTemplate.InstantiateIn(_contentTemplateContainer);
                ChildControls.AddInternal(_contentTemplateContainer);
            }
            else if(_contentTemplate != null)
                _contentTemplate.InstantiateIn(_contentTemplateContainer);
        }

        private LimitCollection ChildControls
        {
            get
            {
                if(Controls is LimitCollection)
                    return (LimitCollection)Controls;
                else
                    throw new InvalidOperationException("The controls property has the wrong class");
            }
        }

        #endregion

        #region Image Class

        private sealed class DispBoxImage : WebGfx
        {
            public DispBoxImage(DisplayBox box, Position pos)
            {
                _position = pos;
                if(box.HeaderTemplateContainer.Controls.Count > 0 && Util.IsIn(_position, Position.TopLeft, Position.TopRight, Position.Top))
                    _boxColor = box.HeaderColor;
                else
                    _boxColor = box.ContentColor;

                if(box.Page.Request.Browser.Browser == "IE" && box.Page.Request.Browser.MajorVersion < 7)
                    _fixColor = box.IEFixColor;
                else
                    _fixColor = Color.Empty;

                _radTL = box.RadiusTopLeft;
                _radTR = box.RadiusTopRight;
                _radBL = box.RadiusBottomLeft;
                _radBR = box.RadiusBottomRight;
                _ds = box.DropShadow;
                _og = box.OuterGlow;
                _cacheName = box.CacheName;
                _borderColor = box.BorderColor;
                _borderWidth = box.BorderWidth;

                _imageSize = box._imageSize;
                _boxRect = box._boxRect;
                _glwRect = box._glwRect;
                _shdRect = box._shdRect;
                _padRect = box._padRect;

                ImageFormat = System.Drawing.Imaging.ImageFormat.Png;
                ExpireSpan = TimeSpan.FromMinutes(20);
                Save();
            }

            public override string FileName
            {
                get
                {
                    string fn = CacheFileName();
                    switch(_position)
                    {
                        case Position.TopLeft: fn = "TL_" + CacheFileName(); break;
                        case Position.TopRight: fn = "TR_" + CacheFileName(); break;
                        case Position.Top: fn = "T_" + CacheFileName(); break;
                        case Position.Left: fn = "L_" + CacheFileName(); break;
                        case Position.Right: fn = "R_" + CacheFileName(); break;
                        case Position.BotLeft: fn = "BL_" + CacheFileName(); break;
                        case Position.BotRight: fn = "BR_" + CacheFileName(); break;
                        case Position.Bottom: fn = "B_" + CacheFileName(); break;
                    }
                    return fn;
                }
                set
                { }
            }

            public override System.Drawing.Image Image()
            {
                Bitmap rst = null;
                switch(_position)
                {
                    case Position.TopLeft:
                        rst = PartImage(GenImage(), 0, 0, _padRect.Left, _padRect.Top + 200);
                        break;
                    case Position.TopRight:
                        rst = PartImage(GenImage(), _imageSize.Width - _padRect.Right, 0, _padRect.Right, _padRect.Top + 200);
                        break;
                    case Position.Top:
                        rst = PartImage(GenImage(), _padRect.Left, 0, 1, _padRect.Top + 200);
                        break;
                    case Position.Right:
                        rst = PartImage(GenImage(), _imageSize.Width - _padRect.Right, _padRect.Top + 10, _padRect.Right, 1);
                        break;
                    case Position.Left:
                        rst = PartImage(GenImage(), 0, _padRect.Top + 10, _padRect.Left, 1);
                        break;
                    case Position.BotRight:
                        rst = PartImage(GenImage(), _imageSize.Width - _padRect.Right, _imageSize.Height - _padRect.Bottom, _padRect.Right, _padRect.Bottom);
                        break;
                    case Position.BotLeft:
                        rst = PartImage(GenImage(), 0, _imageSize.Height - _padRect.Bottom, _padRect.Left, _padRect.Bottom);
                        break;
                    case Position.Bottom:
                        rst = PartImage(GenImage(), _padRect.Left, _imageSize.Height - _padRect.Bottom, 1, _padRect.Bottom);
                        break;
                }
                return rst;
            }

            private Bitmap PartImage(System.Drawing.Image img, int xPos, int yPos, int width, int height)
            {
                Bitmap rst = new Bitmap(width, height);
                using(Graphics gfx = Graphics.FromImage(rst))
                {
                    gfx.DrawImage(img, new Rectangle(0, 0, width, height), new Rectangle(xPos, yPos, width, height), GraphicsUnit.Pixel);
                }
                return rst;
            }

            private string CacheFileName()
            {
                string name = _radTL.ToString() + "x" + _radTR.ToString() + "x" + _radBL.ToString() + "x" + _radBR.ToString();
                name += "_C" + ColorTranslator.ToOle(_boxColor).ToString();
                name += _ds.FileString() + _og.FileString();
                if(_fixColor != Color.Empty)
                    name += "_F" + _fixColor.ToString();
                if(_borderColor != Color.Empty && _borderWidth > 0)
                {
                    name += "_B" + ColorTranslator.ToOle(_borderColor).ToString();
                    name += "-" + _borderWidth.ToString();
                }
                return name;
            }

            private Bitmap GenImage()
            {
                string imagePath = Path.GetTempPath() + _cacheName + "\\" + CacheFileName() + ".png";
                if(File.Exists(imagePath))
                    return new Bitmap(imagePath);
                else
                {
                    Bitmap shdImg = null;
                    if(!_ds.ShadowColor.IsEmpty)
                    {
                        shdImg = new Bitmap(_shdRect.Width + 1, _shdRect.Height + 1);
                        using(Graphics gfx = Graphics.FromImage(shdImg))
                        {
                            gfx.Clear(Color.Transparent);
                            gfx.SmoothingMode = SmoothingMode.AntiAlias;
                            GraphicsPath boxPath = GenBox(_radTL, _radTR, _radBL, _radBR, _boxRect.Width, _boxRect.Height);
                            Matrix tf = new Matrix();
                            tf.Translate((_ds.Blend / 2), (_ds.Blend / 2));
                            boxPath.Transform(tf);
                            gfx.FillPath(new SolidBrush(_ds.ShadowColor), boxPath);
                        }
                        if(_ds.Blend > 0)
                        {
                            shdImg = Graphic.Blur.Gaussian(shdImg, _ds.Blend);
                            shdImg = Graphic.Blur.FillAlphaPreserve(shdImg, _ds.ShadowColor);
                        }
                    }

                    Bitmap glwImg = null;
                    if(!_og.GlowColor.IsEmpty)
                    {
                        glwImg = new Bitmap(_glwRect.Width + 1, _glwRect.Height + 1);
                        using(Graphics gfx = Graphics.FromImage(glwImg))
                        {
                            gfx.Clear(Color.Transparent);
                            gfx.SmoothingMode = SmoothingMode.AntiAlias;
                            GraphicsPath glowPath = GenBox(_radTL + _og.Blend, _radTR + _og.Blend, _radBL + _og.Blend, _radBR + _og.Blend, _boxRect.Width + _og.Blend, _boxRect.Height + _og.Blend);
                            Matrix tf = new Matrix();
                            tf.Translate(_og.Blend / 2, _og.Blend / 2);
                            glowPath.Transform(tf);
                            gfx.FillPath(new SolidBrush(_og.GlowColor), glowPath);
                        }
                        if(_og.Blend > 0)
                        {
                            glwImg = Graphic.Blur.Gaussian(glwImg, _og.Blend);
                            glwImg = Graphic.Blur.FillAlphaPreserve(glwImg, _og.GlowColor);
                        }
                    }

                    Bitmap img = new Bitmap(_imageSize.Width, _imageSize.Height);

                    using(Graphics gfx = Graphics.FromImage(img))
                    {
                        if(_fixColor != Color.Empty)
                            gfx.Clear(_fixColor);
                        else
                            gfx.Clear(Color.Transparent);

                        if(!_ds.ShadowColor.IsEmpty)
                            gfx.DrawImageUnscaled(shdImg, _shdRect.X, _shdRect.Y);
                        if(!_og.GlowColor.IsEmpty)
                            gfx.DrawImageUnscaled(glwImg, _glwRect.X, _glwRect.Y);
                        gfx.SmoothingMode = SmoothingMode.AntiAlias;
                        GraphicsPath boxPath = GenBox(_radTL, _radTR, _radBL, _radBR, _boxRect.Width, _boxRect.Height);
                        Matrix tf = new Matrix();
                        tf.Translate(_boxRect.X, _boxRect.Y);
                        boxPath.Transform(tf);
                        gfx.FillPath(new SolidBrush(_boxColor), boxPath);

                        if(!_borderColor.IsEmpty && _borderWidth > 0)
                            gfx.DrawPath(new Pen(_borderColor, _borderWidth), boxPath);
                    }

                    if(!File.Exists(imagePath))
                    {
                        if(!string.IsNullOrEmpty(_cacheName))
                        {
                            if(!Directory.Exists(Path.GetTempPath() + _cacheName))
                                Directory.CreateDirectory(Path.GetTempPath() + _cacheName);
                            img.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
                        }
                    }
                    return img;
                }
            }

            private GraphicsPath GenBox(int topLeft, int topRight, int botLeft, int botRight, int width, int height)
            {
                GraphicsPath gp = new GraphicsPath();
                gp.StartFigure();
                if(topLeft > 0)
                    gp.AddArc(0, 0, topLeft * 2, topLeft * 2, 180, 90);
                gp.AddLine(topLeft, 0, width - topRight, 0);
                if(topRight > 0)
                    gp.AddArc(width - (topRight * 2), 0, topRight * 2, topRight * 2, 270, 90);
                gp.AddLine(width, topRight, width, height - botRight);
                if(botRight > 0)
                    gp.AddArc(width - (botRight * 2), height - (botRight * 2), botRight * 2, botRight * 2, 0, 90);
                gp.AddLine(width - botRight, height, botLeft, height);
                if(botLeft > 0)
                    gp.AddArc(0, height - (botLeft * 2), botLeft * 2, botLeft * 2, 90, 90);
                gp.AddLine(0, height - botLeft, 0, topLeft);
                gp.CloseFigure();
                return gp;
            }

            private Bitmap gaussianBlur(Bitmap img, int blurSize)
            {
                Bitmap nm = new Bitmap(img.Width, img.Height);

                for(int py = 0; py <= img.Height - 1; py++)
                {
                    for(int px = 0; px <= img.Width - 1; px++)
                    {

                        ArrayList pxls = new ArrayList();

                        for(int x = px - (int)blurSize / 2; x <= px + (int)blurSize / 2; x++)
                        {
                            for(int y = py - (int)blurSize / 2; y <= py + (int)blurSize / 2; y++)
                            {
                                if(x > 0 && x < img.Width && y > 0 && y < img.Height)
                                    pxls.Add(img.GetPixel(x, y));
                            }
                        }

                        int r = 0;
                        int g = 0;
                        int b = 0;
                        int a = 0;

                        foreach(Color col in pxls)
                        {
                            r += col.R;
                            g += col.G;
                            b += col.B;
                            a += col.A;
                        }

                        if(pxls.Count > 0)
                            nm.SetPixel(px, py, Color.FromArgb(a / pxls.Count, r / pxls.Count, g / pxls.Count, b / pxls.Count));
                    }
                }
                return nm;
            }

            private Position _position;
            private int _radTL, _radTR, _radBL, _radBR;
            private Color _boxColor;
            private Color _fixColor;
            private Color _borderColor;
            private int _borderWidth;
            private DropShadow _ds;
            private OuterGlow _og;
            private string _cacheName;
            private Size _imageSize;
            private Rectangle _boxRect;
            private Rectangle _glwRect;
            private Rectangle _shdRect;
            private Rectangle _padRect;

            public enum Position
            {
                TopLeft,
                Top,
                TopRight,
                Left,
                Right,
                BotLeft,
                Bottom,
                BotRight
            }
        }

        #endregion

        #region Variables

        private ITemplate _contentTemplate;
        private Control _contentTemplateContainer;
        private ITemplate _headerTemplate;
        private Control _headerTemplateContainer;

        private DropShadow _dropShadow;
        private OuterGlow _outerGlow;
        private PaddingStyle _contentStyle;
        private PaddingStyle _headerStyle;

        internal Size _imageSize;
        internal Rectangle _boxRect;
        internal Rectangle _glwRect;
        internal Rectangle _shdRect;
        internal Rectangle _padRect;

        #endregion

        #endregion
    }

    /// <summary>
    /// The drop shadow values for a display box
    /// </summary>
    [ParseChildren(true)]
    public class DropShadow : ViewStateBase
    {

        /// <summary>
        /// The X offset of the shadow
        /// </summary>
        [Category("Appearance"), DefaultValue(0), NotifyParentProperty(true)]
        public int XOffset
        {
            get
            {
                if(ViewState["XOffset"] == null)
                    return 0;
                else
                    return (int)ViewState["XOffset"];
            }
            set
            {
                if(value == 0)
                    ViewState.Remove("XOffset");
                else
                    ViewState["XOffset"] = value;
            }
        }

        /// <summary>
        /// The Y offset of the shadow
        /// </summary>
        [Category("Appearance"), DefaultValue(0), NotifyParentProperty(true)]
        public int YOffset
        {
            get
            {
                if(ViewState["YOffset"] == null)
                    return 0;
                else
                    return (int)ViewState["YOffset"];
            }
            set
            {
                if(value == 0)
                    ViewState.Remove("YOffset");
                else
                    ViewState["YOffset"] = value;
            }
        }

        /// <summary>
        /// The base color for the shadow
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Color), null), NotifyParentProperty(true), TypeConverter(typeof(WebColorConverter))]
        public Color ShadowColor
        {
            get
            {
                if(ViewState["ShadowColor"] == null)
                    return Color.Empty;
                else
                    return (Color)ViewState["ShadowColor"];
            }
            set
            {
                if(value == Color.Empty)
                    ViewState.Remove("ShadowColor");
                else
                    ViewState["ShadowColor"] = value;
            }
        }

        /// <summary>
        /// The Blend factor for the shadow
        /// </summary>
        [Category("Appearance"), DefaultValue(0), NotifyParentProperty(true)]
        public int Blend
        {
            get
            {
                if(ViewState["Blend"] == null)
                    return 0;
                else
                    return (int)ViewState["Blend"];
            }
            set
            {
                if(value == 0)
                    ViewState.Remove("Blend");
                else
                    ViewState["Blend"] = value;
            }
        }

        internal string FileString()
        {
            if(!ShadowColor.IsEmpty)
                return "_" + XOffset.ToString() + "x" + YOffset.ToString() + "_" + ShadowColor.ToString().Replace("Color", "C").Replace(" ", "").Replace("[", "").Replace("]", "").Replace("=", "").Replace(",", "_") + "_" + Blend.ToString();
            else
                return string.Empty;
        }

    }

    /// <summary>
    /// The details of the outer glow
    /// </summary>
    [ParseChildren(true)]
    public class OuterGlow : ViewStateBase
    {

        /// <summary>
        /// The color of the glow
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Color), null), NotifyParentProperty(true), TypeConverter(typeof(WebColorConverter))]
        public Color GlowColor
        {
            get
            {
                if(ViewState["GlowColor"] == null)
                    return Color.Empty;
                else
                    return (Color)ViewState["GlowColor"];
            }
            set
            {
                if(value == Color.Empty)
                    ViewState.Remove("GlowColor");
                else
                    ViewState["GlowColor"] = value;
            }
        }

        /// <summary>
        /// The Blend factor for the glow
        /// </summary>
        [Category("Appearance"), DefaultValue(0), NotifyParentProperty(true)]
        public int Blend
        {
            get
            {
                if(ViewState["Blend"] == null)
                    return 0;
                else
                    return (int)ViewState["Blend"];
            }
            set
            {
                if(value == 0)
                    ViewState.Remove("Blend");
                else
                {
                    if(value < 0) value = 0;
                    ViewState["Blend"] = value;
                }
            }
        }

        internal string FileString()
        {
            if(!GlowColor.IsEmpty)
                return "_" + GlowColor.ToString().Replace("Color", "C").Replace(" ", "").Replace("[", "").Replace("]", "").Replace("=", "").Replace(",", "_") + "_" + Blend.ToString();
            else
                return string.Empty;
        }

    }
}