/*
* jquery.ls_datepicker.js - additional function for use in datepicker
*
* version 1.1.0 (2012/05/17) (C)opyright Leon Pennington 2012
* 
* Dual licensed under the MIT and GPL licenses: 
*   http://www.opensource.org/licenses/mit-license.php 
*   http://www.gnu.org/licenses/gpl.html 
*/
function ls_dp_close(dateText, inst, mode, allowBlank, minCtrl, minMode, maxCtrl, maxMode)
{
	if(!allowBlank && (dateText == null || dateText == ''))
	{
		$("#" + inst.id).val(inst.lastVal);
		return;
	}

	var selected = $("#" + inst.id).datepicker("getDate");
	if(minCtrl != null)
	{
		if(minMode == "Fixed")
			$(minCtrl).datepicker("option", "maxDate", selected);
	
		if($(minCtrl).val() == '')
			$(minCtrl).val(dateText);
		else
		{
			var minVal = $(minCtrl).datepicker("getDate");
			if(minVal > selected)
				$(minCtrl).val(dateText);
		}
	}

	if(maxCtrl != null)
	{
		if(maxMode == "Fixed")
			$(maxCtrl).datepicker("option", "minDate", selected);
	
		if($(maxCtrl).val() == '')
			$(maxCtrl).val(dateText);
		else
		{
			var maxVal = $(maxCtrl).datepicker("getDate");
			if(maxVal < selected)
				$(maxCtrl).val(dateText);
		}
	}
}
