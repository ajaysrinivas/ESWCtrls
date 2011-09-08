using System;
using System.ComponentModel;
using System.Web.UI;

namespace ESWCtrls
{
    using Internal;

    /// <summary>
    /// Server side triggers for popups
    /// </summary>
    public class ServerSideTrigger : ViewStateBase
    {
        /// <summary>
        /// The ID of the control to trigger the event
        /// </summary>
        [DefaultValue(""), Category("Behaviour"), IDReferenceProperty()]
        public string ControlID
        {
            get
            {
                if (ViewState["ControlID"] == null)
                    return string.Empty;
                else
                    return (string)ViewState["ControlID"];
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    ViewState.Remove("ControlID");
                else
                    ViewState["ControlID"] = value;
            }
        }

        /// <summary>
        /// The controls event to trigger on
        /// </summary>
        [DefaultValue("")]
        public string Event
        {
            get
            {
                if (ViewState["Event"] == null)
                {
                    return string.Empty;
                }
                else
                {
                    return (string)ViewState["Event"];
                }
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    ViewState.Remove("Event");
                }
                else
                {
                    ViewState["Event"] = value;
                }
            }
        }

    }

    /// <summary>
    /// Client side triggers for popups
    /// </summary>
    public class ClientSideTrigger : ServerSideTrigger
    {

        /// <summary>
        /// Cancel the client side event that this trigger caught
        /// </summary>
        [DefaultValue(false)]
        public bool CancelEvent
        {
            get
            {
                if (ViewState["CancelEvent"] == null)
                    return false;
                else
                    return (bool)ViewState["CancelEvent"];
            }
            set
            {
                if (!value)
                    ViewState.Remove("CancelEvent");
                else
                    ViewState["CancelEvent"] = value;
            }
        }

    }

    /// <summary>
    /// TriggerList class for popups
    /// </summary>
    /// <remarks></remarks>
    public class TriggerList : ViewStateListBase<ServerSideTrigger>
    {
        ///
        protected internal override ServerSideTrigger Create(object state)
        {
            throw new NotImplementedException();
        }
    }
}
