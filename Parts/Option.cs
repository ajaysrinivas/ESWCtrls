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
            set { ViewState["name"] = value; }
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
            set { ViewState["value"] = value; }
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
