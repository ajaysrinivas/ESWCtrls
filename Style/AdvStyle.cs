using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace ESWCtrls
{
    /// <summary>
    /// Extends the default style class with padding background image and z-index
    /// </summary>
    public class AdvStyle : PaddingStyle
    {
        #region Constructors

        /// <summary>
        /// Standard Constructor
        /// </summary>
        public AdvStyle()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Constructor with given viewstate bag
        /// </summary>
        /// <param name="bag">ViewState bag</param>
        public AdvStyle(StateBag bag)
            : base(bag)
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The background image url
        /// </summary>
        [Category("Appearance"), DefaultValue(""), UrlProperty(), Editor( typeof(ImageUrlEditor), typeof(UITypeEditor)), NotifyParentProperty(true)]
        public virtual string BackgroundImageUrl
        {
            get
            {
                if(ViewState["BackImageUrl"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["BackImageUrl"];
            }
            set { ViewState["BackImageUrl"] = value; }
        }

        /// <summary>
        /// The position of the background image
        /// </summary>
        [Category("Appearance"), DefaultValue(BackgroundPosition.NotSet), NotifyParentProperty(true)]
        public virtual BackgroundPosition BackgroundImagePosition
        {
            get
            {
                if(ViewState["BackImagePos"] == null)
                    return BackgroundPosition.NotSet;
                else
                    return (BackgroundPosition)ViewState["BackImagePos"];
            }
            set { ViewState["BackImagePos"] = value; }
        }

        /// <summary>
        /// How the background image is to be repeated
        /// </summary>
        [Category("Appearance"), DefaultValue(BackgroundRepeat.NotSet), NotifyParentProperty(true)]
        public virtual BackgroundRepeat BackgroundImageRepeat
        {
            get
            {
                if(ViewState["BackImageRepeat"] == null)
                    return BackgroundRepeat.NotSet;
                else
                    return (BackgroundRepeat)ViewState["BackImageRepeat"];
            }
            set { ViewState["BackImageRepeat"] = value; }
        }

        /// <summary>
        /// the z-index of the control
        /// </summary>
        [Category("Appearance"), DefaultValue(0), NotifyParentProperty(true)]
        public virtual int ZIndex
        {
            get
            {
                if(ViewState["zIndex"] == null)
                    return 0;
                else
                    return (int)ViewState["zIndex"];
            }
            set { ViewState["zIndex"] = value; }
        }

        /// <summary>
        /// Display Property
        /// </summary>
        [Category("Appearance"), DefaultValue(Display.NotSet),NotifyParentProperty(true)]
        public Display Display
		{
			get
			{
				if(ViewState["Display"] != null)
					return(Display)ViewState["Display"];
				else
					return Display.NotSet;
			}
			set
			{
				if(value != Display.NotSet)
					ViewState["Display"] = value;
				else
					ViewState.Remove("Display");
			}
		}

        /// <summary>
        /// This css positioning
        /// </summary>
        [Category("Appearance"), DefaultValue(ElementPosition.NotSet), NotifyParentProperty(true)]
        public virtual ElementPosition Position
        {
            get
            {
                if(ViewState["Position"] == null)
                    return ElementPosition.NotSet;
                else
                    return (ElementPosition)ViewState["Position"];
            }
            set { ViewState["Position"] = value; }
        }

        /// <summary>
        /// The left position of the element
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Unit), null), NotifyParentProperty(true)]
        public virtual Unit Left
        {
            get
            {
                if(ViewState["Left"] == null)
                    return Unit.Empty;
                else
                    return (Unit)ViewState["Left"];
            }
            set { ViewState["Left"] = value; }
        }

        /// <summary>
        /// The right position of the element
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Unit), null), NotifyParentProperty(true)]
        public virtual Unit Right
        {
            get
            {
                if(ViewState["Right"] == null)
                    return Unit.Empty;
                else
                    return (Unit)ViewState["Right"];
            }
            set { ViewState["Right"] = value; }
        }

        /// <summary>
        /// The top position of the element
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Unit), null), NotifyParentProperty(true)]
        public virtual Unit Top
        {
            get
            {
                if(ViewState["Top"] == null)
                    return Unit.Empty;
                else
                    return (Unit)ViewState["Top"];
            }
            set { ViewState["Top"] = value; }
        }

        /// <summary>
        /// The bottom position of the element
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Unit), null), NotifyParentProperty(true)]
        public virtual Unit Bottom
        {
            get
            {
                if(ViewState["Bottom"] == null)
                    return Unit.Empty;
                else
                    return (Unit)ViewState["Bottom"];
            }
            set { ViewState["Bottom"] = value; }
        }

        /// <summary>
        /// The opacity of the element
        /// </summary>
        [Category("Appearance"), DefaultValue(1.0f), NotifyParentProperty(true)]
        public virtual float Opacity
        {
            get
            {
                if(ViewState["Opacity"] == null)
                    return 1.0f;
                else
                    return (float)ViewState["Opacity"];
            }
            set
            { ViewState["Opacity"] = value < 0.0f ? 0.0f : value > 1.0f ? 1.0f : value; }
        }

        /// <summary>
        /// The floating of the element
        /// </summary>
        [Category("Appearance"), DefaultValue(ElementFloat.NotSet), NotifyParentProperty(true)]
        public virtual ElementFloat Float
		{
			get
			{
				if(ViewState["Float"] != null)
					return(ElementFloat)ViewState["Float"];
				else
					return ElementFloat.NotSet;
			}
			set
			{
				if(value != ElementFloat.NotSet)
					ViewState["Float"] = value;
				else
					ViewState.Remove("Float");
			}
		}

        /// <summary>
        /// The list style type
        /// </summary>
        [Category("Appearance"), DefaultValue(ListStyleType.NotSet), NotifyParentProperty(true)]
        public virtual ListStyleType ListStyleType
		{
			get
			{
				if(ViewState["ListStyleType"] != null)
					return(ListStyleType)ViewState["ListStyleType"];
				else
					return ListStyleType.NotSet;
			}
			set
			{
				if(value != ListStyleType.NotSet)
					ViewState["ListStyleType"] = value;
				else
					ViewState.Remove("ListStyleType");
			}
		}

        /// <summary>
        /// The list style position
        /// </summary>
        [Category("Appearance"),DefaultValue(ListStylePosition.NotSet),NotifyParentProperty(true)]
        public virtual ListStylePosition ListStylePosition
		{
			get
			{
				if(ViewState["ListStylePosition"] != null)
					return(ListStylePosition)ViewState["ListStylePosition"];
				else
					return ListStylePosition.NotSet;
			}
			set
			{
				if(value != ListStylePosition.NotSet)
					ViewState["ListStylePosition"] = value;
				else
					ViewState.Remove("ListStylePosition");
			}
		}

        /// <summary>
        /// Specifies the image to use for a marker in a list
        /// </summary>
        [Category("Appearance"), DefaultValue(null), UrlProperty(), Editor(typeof(ImageUrlEditor), typeof(UITypeEditor)), NotifyParentProperty(true)]
        public virtual string ListStyleImage
		{
			get
			{
				if(ViewState["ListStyleImage"] != null)
					return(string)ViewState["ListStyleImage"];
				else
					return null;
			}
			set
			{
				if(value != null)
					ViewState["ListStyleImage"] = value;
				else
					ViewState.Remove("ListStyleImage");
			}
		}
        
        /// <summary>
        /// True if nothing is set within the style
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool IsEmpty
        {
            get
            {
                if(base.IsEmpty)
                {
                    if(BackgroundImageUrl.Length == 0 && BackgroundImagePosition == BackgroundPosition.NotSet && BackgroundImageRepeat == BackgroundRepeat.NotSet && ZIndex == 0 &&
                        Position == ElementPosition.NotSet && Left.IsEmpty && Right.IsEmpty && Top.IsEmpty && Bottom.IsEmpty && Opacity == 1.0f && Float == ElementFloat.NotSet)
                        return true;
                }

                return false;
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Fills this style with the values from another
        /// </summary>
        /// <param name="s">The style to copy from</param>
        public override void CopyFrom(AdvStyle s)
        {
            if(s != null & !s.IsEmpty)
            {
                base.CopyFrom(s);
                if(s.RegisteredCssClass.Length != 0)
                {
                    if(!string.IsNullOrEmpty(s.BackgroundImageUrl))
                        ViewState.Remove("BackImageUrl");

                    if(s.BackgroundImagePosition != BackgroundPosition.NotSet)
                        ViewState.Remove("BackImagePos");

                    if(s.BackgroundImageRepeat != BackgroundRepeat.NotSet)
                        ViewState.Remove("BackImageRepeat");

                    if(s.ZIndex != 0)
                        ViewState.Remove("zIndex");

                    if(s.Display != Display.NotSet)
                        ViewState.Remove("Display");

                    if(s.Position != ElementPosition.NotSet)
                        ViewState.Remove("Position");

                    if(!s.Left.IsEmpty)
                        ViewState.Remove("Left");

                    if(!s.Right.IsEmpty)
                        ViewState.Remove("Right");

                    if(!s.Top.IsEmpty)
                        ViewState.Remove("Top");

                    if(!s.Bottom.IsEmpty)
                        ViewState.Remove("Bottom");

                    if(s.Opacity != 1.0f)
                        ViewState.Remove("Opacity");

                    if(s.Float != ElementFloat.NotSet)
                        ViewState.Remove("Float");

                    if(s.ListStyleType != ListStyleType.NotSet)
                        ViewState.Remove("ListStyleType");

                    if(s.ListStylePosition != ListStylePosition.NotSet)
                        ViewState.Remove("ListStylePosition");

                    if(!string.IsNullOrEmpty(ListStyleImage))
                        ViewState.Remove("ListStyleImage");
                }
                else
                {
                    if(!string.IsNullOrEmpty(s.BackgroundImageUrl))
                        BackgroundImageUrl = s.BackgroundImageUrl;

                    if(s.BackgroundImagePosition != BackgroundPosition.NotSet)
                        BackgroundImagePosition = s.BackgroundImagePosition;

                    if(s.BackgroundImageRepeat != BackgroundRepeat.NotSet)
                        BackgroundImageRepeat = s.BackgroundImageRepeat;

                    if(s.ZIndex != 0)
                        ZIndex = s.ZIndex;

                    if(s.Display != Display.NotSet)
                        Display = s.Display;

                    if(s.Position != ElementPosition.NotSet)
                        Position = s.Position;

                    if(!s.Left.IsEmpty)
                        Left = s.Left;

                    if(!s.Right.IsEmpty)
                        Right = s.Right;

                    if(!s.Top.IsEmpty)
                        Top = s.Top;

                    if(!s.Bottom.IsEmpty)
                        Bottom = s.Bottom;

                    if(s.Opacity != 1.0f)
                        Opacity = s.Opacity;

                    if(s.Float != ElementFloat.NotSet)
                        Float = s.Float;

                    if(s.ListStyleType != ListStyleType.NotSet)
                        ListStyleType = s.ListStyleType;

                    if(s.ListStylePosition != ListStylePosition.NotSet)
                        ListStylePosition = s.ListStylePosition;

                    if(!string.IsNullOrEmpty(s.ListStyleImage))
                        ListStyleImage = s.ListStyleImage;
                }
            }
        }

        #endregion

        #region Protected

        /// <summary>
        /// Fills the attributes collection given with the ones from the style
        /// </summary>
        /// <param name="attributes">The atrributes</param>
        /// <param name="urlResolver">The resolver to use</param>
        protected override void FillStyleAttributes(CssStyleCollection attributes, IUrlResolutionService urlResolver)
        {
            base.FillStyleAttributes(attributes, urlResolver);

            if(!string.IsNullOrEmpty(BackgroundImageUrl))
                attributes.Add(HtmlTextWriterStyle.BackgroundImage, "url('" + urlResolver.ResolveClientUrl(BackgroundImageUrl) + "')");

            if(BackgroundImagePosition != BackgroundPosition.NotSet)
                attributes.Add("background-position", backgroundPos[(int)BackgroundImagePosition]);

            if(BackgroundImageRepeat != BackgroundRepeat.NotSet)
                attributes.Add("background-repeat", backgroundRpt[(int)BackgroundImageRepeat]);

            if(ZIndex != 0)
                attributes.Add(HtmlTextWriterStyle.ZIndex, ZIndex.ToString());

            if(Display != Display.NotSet)
                attributes.Add(HtmlTextWriterStyle.Display, displayStr[(int)Display]);

            if(Position != ElementPosition.NotSet)
                attributes.Add("position", positionStr[(int)Position]);

            if(!Left.IsEmpty)
                attributes.Add("left", Left.ToString());

            if(!Right.IsEmpty)
                attributes.Add("right", Right.ToString());

            if(!Top.IsEmpty)
                attributes.Add("top", Top.ToString());

            if(!Bottom.IsEmpty)
                attributes.Add("bottom", Bottom.ToString());

            if(Opacity != 1.0f)
            {
                attributes.Add("opacity", Opacity.ToString("0.0"));
                attributes.Add("-ms-filter", string.Format("progid:DXImageTransform.Microsoft.Alpha(Opacity={0:0})", Opacity * 100));
                attributes.Add("filter", string.Format("alpha(opacity={0:0})", Opacity * 100));
            }

            if(Float != ElementFloat.NotSet)
                attributes.Add("float", floatStr[(int)Float]);

            if(ListStyleType != ListStyleType.NotSet)
                attributes.Add(HtmlTextWriterStyle.ListStyleType, ListStyleTypeStr[(int)ListStyleType]);

            if(ListStylePosition != ListStylePosition.NotSet)
                attributes.Add("list-style-position", ListStylePositionStr[(int)ListStylePosition]);

            if(!string.IsNullOrEmpty(ListStyleImage))
                attributes.Add(HtmlTextWriterStyle.ListStyleImage, "url('" + urlResolver.ResolveClientUrl(ListStyleImage) + "')");
        }

        #endregion

        #region Private

        private static readonly string[] backgroundPos = new string[] { "NotSet", "left top", "left center", "left bottom", "center top", "center center", "center bottom", "right top", 
                                                                        "right center", "right bottom", "inherit" };
        private static readonly string[] backgroundRpt = new string[] { "NotSet", "repeat", "no-repeat", "repeat-x", "repeat-y", "inherit" };
        private static readonly string[] displayStr = new string[] { "NotSet", "none", "block", "inline", "inline-block", "inline-table", "list-item", "run-in", "table", "table-caption",
                                                                        "table-cell", "table-column", "table-column-group", "table-footer-group", "table-header-group", "table-row",
                                                                        "table-row-group", "inherit" };
        private static readonly string[] positionStr = new string[] { "NotSet", "static", "absolute", "fixed", "relative", "inherit" };
        private static readonly string[] floatStr = new string[] { "NotSet", "left", "right", "none", "inherit" };
        private static readonly string[] ListStyleTypeStr = new string[] { "NotSet", "armenian","circle","cjk-ideographic","decimal","decimal-leading-zero","disc","georgian","hebrew",
                                                                            "hiragana", "hiragana-iroha","inherit","katakana","katakana-iroha","lower-alpha","lower-greek","lower-latin",
                                                                            "lower-roman", "none", "square", "upper-alpha","upper-latin","upper-roman"};
        private static readonly string[] ListStylePositionStr = new string[] { "NotSet", "inside", "outside", "inherit" };

        #endregion

    }
}