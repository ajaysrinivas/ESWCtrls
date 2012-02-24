using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.Design;
using System.Drawing.Design;
using System.Collections;

namespace ESWCtrls
{
    /// <summary>
    /// Adds a sorting column to the datagrid.
    /// </summary>
    public class SortColumn : DataGridColumn
    {
        /// <summary>
        /// The text to show for the sort column
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("↑↓")]
        public string Text
        {
            get
            {
                if(ViewState["Text"] == null)
                    return "↑↓";
                else
                    return (string)ViewState["Text"];
            }
            set { ViewState["Text"] = value; }
        }

        /// <summary>
        /// The image to use for the sort column
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(null), UrlProperty(), Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
        public string ImageUrl
        {
            get
            {
                if(ViewState["ImageUrl"] == null)
                    return null;
                else
                    return (string)ViewState["ImageUrl"];
            }
            set { ViewState["ImageUrl"] = value; }
        }

        /// <summary>
        /// Whether to show the image or the text
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue(ImageText.Both)]
        public ImageText ImageText
        {
            get
            {
                if(ViewState["ImageText"] == null)
                    return ImageText.Both;
                else
                    return (ImageText)ViewState["ImageText"];
            }
            set { ViewState["ImageText"] = value; }
        }
    
        ///        
        public override void  Initialize()
        {
 	        base.Initialize();
            _rowIdx = 0;

            Script.AddResourceScript(Owner.Page, new string[] { "jquery.ui.sortable.js", "jquery.sortcolumn.js" });
            List<string> opts = new List<string>();
            if(!Owner.ShowHeader)
                opts.Add("hasHeader:false");
            if(Owner.ShowFooter)
                opts.Add("hasFooter:true");
            Script.AddStartupScript(Owner, Owner.ClientID + "_" + Owner.Columns.IndexOf(this).ToString(), "ls_sortcol", opts);
            
        }

        ///
        public override void  InitializeCell(TableCell cell, int columnIndex, ListItemType itemType)
        {
 	        base.InitializeCell(cell, columnIndex, itemType);

            if(itemType == ListItemType.Header || itemType == ListItemType.Footer || itemType == ListItemType.Pager || itemType == ListItemType.Separator)
                return;

            cell.CssClass += " sortCol";

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<span idx=\"{0}\">", _rowIdx);
            if(!string.IsNullOrEmpty(ImageUrl) && ImageText != ESWCtrls.ImageText.TextOnly)
                sb.AppendFormat("<img src=\"{0}\" alt=\"Sort Item\" />", cell.ResolveUrl(ImageUrl));
            
            if(!string.IsNullOrEmpty(Text) && ImageText != ESWCtrls.ImageText.ImageOnly)
                sb.AppendFormat( "{0}",Owner.Page.Server.HtmlEncode(Text));
            sb.Append("</span>");
            Literal l = new Literal();
            l.Text = sb.ToString();
            cell.Controls.Add(l);
            ++_rowIdx;
        }

        private int _rowIdx;
    }
}
