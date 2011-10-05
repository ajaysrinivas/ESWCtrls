/*
* jquery.ls_datepicker.js - additional function for use in datepicker
*
* version 1.0.0 (2011/09/05) (C)opyright Leon Pennington 2011
* 
* Dual licensed under the MIT and GPL licenses: 
*   http://www.opensource.org/licenses/mit-license.php 
*   http://www.gnu.org/licenses/gpl.html 
*/
;function ls_dp_close(a,b,c,d,e,f,g,h){var i=$.datepicker.formatDate(b.settings.dateFormat,new Date(b.selectedYear,b.selectedMonth,b.selectedDay));if(a!=i){if(!d){if(a==""){$("#"+b.id).datepicker("setDate",b.lastVal);return}}if(a!=""){var j;try{j=$.datepicker.parseDate(b.settings.dateFormat,a)}catch(k){$("#"+b.id).datepicker("setDate",b.lastVal);return}if(b.settings.minDate!=null&&j<$.datepicker.parseDate(b.settings.dateFormat,b.settings.minDate))j=$.datepicker.parseDate(b.settings.dateFormat,b.settings.minDate);if(b.settings.maxDate!=null&&j>$.datepicker.parseDate(b.settings.dateFormat,b.settings.maxDate))j=$.datepicker.parseDate(b.settings.dateFormat,b.settings.maxDate);a=$.datepicker.formatDate(b.settings.dateFormat,j);$("#"+b.id).datepicker("setDate",j)}}var l=$("#"+b.id).datepicker("getDate");if(e!=null){if(f=="Fixed")$(e).datepicker("option","minDate",a!=""?a:null);else{if(l!=null&&l>$(e).datepicker("getDate"))$(e).datepicker("setDate",l)}}if(g!=null){if(h=="Fixed")$(g).datepicker("option","minDate",a!=""?a:null);else{if(l!=null&&l<$(g).datepicker("getDate"))$(g).datepicker("setDate",l)}}};