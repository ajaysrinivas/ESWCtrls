using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESWCtrls
{
    using Internal;
    using System.ComponentModel;
    using System.Web.UI;
    using System.Drawing;

    /// <summary>
    /// Used by controls for position elements or the control itself
    /// </summary>
    public class Positioning : ViewStateBase
    {
        /// <summary>
        /// The position of the main element
        /// </summary>
        [DefaultValue(Position.CenterCenter), Category("Appearance")]
        public Position My
        {
            get
            {
                if(ViewState["My"] != null)
                    return (Position)ViewState["My"];
                else
                    return Position.CenterCenter;
            }
            set { ViewState["My"] = value; }
        }

        /// <summary>
        /// The position relative to the element specified in <see cref="Of">Of</see>
        /// </summary>
        [DefaultValue(Position.CenterCenter), Category("Appearance")]
        public Position At
        {
            get
            {
                if(ViewState["At"] != null)
                    return (Position)ViewState["At"];
                else
                    return Position.CenterCenter;
            }
            set
            { ViewState["At"] = value; }
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
            set { ViewState["Of"] = value; }
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
            set { ViewState["Collision"] = value; }
        }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        [Category("Appearance")]
        public Point Offset
        {
            get
            {
                if(ViewState["Offset"] != null)
                    return (Point)ViewState["Offset"];
                else
                    return new Point();
            }
            set { ViewState["Offset"] = value; }
        }

        /// <summary>
        /// Returns a string containing the options as a javascript object.
        /// </summary>
        public string JSOption(Control parent = null)
        {
            List<string> opts = new List<string>();

            if(My != Position.CenterCenter)
                opts.Add(string.Format("my:\"{0}\"", Constants.PositionStrings[(int)My]));
            if(At != Position.CenterCenter)
                opts.Add(string.Format("at:\"{0}\"", Constants.PositionStrings[(int)At]));

            if(Collision != Collision.None)
                opts.Add(string.Format("collision:\"{0}\"", Constants.CollisionStrings[(int)Collision]));

            if(!Offset.IsEmpty)
                opts.Add(string.Format("offset:\"{0} {1}\"", Offset.X, Offset.Y));

            if(string.IsNullOrEmpty(Of))
                opts.Add("of:window");
            else
            {
                if(parent != null)
                {
                    Control c = Util.FindControlRecursiveOut(parent, Of, null);
                    if(c != null)
                        opts.Add(string.Format("of:\"#{0}\"",c.ClientID));
                    else
                        opts.Add(string.Format("of:\"#{0}\"", Of));
                }
                else
                    opts.Add(string.Format("of:\"#{0}\"", Of));
            }

            return "{" + string.Join(",", opts) + "}";
        }
    }
}
