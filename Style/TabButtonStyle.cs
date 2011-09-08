using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace ESWCtrls
{
    /// <summary>
    /// Styles for pad buttons
    /// </summary>
    public class TabButtonStyle : Style
    {
        #region Constructors

        /// <summary>
        /// Standard Constructor
        /// </summary>
        public TabButtonStyle()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Constructor with given viewstate bag
        /// </summary>
        /// <param name="bag">ViewState bag</param>
        public TabButtonStyle(StateBag bag)
            : base(bag)
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The background image for the tab, or the middle image if we have left and right images
        /// </summary>
        [Category("Appearance"), DefaultValue(""), UrlProperty(), Editor(typeof(ImageUrlEditor), typeof(UITypeEditor)), NotifyParentProperty(true)]
        public virtual string BackImage
        {
            get
            {
                if(ViewState[STR_BACKIMG] == null)
                    return string.Empty;
                else
                    return (string)ViewState[STR_BACKIMG];
            }
            set { ViewState[STR_BACKIMG] = value; }
        }

        /// <summary>
        /// The left size of the background image to use on the tab
        /// </summary>
        [Category("Appearance"), DefaultValue(0), NotifyParentProperty(true)]
        public virtual int ImageLeftSize
        {
            get
            {
                if(ViewState[STR_BACKIMGLEFT] == null)
                    return 0;
                else
                    return (int)ViewState[STR_BACKIMGLEFT];
            }
            set { ViewState[STR_BACKIMGLEFT] = value; }
        }

        /// <summary>
        /// The right size of the background image to use on the tab
        /// </summary>
        [Category("Appearance"), DefaultValue(0), NotifyParentProperty(true)]
        public virtual int ImageRightSize
        {
            get
            {
                if(ViewState[STR_BACKIMGRIGHT] == null)
                    return 0;
                else
                    return (int)ViewState[STR_BACKIMGRIGHT];
            }
            set { ViewState[STR_BACKIMGRIGHT] = value; }
        }

        /// <summary>
        /// The top size of the background image to use on the tab
        /// </summary>
        [Category("Appearance"), DefaultValue(0), NotifyParentProperty(true)]
        public virtual int ImageTopSize
        {
            get
            {
                if(ViewState[STR_BACKIMGTOP] == null)
                    return 0;
                else
                    return (int)ViewState[STR_BACKIMGTOP];
            }
            set { ViewState[STR_BACKIMGTOP] = value; }
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
                    if(string.IsNullOrEmpty(BackImage) && ImageLeftSize == 0 && ImageRightSize == 0 && ImageTopSize == 0)
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
        public virtual void CopyFrom(TabButtonStyle s)
        {
            if(s != null & !s.IsEmpty)
            {
                base.CopyFrom(s);

                if(!string.IsNullOrEmpty(BackImage))
                    BackImage = s.BackImage;

                if(ImageLeftSize != 0)
                    ImageLeftSize = s.ImageLeftSize;

                if(ImageRightSize != 0)
                    ImageRightSize = s.ImageRightSize;

                if(ImageTopSize != 0)
                    ImageTopSize = s.ImageTopSize;
            }
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
                else
                    css += " " + this.RegisteredCssClass;
                return css;
            }
        }

        #endregion

        #region Variables

        private const string STR_BACKIMG = "BackImage";
        private const string STR_BACKIMGLEFT = "ImageLeftSize";
        private const string STR_BACKIMGRIGHT = "ImageRightSize";
        private const string STR_BACKIMGTOP = "ImageTopSize";

        #endregion
    }
}
