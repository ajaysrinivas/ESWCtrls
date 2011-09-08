using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ESWCtrls
{
    /// <summary>
    /// Extends the default style class with padding
    /// </summary>
    public class PaddingStyle : Style
    {

        #region Constructors

        /// <summary>
        /// Standard Constructor
        /// </summary>
        public PaddingStyle()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Constructor with given viewstate bag
        /// </summary>
        /// <param name="bag">ViewState bag</param>
        public PaddingStyle(StateBag bag)
            : base(bag)
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The Top padding
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Unit), null), NotifyParentProperty(true)]
        public virtual Unit PaddingTop
        {
            get
            {
                if(ViewState[STR_PADTOP] == null)
                    return Unit.Empty;
                else
                    return (Unit)ViewState[STR_PADTOP];
            }
            set { ViewState[STR_PADTOP] = value; }
        }

        /// <summary>
        /// The bottom padding
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Unit), null), NotifyParentProperty(true)]
        public virtual Unit PaddingBottom
        {
            get
            {
                if(ViewState[STR_PADBOT] == null)
                    return Unit.Empty;
                else
                    return (Unit)ViewState[STR_PADBOT];
            }
            set { ViewState[STR_PADBOT] = value; }
        }

        /// <summary>
        /// The left padding
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Unit), null), NotifyParentProperty(true)]
        public virtual Unit PaddingLeft
        {
            get
            {
                if(ViewState[STR_PADLEFT] == null)
                    return Unit.Empty;
                else
                    return (Unit)ViewState[STR_PADLEFT];
            }
            set { ViewState[STR_PADLEFT] = value; }
        }

        /// <summary>
        /// The right padding
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Unit), null), NotifyParentProperty(true)]
        public virtual Unit PaddingRight
        {
            get
            {
                if(ViewState[STR_PADRIGHT] == null)
                    return Unit.Empty;
                else
                    return (Unit)ViewState[STR_PADRIGHT];
            }
            set { ViewState[STR_PADRIGHT] = value; }
        }

        /// <summary>
        /// Write only to set all the padding
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Unit), null), NotifyParentProperty(true)]
        public virtual Unit Padding
        {
            set
            {
                ViewState[STR_PADTOP] = value;
                ViewState[STR_PADBOT] = value;
                ViewState[STR_PADLEFT] = value;
                ViewState[STR_PADRIGHT] = value;
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
                    if( PaddingTop.IsEmpty && PaddingBottom.IsEmpty && PaddingLeft.IsEmpty)
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
        public virtual void CopyFrom(AdvStyle s)
        {
            if(s != null & !s.IsEmpty)
            {
                base.CopyFrom(s);
                if(s.RegisteredCssClass.Length != 0)
                {
                    if(!s.PaddingTop.IsEmpty)
                        ViewState.Remove(STR_PADTOP);

                    if(!s.PaddingBottom.IsEmpty)
                        ViewState.Remove(STR_PADBOT);

                    if(!s.PaddingLeft.IsEmpty)
                        ViewState.Remove(STR_PADLEFT);

                    if(!s.PaddingRight.IsEmpty)
                        ViewState.Remove(STR_PADRIGHT);
                }
                else
                {
                    if(!s.PaddingTop.IsEmpty)
                        PaddingTop = s.PaddingTop;

                    if(!s.PaddingBottom.IsEmpty)
                        PaddingTop = s.PaddingBottom;

                    if(!s.PaddingLeft.IsEmpty)
                        PaddingTop = s.PaddingLeft;

                    if(!s.PaddingRight.IsEmpty)
                        PaddingTop = s.PaddingRight;
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

            if(!PaddingTop.IsEmpty)
                attributes.Add(HtmlTextWriterStyle.PaddingTop, PaddingTop.ToString());

            if(!PaddingBottom.IsEmpty)
                attributes.Add(HtmlTextWriterStyle.PaddingBottom, PaddingTop.ToString());

            if(!PaddingLeft.IsEmpty)
                attributes.Add(HtmlTextWriterStyle.PaddingLeft, PaddingLeft.ToString());

            if(!PaddingRight.IsEmpty)
                attributes.Add(HtmlTextWriterStyle.PaddingRight, PaddingRight.ToString());
        }

        #endregion

        #region Internal

        internal string RenderClass
        {
            get
            {
                var css = this.CssClass;
                if(string.IsNullOrEmpty(css))
                    css = this.RegisteredCssClass;
                else if(!string.IsNullOrEmpty(this.RegisteredCssClass))
                    css += " " + this.RegisteredCssClass;
                return css;
            }
        }

        #endregion

        #region Private

        private const string STR_PADTOP = "PaddingTop";
        private const string STR_PADBOT = "PaddingBot";
        private const string STR_PADLEFT = "PaddingLeft";
        private const string STR_PADRIGHT = "PaddingRight";

        #endregion

    }
}