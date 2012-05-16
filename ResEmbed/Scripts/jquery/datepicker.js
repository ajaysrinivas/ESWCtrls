/*
* jquery.ls_datepicker.js - additional function for use in datepicker
*
* version 1.0.0 (2011/09/05) (C)opyright Leon Pennington 2011
* 
* Dual licensed under the MIT and GPL licenses: 
*   http://www.opensource.org/licenses/mit-license.php 
*   http://www.gnu.org/licenses/gpl.html 
*/
function ls_dp_close(dateText, inst, typed, allowBlank, minCtrl, minMode, maxCtrl, maxMode)
{
	var selected = $("#" + inst.id).datepicker("getDate");
		
	if (!allowBlank)
	{
		if (dateText == '')
		{
			$("#" + inst.id).datepicker("setDate", inst.lastVal);
			return;
		}
	}
	
    if (minCtrl != null)
    {
        if (minMode == "Fixed")
            $(minCtrl).datepicker("option", "maxDate", dateText != '' ? selected : null);
        else
        {
            if (dateText != '' && selected > $(minCtrl).datepicker("getDate"))
                $(minCtrl).datepicker("setDate", selected);
        }
    }

    if (maxCtrl != null)
    {
        if (maxMode == "Fixed")
            $(maxCtrl).datepicker("option", "minDate", dateText != '' ? selected : null);
        else
        {
            if (dateText != '' && selected < $(maxCtrl).datepicker("getDate"))
                $(maxCtrl).datepicker("setDate", selected);
        }
    }
}