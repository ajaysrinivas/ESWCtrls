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
	var selected = $.datepicker.formatDate(inst.settings.dateFormat,new Date(inst.selectedYear,inst.selectedMonth,inst.selectedDay));
	if(dateText != selected)
	{
		if(!allowBlank)
		{
			if(dateText=='')
			{
				$("#" + inst.id).datepicker("setDate",inst.lastVal);
				return;
			}
		}
		
		if(dateText != '')
		{
			var td;
			try
			{
				td = $.datepicker.parseDate(inst.settings.dateFormat,dateText);
			}
			catch(e)
			{
				$("#" + inst.id).datepicker("setDate",inst.lastVal);
				return;
			}
			
			if(inst.settings.minDate != null && td < $.datepicker.parseDate(inst.settings.dateFormat,inst.settings.minDate))
				td = $.datepicker.parseDate(inst.settings.dateFormat,inst.settings.minDate);
				
			if(inst.settings.maxDate != null && td > $.datepicker.parseDate(inst.settings.dateFormat,inst.settings.maxDate))
				td = $.datepicker.parseDate(inst.settings.dateFormat,inst.settings.maxDate);
				
			dateText = $.datepicker.formatDate(inst.settings.dateFormat, td);
			$("#" + inst.id).datepicker("setDate", td);
		}
	}
	
	var cd = $("#" + inst.id).datepicker("getDate");
	if(minCtrl != null)
	{
		if(minMode == "Fixed")
			$(minCtrl).datepicker("option","minDate",dateText!=''?dateText:null);
		else
		{
			if(cd != null && cd > $(minCtrl).datepicker("getDate"))
				$(minCtrl).datepicker("setDate",cd);
		}
	}
	
	if(maxCtrl != null)
	{
		if(maxMode == "Fixed")
			$(maxCtrl).datepicker("option","minDate",dateText!=''?dateText:null);
		else
		{
			if(cd != null && cd < $(maxCtrl).datepicker("getDate"))
				$(maxCtrl).datepicker("setDate",cd);
		}
	}
}