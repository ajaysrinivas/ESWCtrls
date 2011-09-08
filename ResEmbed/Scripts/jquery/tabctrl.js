/*
* jQuery.ls_tabctrl.js - tab ctrl plugin, specifically to work with asp.net control
*
* version 1.0.0 (2011/08/28) (C)opyright Leon Pennington 2011
* 
* Dual licensed under the MIT and GPL licenses: 
*   http://www.opensource.org/licenses/mit-license.php 
*   http://www.gnu.org/licenses/gpl.html 
*/
;(function(a){a.fn.ls_tabctrl=function(b){var c=a.extend(true,{useImages:false,activeClass:null,inactiveClass:null,left:{active:null,inactive:null},middle:{active:null,inactive:null},right:{active:null,inactive:null}},b);return this.each(function(){function g(){var b=e.val();if(this.idx==b)return false;if(c.useImages){a("#"+d+"_ld_"+b).addClass(c.inactiveClass).removeClass(c.activeClass).css("background-image",c.left.inactive);a("#"+d+"_rd_"+b).addClass(c.inactiveClass).removeClass(c.activeClass).css("background-image",c.right.inactive);a("#"+d+"_btn_"+b).addClass(c.inactiveClass).removeClass(c.activeClass).css("background-image",c.middle.inactive);a("#"+d+"_ld_"+this.idx).addClass(c.activeClass).removeClass(c.inactiveClass).css("background-image",c.left.active);a("#"+d+"_rd_"+this.idx).addClass(c.activeClass).removeClass(c.inactiveClass).css("background-image",c.right.active);a(this).addClass(c.activeClass).removeClass(c.inactiveClass).css("background-image",c.middle.active)}else{a("#"+d+"_btn_"+b).parent().removeClass(c.activeClass).addClass(c.inactiveClass);a(this).parent().removeClass(c.inactiveClass).addClass(c.activeClass)}a("#"+d+"_page_"+b).css("display","none");a("#"+d+"_page_"+this.idx).css("display","block");e.val(a(this).attr("oa"))}var b=a(this);var d=b.attr("id");var e=a("#"+d+"_actIdx");var f=a("#"+d+"_tabbar a");f.each(function(){var b=a(this);this.idx=b.attr("oa");b.attr("href",null);b.css("cursor","pointer");b.bind("click",g)});})}})(jQuery);