/*
* jQuery.ls_dropmenu.js - dropdown menu plugin, specifically to work with asp.net control
*
* version 1.0.0 (2011/08/28) (C)opyright Leon Pennington 2011
* 
* Dual licensed under the MIT and GPL licenses: 
*   http://www.opensource.org/licenses/mit-license.php 
*   http://www.gnu.org/licenses/gpl.html 
*/
;(function(a){a.fn.ls_dropmenu=function(b){var c=a.extend(true,{itemClass:null,openClass:null,hoverClass:null,topItemClass:null,topHoverClass:null,disabledClass:null,arrowDisplayType:"none",arrowDown:{image:null,"class":null},arrowRight:{image:null,"class":null},showEffect:null,hideEffect:null},b);this.each(function(){function h(a,b){if(a.children("a").length>0)a.children("a").append('<img src="'+b.image+'" alt="Open Arrow" class="'+b.class+'" />');else a.append('<img src="'+b.image+'" alt="Open Arrow" class="'+b.class+'" />')}function g(a){a.children("ul").stop(true,true);if(c.hideEffect!=null)a.children("ul").hide(c.hideEffect.effect,c.hideEffect.options,c.hideEffect.speed,c.hideEffect.callback);else a.children("ul").css("display","none");a.attr("open",false).removeClass(function(){return this.oclass})}function f(d){var e=a(d);var f=e.children("ul");var h=e.attr("open");if(f.length>0&&e.attr("open")==undefined){e.attr("open",true);e.addClass(d.oclass);var i=true;var j=e.offset();if(e.css("display")=="inline"){if(e.css("position")=="static")j.top+=e.height();else{j.left=0;j.top=e.height()}}else if(e.css("float")!="none")j.top+=e.height();else{j.left=e.width();if(e.css("position")=="static")j.top=e.position().top;else j.top=0;i=false}f.css({left:j.left,top:j.top});if(c.showEffect!=null)f.show(c.showEffect.effect,c.showEffect.options,c.showEffect.speed,c.showEffect.callback);else f.css("display","block")}g(b.find("li[open]").not(e.parents("ul").parent().add(d)))}var b=a(this);var d=false;var e=null;b.click(function(a){a.stopPropagation()});b.find("ul").css({visibility:"hidden",display:"block"}).css("width",function(){return a(this).width()+"px"}).css({visibility:"",display:"none"});b.children("li").click(function(a){if(!d){d=true;f(this)}}).each(function(a,b){b.topLevel=true});b.find("li").each(function(d,e){var f=e.id.replace(b.attr("id"),"");var g="";var h="";if(c.hoverClass!=null)g=c.hoverClass;if(c.openClass!=null)h=c.openClass;if(e.topLevel){if(c.topHoverClass!=null)g+=" "+c.topHoverClass;if(c.topOpenClass!=null)h+=" "+c.topOpenClass}var i=a(e);if(i.attr("hoverclass")!=null)g+=" "+i.attr("hoverclass");if(i.attr("openclass")!=null)h+=" "+i.attr("openclass");e.hclass=a.trim(g);e.oclass=a.trim(h)}).hover(function(b){a(this).addClass(this.hclass);var c=this;if(d){clearTimeout(e);e=setTimeout(function(){f(c)},200)}b.stopPropagation()},function(b){a(this).removeClass(this.hclass)});if(c.arrowDisplayType!="none"){b.find("li > ul").parent().each(function(){if(this.topLevel){if(c.arrowDisplayType=="subonly")return}else if(c.arrowDisplayType=="toponly")return;var b=a(this);if(b.css("display")=="inline"||b.css("float")!="none"){if(c.arrowDown.image!=null)h(b,c.arrowDown)}else if(c.arrowRight.image!=null)h(b,c.arrowRight)})}a(document).click(function(){if(d)e=setTimeout(function(){g(b.find("li[open]"));d=false},100)});})}})(jQuery);