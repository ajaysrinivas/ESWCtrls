/*
* jQuery.ls_popup.js - popup plugin, specifically to work with asp.net control
*
* version 1.1.0 (2011/09/30) (C)opyright Leon Pennington 2011
* 
* Dual licensed under the MIT and GPL licenses: 
*   http://www.opensource.org/licenses/mit-license.php 
*   http://www.gnu.org/licenses/gpl.html 
*/
;(function($){function hidePopup(popup,opts){function hideBackground(){if(opts.usejqueryoverlay){var a=$(".ui-widget-overlay");if(a.length>0){if(opts.bgd_zindex>0)a.css("z-index",opts.bgd_zindex);else a.remove()}}else{var a=$("#ls_popup_bgd").first();if(a.length>0){var b=a.data("using");var c=$.inArray(popup[0],b);if(c>=0)b.splice(c,1);if(b.length>0){a.css("z-index",0);for(var c=0;c<b.length;++c){if($(b[c]).css("z-index")-1>a.css("z-index"))a.css("z-index",$(b[c]).css("z-index")-1)}}else a.css("display","none");a.data("using",b)}}}function postHide(){$(window).unbind(".ls_popup_"+popup.attr("id"));popup.unbind("resize");var _shownHid=$("#"+popup.attr("id")+"_shown");_shownHid.val("false");if(opts.modal)hideBackground();if(opts.hideeffect!=null&&opts.hideeffect.callback!=null)opts.hideeffect.callback();if(opts.afterclose!=null)eval(opts.afterclose())}if(opts.beforeclose!=null){if(!opts.beforeclose())return}if(opts.hideeffect!=null)popup.hide(opts.hideeffect.effect,opts.hideeffect.options,opts.hideeffect.speed,postHide);else{popup.css("display","none");postHide()}}function showPopup(a,b,c){function f(){a.position(b.position);var c=a.offset();if(c.top<0)c.top=0;if(c.left<0)c.left=0;a.offset(c)}function e(){if(b.usejqueryoverlay){var c=$(".ui-widget-overlay");if(c.length>0){b.bgd_zindex=c.css("z-index");c.css("z-index",a.css("z-index")-1)}else{$("<div class='ui-widget-overlay'></div>").insertBefore(a);var c=$(".ui-widget-overlay");b.bgd_zindex=0;c.css({"z-index":a.css("z-index")-1,position:"fixed",width:"100%",height:"100%"})}}else{var c=$("#ls_popup_bgd").first();if(c.length>0){c.css("display","block");var d=c.data("using");if($.inArray(a[0],d)<0)d.push(a[0]);c.css("z-index",0);for(var e=0;e<d.length;++e){if($(d[e]).css("z-index")-1>c.css("z-index"))c.css("z-index",$(d[e]).css("z-index")-1)}c.data("using",d)}else{$("<div id='ls_popup_bgd'></div>").insertBefore(a);var c=$("#ls_popup_bgd").first();c.css({zIndex:a.css("z-index")-1,position:"fixed",left:"0px",top:"0px",backgroundColor:"#000000",width:"100%",height:"100%",opacity:"0.5"});var d=[];d.push(a[0]);c.data("using",d)}}}function d(){var c=$("#"+a.attr("id")+"_shown");c.val("true");$(window).bind("resize.ls_popup_"+a.attr("id"),function(a){f()});a.bind("resize",function(a){f()});if(b.scrolling)$(window).bind("scroll.ls_popup_"+a.attr("id"),function(a){f()});if(b.showeffect!=null&&b.showeffect.callback!=null)b.showeffect.callback();if(b.afteropen!=null)b.afteropen()}if(b.beforeopen!=null){if(!b.beforeopen())return}a.css("display","block");a.css("position","absolute");f();if(b.modal)e();if((c==null||!c)&&b.showeffect!=null){a.css("display","none");a.show(b.showeffect.effect,b.showeffect.options,b.showeffect.speed,d)}else d();}var ls_popup_methods={init:function(a){var b=$.extend(true,{modal:false,position:{of:window},moveelement:null,usejqueryoverlay:false,scrolling:true,open:false,opentriggers:null,closetriggers:null,beforeopen:null,afteropen:null,beforeclose:null,afterclose:null,showeffect:null,hideeffect:null},a);return this.each(function(){function k(){var a=$("body")[0];if(typeof a.onselectstart!=null)a.onselectstart=null;else{a.style.MozUserSelect="auto";a.style.webkitUserSelect="auto"}}function j(){if(window.getSelection)window.getSelection().removeAllRanges();else if(document.selection)document.selection.empty();var a=$("body")[0];if(typeof a.onselectstart!=null)a.onselectstart=function(){return false};else{a.style.MozUserSelect="none";a.style.webkitUserSelect="none"}}function i(b){$(document).unbind(".ls_popup_"+a.attr("id"));k()}function h(c){var d={x:c.pageX,y:c.pageY};c.cancelBubble=true;c.returnValue=false;var e=a.offset();var f=e.left+(d.x-b.mousepos.x);var g=e.top+(d.y-b.mousepos.y);b.mousepos=d;a.offset({left:f<0?0:f,top:g<0?0:g});$(window).unbind(".ls_popup_"+a.attr("id"))}var a=$(this);a.data("opts",b);var c=$("#"+a.attr("id")+"_shown");if(c.val()=="true")showPopup(a,b,true);else if(b.open)showPopup(a,b);b.mousepos=null;if(b.opentriggers!=null){for(var d=0;d<b.opentriggers.length;++d){var e=b.opentriggers[d];var f=$("#"+e.control);if(f.length>0){f.get(0).cancelEvent=e.cancel;f.bind(e.event,function(c){showPopup(a,b);if(this.cancelEvent){if(c&&c.preventDefault)c.preventDefault();return false}})}}}if(b.closetriggers!=null){for(var d=0;d<b.closetriggers.length;++d){var e=b.closetriggers[d];var f=$("#"+e.control);if(f.length>0){f.get(0).cancelEvent=e.cancel;f.bind(e.event,function(c){hidePopup(a,b);if(this.cancelEvent){if(c&&c.preventDefault)c.preventDefault();return false}})}}}if(b.moveelement!=null){var g=$("#"+b.moveelement);if(g!=null){g.css("cursor","move");g.mousedown(function(c){b.mousepos={x:c.pageX,y:c.pageY};$(document).bind("mousemove.ls_popup_"+a.attr("id"),h);$(document).bind("mouseup.ls_popup_"+a.attr("id"),i);c.preventDefault();j()})}}})},show:function(a){return this.each(function(){var b=$(this);var c=b.data("opts");showPopup(b,c,a)})},hide:function(){return this.each(function(){var a=$(this);var b=a.data("opts");hidePopup(a,b)})},option:function(a,b){return this.each(function(){var c=$(this);var d=c.data("opts");if(b!=null){d[a]=b;c.data("opts",d)}else return d[a]})}};$.fn.ls_popup=function(a){if(ls_popup_methods[a])return ls_popup_methods[a].apply(this,Array.prototype.slice.call(arguments,1));else if(typeof a=="object"||!a)return ls_popup_methods.init.apply(this,arguments);else $.error("Method "+a+" does not exists in ls_popup")}})(jQuery);