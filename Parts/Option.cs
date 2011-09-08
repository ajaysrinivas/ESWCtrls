using System.ComponentModel;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// Option value pair
    /// </summary>
    public class Option : ViewStateBase
    {
        /// <summary>The name of the option</summary>
        [Bindable(true), Category("Data"), DefaultValue(null)]
        public string name
        {
            get
            {
                if(ViewState["name"] != null)
                    return (string)ViewState["name"];
                else
                    return null;
            }
            set
            {
                if(value != null)
                    ViewState["name"] = value;
                else
                    ViewState.Remove("name");
            }
        }

        /// <summary>The value of the option</summary>
        [Bindable(true), Category("Data"), DefaultValue(null)]
        public string value
        {
            get
            {
                if(ViewState["value"] != null)
                    return (string)ViewState["value"];
                else
                    return null;
            }
            set
            {
                if(value != null)
                    ViewState["value"] = value;
                else
                    ViewState.Remove("value");
            }
        }
    }

    /// <summary>
    /// An options list
    /// </summary>
    public class OptionList : ViewStateListBase<Option>
    {
        /// <summary>marked as internal not to be created outside of this class</summary>
        internal OptionList() { }

        ///
        protected internal override Option Create(object state)
        {
            Option rst = new Option();
            rst.LoadViewState(state);
            return rst;
        }
    }
}
