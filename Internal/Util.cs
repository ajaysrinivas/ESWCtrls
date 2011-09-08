using System.Drawing;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ESWCtrls.Internal
{
    internal static class Util
    {
        #region Styles

        public static void addStyleSheet(string css, string key, Page currentPage, WebControl control)
        {
            ControlCollection ctrls = currentPage.Controls;
            if (currentPage.Master != null) 
                ctrls = currentPage.Master.Controls;

            foreach (Control ctrl in ctrls)
            {
                if (ctrl.GetType().Name == "HtmlHead")
                {
                    ctrls = ctrl.Controls;
                    break;
                }
            }

            if (key != null)
            {
                foreach (Control ctrl in ctrls)
                {
                    if (ctrl.ID == key) 
                        return;
                }
            }

            string url = currentPage.ClientScript.GetWebResourceUrl(control.GetType(), "ESWCtrls.ResEmbed.Styles." + css);
            HtmlLink link = new HtmlLink();
            link.Attributes.Add("type", "text/css");
            link.Attributes.Add("rel", "stylesheet");
            link.Attributes.Add("media", "screen");
            link.Href = url;
            link.ID = key;

            ctrls.Add(new LiteralControl("\n"));
            ctrls.Add(link);
        }

        /// <summary>
        /// Registers a style with the pages header, and js attributes
        /// </summary>
        /// <param name="ctrl">The control that uses this style</param>
        /// <param name="style">The style to register</param>
        /// <param name="name">The name to give it for js</param>
        public static void RegisterStyle(Control ctrl, System.Web.UI.WebControls.Style style, string name)
        {
            if (!style.IsEmpty)
            {
                ctrl.Page.Header.StyleSheet.RegisterStyle(style, ctrl);
                string cssClass = style.RegisteredCssClass;
                if (!string.IsNullOrEmpty(style.CssClass))
                    cssClass = style.CssClass + " " + cssClass;

                ScriptManager.RegisterExpandoAttribute(ctrl, ctrl.ClientID, name, cssClass, false);
            }
        }

        #endregion

        #region general

        /// <summary>
        /// Check to see if main is in the array of objects provided
        /// </summary>
        public static bool IsIn(object main, params object[] parts)
        {
            foreach (object p1 in parts)
            {
                if (p1.Equals(main))
                    return true;
            }
            return false;
        }

        //Multi comparison case insensitive
        public static bool msCompCI(string c1, params string[] ca)
        {
            foreach (string c2 in ca)
            {
                if (c1.ToLower() == c2.ToLower())
                    return true;
            }
            return false;
        }

        //Finds a control recursivly within a parent
        public static Control FindControlRecursive(Control Parent, string ID)
        {
            if (Parent.ID == ID)
            {
                return Parent;
            }

            foreach (Control child in Parent.Controls)
            {
                Control ctrl = FindControlRecursive(child, ID);
                if (ctrl != null)
                {
                    return ctrl;
                }
            }

            return null;
        }

        // Finds a control first within the parent, then proceeds up the tree skipping the bits its already checked
        public static Control FindControlRecursiveOut(Control parent, string ID, Control check)
        {
            if (parent.ID == ID) return parent;

            foreach (Control child in parent.Controls)
            {
                if (child.Equals(check)) continue;
                Control ctrl = FindControlRecursive(child, ID);
                if (ctrl != null)
                {
                    return ctrl;
                }
            }

            if (parent.Parent != null)
            {
                return FindControlRecursiveOut(parent.Parent, ID, parent);
            }
            else
            {
                return null;
            }
        }

        // Finds if a controls parent tree has an update panel
        public static bool InUpdatePanel(Control ctrl)
        {
            if(ctrl is UpdatePanel)
                return true;

            Control parent = ctrl.Parent;
            while(parent != null)
            {
                if(parent is UpdatePanel)
                    return true;
                parent = parent.Parent;
            }

            return false;
        }

        #endregion

        #region Fonts

        public static System.Drawing.Font ConvertFont(FontInfo FontInfo)
        {
            if (FontInfo == null) return new Font("Verdana", 12, FontStyle.Regular, GraphicsUnit.Pixel);

            string name = FontInfo.Name;
            if (string.IsNullOrEmpty(name)) name = "Verdana";

            float size = 12;
            GraphicsUnit unitType = GraphicsUnit.Pixel;

            if (!FontInfo.Size.IsEmpty)
            {
                size = (float)FontInfo.Size.Unit.Value;
                switch (FontInfo.Size.Unit.Type)
                {
                    case UnitType.Em:
                    case UnitType.Ex:
                    case UnitType.Percentage:
                    case UnitType.Pica:
                        size = 12;
                        break;
                    case UnitType.Pixel:
                        unitType = GraphicsUnit.Pixel;
                        break;
                    case UnitType.Point:
                        unitType = GraphicsUnit.Point;
                        break;
                    case UnitType.Mm:
                        unitType = GraphicsUnit.Millimeter;
                        break;
                    case UnitType.Cm:
                        unitType = GraphicsUnit.Millimeter;
                        size *= 10;
                        break;
                    case UnitType.Inch:
                        unitType = GraphicsUnit.Inch;
                        break;
                }
            }

            FontStyle style = FontStyle.Regular;
            if (FontInfo.Bold) style = FontStyle.Bold;
            if (FontInfo.Underline) style = style | FontStyle.Underline;
            if (FontInfo.Strikeout) style = style | FontStyle.Strikeout;
            if (FontInfo.Italic) style = style | FontStyle.Italic;


            return new Font(name, size, style, unitType);
        }

        #endregion

    }
}
