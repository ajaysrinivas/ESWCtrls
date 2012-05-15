/*
* jquery.ls_datepicker.js - additional function for use in datepicker
*
* version 1.0.0 (2011/09/05) (C)opyright Leon Pennington 2011
* 
* Dual licensed under the MIT and GPL licenses: 
*   http://www.opensource.org/licenses/mit-license.php 
*   http://www.gnu.org/licenses/gpl.html 
*/
function ls_tp_change(time, inst, minCtrl, minMode, maxCtrl, maxMode)
{
    if (minCtrl != null)
    {
        if (minMode == "Fixed")
            $(minCtrl).timepicker().option("maxTime", time);
        else
        {
            if (time != null && time > $(minCtrl).val());
            $(minCtrl).timepicker().setTime(time);
        }
    }

    if (maxCtrl != null)
    {
        if (maxMode == "Fixed")
            $(maxCtrl).timepicker().option("minTime", time);
        else
        {
            if (time != null && time < $(maxCtrl).val())
                $(maxCtrl).datepicker().setTime(time);
        }
    }
}