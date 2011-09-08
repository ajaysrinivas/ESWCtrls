using System.ComponentModel;
using System.Web.UI;

namespace ESWCtrls
{

    /// <summary>
    /// The list menu control.
    /// </summary>
    /// <remarks>
    /// This control is for list menus, it uses a datatable for the items.
    /// 
    /// This class can also handle Url Events, for passing control and arguments between pages.
    /// </remarks>
    [ToolboxData("<{0}:ListMenu runat=\"server\" />")]
    public class ListMenu : Menu
    {
        #region Properties

        #region DataItems

        /// <summary>
        /// The current active item in the menu
        /// </summary>
        /// <returns>The active item or -1 for none</returns>
        [Bindable(true), Category("Data"), DefaultValue("")]
        public string ActiveItem
        {
            get
            {
                if (ViewState["ActiveItem"] == null)
                    return null;
                else
                    return (string)ViewState["ActiveItem"];
            }
            set
            {
                if (Items[value] != null)
                    ViewState["ActiveItem"] = value;
            }
        }

        #endregion

        #endregion
    }

}