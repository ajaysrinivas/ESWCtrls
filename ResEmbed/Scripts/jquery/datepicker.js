/*
* jquery.ls_datepicker.js - additional function for use in datepicker
*
* version 1.1.0 (2012/05/17) (C)opyright Leon Pennington 2012
* 
* Dual licensed under the MIT and GPL licenses: 
*   http://www.opensource.org/licenses/mit-license.php 
*   http://www.gnu.org/licenses/gpl.html 
*/
function ls_dp_close(a,b,c,d,e,f,g,h){if(!d&&(a==null||a=="")){$("#"+b.id).val(b.lastVal);return}var i=$("#"+b.id).datepicker("getDate");if(e!=null){if(f=="Fixed")$(e).datepicker("option","maxDate",i);if($(e).val()=="")$(e).val(a);else{var j=$(e).datepicker("getDate");if(j>i)$(e).val(a)}}if(g!=null){if(h=="Fixed")$(g).datepicker("option","minDate",i);if($(g).val()=="")$(g).val(a);else{var k=$(g).datepicker("getDate");if(k<i)$(g).val(a)}}}