using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESWCtrls
{
    using Internal;
    using System.ComponentModel;
    using System.Web.UI;

    /// <summary>
    /// Used by controls for position elements or the control itself
    /// </summary>
    public class Positioning : ViewStateBase
    {
        /// <summary>
        /// The position of the main element
        /// </summary>
        [DefaultValue(Position.LeftTop), Category("Appearance")]
        public Position My
        {
            get
            {
                if(ViewState["My"] != null)
                    return (Position)ViewState["My"];
                else
                    return Position.LeftTop;
            }
            set
            {
                if(value != Position.LeftTop)
                    ViewState["My"] = value;
                else
                    ViewState.Remove("My");
            }
        }

        /// <summary>
        /// The position relative to the element specified in <see cref="Of">Of</see>
        /// </summary>
        [DefaultValue(Position.LeftBottom), Category("Appearance")]
        public Position At
        {
            get
            {
                if(ViewState["At"] != null)
                    return (Position)ViewState["At"];
                else
                    return Position.LeftBottom;
            }
            set
            {
                if(value != Position.LeftBottom)
                    ViewState["At"] = value;
                else
                    ViewState.Remove("At");
            }
        }

        /// <summary>
        /// The control to position against
        /// </summary>
        [DefaultValue(null), Category("Appearance"), IDReferenceProperty()]
        public string Of
        {
            get
            {
                if(ViewState["Of"] != null)
                    return (string)ViewState["Of"];
                else
                    return null;
            }
            set
            {
                if(value != null)
                    ViewState["Of"] = value;
                else
                    ViewState.Remove("Of");
            }
        }
    
        /// <summary>
        /// When a position element overflows in some direction, move it an alternative position
        /// When two are specified Horzontal/Vertical
        /// </summary>
        [DefaultValue(Collision.None), Category("Appearance")]
        public Collision Collision
        {
            get
            {
                if(ViewState["Collision"] != null)
                    return (Collision)ViewState["Collision"];
                else
                    return Collision.None;
            }
            set
            {
                if(value != Collision.None)
                    ViewState["Collision"] = value;
                else
                    ViewState.Remove("Collision");
            }
        }
    
        /// <summary>
        /// Returns a string containing the options as a javascript object.
        /// </summary>
        public string JSOption(Control parent = null)
        {
            if(string.IsNullOrEmpty(Of))
                return string.Format("{{my:\"{0}\",at:\"{1}\",collision:\"{2}\"}}", PositionStrings[(int)My], PositionStrings[(int)At], CollisionStrings[(int)Collision]);
            else
            {
                if(parent != null)
                {
                    Control c = Util.FindControlRecursiveOut(parent, Of, null);
                    if(c != null)
                        return string.Format("{{my:\"{0}\",at:\"{1}\",of:\"{2}\",collision:\"{3}\"}}", PositionStrings[(int)My], PositionStrings[(int)At], c.ClientID, CollisionStrings[(int)Collision]);
                    else
                        return string.Format("{{my:\"{0}\",at:\"{1}\",of:\"{2}\",collision:\"{3}\"}}", PositionStrings[(int)My], PositionStrings[(int)At], Of, CollisionStrings[(int)Collision]);
                }
                else
                    return string.Format("{{my:\"{0}\",at:\"{1}\",of:\"{2}\",collision:\"{3}\"}}", PositionStrings[(int)My], PositionStrings[(int)At], Of, CollisionStrings[(int)Collision]);
            }
        }

        private static readonly string[] PositionStrings = new string[] { "'top'", "'center'", "'bottom'", "'left top'", "'left'", "'left bottom'", "'right top'", "'right'", "'right bottom'" };
        private static readonly string[] CollisionStrings = new string[] {"'none'","'flip'", "'fit'", "'none flip'", "'none fit'", "'flip none'", "'flip fit'", "'fit none'", "'fit flip'" };
    }
}
