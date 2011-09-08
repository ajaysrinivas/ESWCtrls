/*
* jQuery.ls_popup.js - popup plugin, specifically to work with asp.net control
*
* version 1.0.0 (2011/08/27) (C)opyright Leon Pennington 2011
* 
* Dual licensed under the MIT and GPL licenses: 
*   http://www.opensource.org/licenses/mit-license.php 
*   http://www.gnu.org/licenses/gpl.html 
*/
;(function($){
	$.fn.ls_popup = function(args) {
		var opts = $.extend(true,{
			modal:false,
			position:"center",
			relativeto:null,
			moveelement: null,
			margin:0,
			usejqueryoverlay: false,
			scrolling: true,
			open: false,
			
			opentriggers: null,
			closetriggers: null,
			
			beforeopen: null,
			afteropen: null,
			beforeclose: null,
			afterclose: null,
			
			showeffect: null,
			hideeffect: null,
		},args);
		
		this.each(function() {
			var _popup = $(this);
			
			var _shownHid = $("#" + _popup.attr("id") + "_shown");
			if(_shownHid.val() == "true")
				showPopup(true);
			else if(opts.open)
				showPopup();
			
			opts.mousepos = null;
				
			if(opts.opentriggers != null)
			{
				for(var i = 0; i < opts.opentriggers.length; ++i)
				{
					var trig = opts.opentriggers[i];
					var ctrl = $("#" + trig.control);
					if(ctrl.length > 0)
					{
						ctrl.get(0).cancelEvent = trig.cancel;
						ctrl.bind(trig.event, function(evt) { 
							showPopup(); 
							if(this.cancelEvent) 
							{
								if(evt && evt.preventDefault)
									evt.preventDefault();
								return false;
							}
						});
					}
				}
			}
			
			if(opts.closetriggers != null)
			{
				for(var i = 0; i < opts.closetriggers.length; ++i)
				{
					var trig = opts.closetriggers[i];
					var ctrl = $("#" + trig.control);
					if(ctrl.length > 0)
					{
						ctrl.get(0).cancelEvent = trig.cancel;
						ctrl.bind(trig.event, function(evt) {
							hidePopup();
							if(this.cancelEvent) 
							{
								if(evt && evt.preventDefault)
									evt.preventDefault();
								return false;
							}
						});
					}
				}
			}
			
			if(opts.moveelement != null)
			{
				var moveEl = $("#" + opts.moveelement);
				if(moveEl != null)
				{
					moveEl.css("cursor","move");
					moveEl.mousedown(function(evt) { 
						opts.mousepos = { x: evt.pageX, y: evt.pageY };
						$(document).bind("mousemove.ls_popup_" + _popup.attr("id"), mouseMove);
						$(document).bind("mouseup.ls_popup_" + _popup.attr("id"), mouseUp);
						evt.preventDefault();
						disableSelect();
						});
				}
			}
			
			function showPopup(skipEffect)
			{
				if(opts.beforeopen != null)
				{
					if(!opts.beforeopen())
						return;
				}
				
				_popup.css("display","block");
				_popup.css("position","absolute");
				
				setPosition();
				
				if(opts.modal)
					setBackground();
				
				if((skipEffect == null || !skipEffect) && opts.showeffect != null)
				{
					_popup.css("display","none");
					_popup.show(opts.showeffect.effect, opts.showeffect.options, opts.showeffect.speed, postShow );						
				}
				else
					postShow();
			};
			
			function postShow()
			{					
				_shownHid.val("true");
				
				$(window).bind("resize.ls_popup_" + _popup.attr("id"), function(event) { setPosition(); });
				_popup.bind("resize", function(event) { setPosition(); });
				
				if(opts.scrolling)
					$(window).bind("scroll.ls_popup_" + _popup.attr("id"), function(event) { setPosition(); });
				
				if(opts.showeffect != null && opts.showeffect.callback != null)
					opts.showeffect.callback();
				
				if(opts.afteropen != null)
					opts.afteropen();
			};
			
			function hidePopup()
			{
				if(opts.beforeclose != null)
				{
					if(!opts.beforeclose())
						return;
				}
				
				if(opts.hideeffect != null)
					_popup.hide(opts.hideeffect.effect, opts.hideeffect.options, opts.hideeffect.speed, postHide);
				else
				{
					_popup.css("display","none");
					postHide();
				}				
			};
			
			function postHide()
			{
				$(window).unbind(".ls_popup_" + _popup.attr("id"));
				_popup.unbind("resize");
				
				_shownHid.val("false");
				
				if(opts.modal)
					hideBackground();
			
				if(opts.hideeffect != null && opts.hideeffect.callback != null)
					opts.hideeffect.callback();
			
				if(opts.afterclose != null)
					eval(opts.afterclose());
			};
			
			function setPosition()
			{
				var posx = opts.margin;
				var posy = opts.margin;
				var posa = opts.position.split(",");				
				var winDim = { width: $(window).width(), height: $(window).height(), top: $(window).scrollTop(), left: $(window).scrollLeft() };
				
				if(opts.relativeto != null)
				{
					var rel = $("#" + opts.relativeto);
					var relpos = rel.offset();
					if(posa.length == 1)
					{
						if(isNaN(posa[0]))
						{
							switch(posa[0])
							{
								case "center":
									posx = (relpos.left + (rel.outerWidth() / 2)) - (_popup.outerWidth() / 2);
									posy = (relpos.top + (rel.outerHeight() / 2)) - (_popup.outerHeight() / 2);
									break;
								case "left":
									posx = relpos.left - (rel.outerWidth() + parseInt(opts.margin));
									posy = (relpos.top + (rel.outerHeight() / 2)) - (_popup.outerHeight() / 2);
									break;
								case "right":
									posx = relpos.left + rel.outerWidth() + parseInt(opts.margin);
									posy = (relpos.top + (rel.outerHeight() / 2)) - (_popup.outerHeight() / 2);
									break;
								case "top":
									posx = (relpos.left + (rel.outerWidth() / 2)) - (_popup.outerWidth() / 2);
									posy = relpos.top - (rel.outerHeight() + parseInt(opts.margin));
									break;
								case "bottom":
									posx = (relpos.left + (rel.outerWidth() / 2)) - (_popup.outerWidth() / 2);
									posy = relpos.top + rel.outerHeight() + parseInt(opts.margin);
									break;
							}
						}
						else
						{
							posx = relpos.left + parseInt(posa[0]);
							posy = relpos.top + parseInt(posa[0]);
						}
					}
					else
					{
						if(isNaN(posa[0]))
						{
							switch(posa[0])
							{
								case "center": posx = (relpos.left + (rel.outerWidth() / 2)) - (_popup.outerWidth() / 2); break;
								case "left": posx = relpos.left - (rel.outerWidth() + parseInt(opts.margin)); break;
								case "right": posx = relpos.left + rel.outerWidth() + parseInt(opts.margin); break;
							}
						}
						else
							posx = relpos.left + parseInt(posa[0]);
							
						if(isNaN(posa[1]))
						{
							switch(posa[1])
							{
								case "center": posy = (relpos.top + (rel.outerHeight() / 2)) - (_popup.outerHeight() / 2); break;
								case "top": posy = relpos.top - (rel.outerHeight() + parseInt(opts.margin));	break;
								case "bottom": posy = relpos.top + rel.outerHeight() + parseInt(opts.margin); break;
							}
						}
						else
							posy = relpos.top + parseInt(posa[1]);
					}
				}
				else
				{
					if(posa.length == 1)
					{
						if(isNaN(posa[0]))
						{
							switch(posa[0])
							{
								case "center":
									posx = ((winDim.width - _popup.outerWidth()) / 2) + winDim.left;
									posy = ((winDim.height - _popup.outerHeight()) / 2) + winDim.top;
									break;
								case "left":
									posx = winDim.left + opts.margin;
									posy = ((winDim.height - _popup.outerHeight()) / 2) + winDim.top;
									break;
								case "right":
									posx = ((winDim.width - _popup.outerWidth()) - opts.margin) + winDim.left;
									posy = ((winDim.height - _popup.outerHeight()) / 2) + winDim.top;
									break;
								case "top":
									posx = ((winDim.width - _popup.outerWidth()) / 2) + winDim.left;
									posy = winDim.top + opts.margin;
									break;
								case "bottom":
									posx = ((winDim.width - _popup.outerWidth()) / 2) + winDim.left;
									posy = ((winDim.height - _popup.outerHeight()) - opts.margin) + winDim.top;
									break;
							}
						}
						else
						{
							posx = posa[0] + winDim.left;
							posy = posa[0] + winDim.top;
						}
					}
					else
					{
						if(isNaN(posa[0]))
						{
							switch(posa[0])
							{
								case "center": posx = ((winDim.width - _popup.outerWidth()) / 2) + winDim.left; break;
								case "left": posx = winDim.left + opts.margin; break;
								case "right": posx = ((winDim.width - _popup.outerWidth()) - opts.margin) + winDim.left; break;
							}
						}
						else
							posx = posa[0] + winDim.left;
							
						if(isNaN(posa[1]))
						{
							switch(posa[1])
							{
								case "center": posy = ((winDim.height - _popup.outerHeight()) / 2) + winDim.top; break;
								case "top": posy = winDim.top + opts.margin; break;
								case "bottom": posy = ((winDim.height - _popup.outerHeight()) - opts.margin) + winDim.top; break;
							}
						}
						else
							posy = posa[0] + winDim.top;
					}
					
					if(_popup.outerHeight() > winDim.height)
						posy = 0;
				}
			
				_popup.offset({ top: posy, left: posx });
			};
		
			function setBackground()
			{
				if(opts.usejqueryoverlay)
				{
					var bgd = $(".ui-widget-overlay");
					if(bgd.length > 0)
					{
						opts.bgd_zindex = bgd.css("z-index");
						bgd.css("z-index", _popup.css("z-index") - 1);
					}
					else
					{
						$("<div class='ui-widget-overlay'></div>").insertBefore(_popup);						
						var bgd = $(".ui-widget-overlay");
						opts.bgd_zindex = 0;
						bgd.css({
							"z-index":_popup.css("z-index") - 1,
							"position":"fixed",
							"width":"100%",
							"height":"100%"
						});
					}
				}
				else
				{
					var bgd = $("#ls_popup_bgd").first();
					if(bgd.length > 0)
					{
						bgd.css("display","block");
						var using = bgd.data("using");
						if($.inArray(_popup,using) < 0)
							using.push(_popup);
							
						bgd.css("z-index",0);
						for(var i = 0; i < using.length; ++i)
						{
							if((using[i].css("z-index") - 1) > bgd.css("z-index"))
								bgd.css("z-index",using[i].css("z-index") - 1);
						}
						
						bgd.data("using",using);
					}
					else
					{						
						$("<div id='ls_popup_bgd'></div>").insertBefore(_popup);
						var bgd = $("#ls_popup_bgd").first();
						bgd.css({
							zIndex:_popup.css("z-index") - 1,
							position:"fixed", left:"0px", top:"0px", backgroundColor:"#000000",
							width:"100%",height:"100%",opacity:"0.5"
						});
						var using = [];
						using.push(_popup);
						bgd.data("using", using );
					}
				}
			};
		
			function hideBackground()
			{
				if(opts.usejqueryoverlay)
				{
					var bgd = $(".ui-widget-overlay");
					if(bgd.length > 0)
					{
						if(opts.bgd_zindex > 0)
							bgd.css("z-index", opts.bgd_zindex);
						else
							bgd.remove();
					}
				}
				else
				{				
					var bgd = $("#ls_popup_bgd").first();
					if(bgd.length > 0)
					{
						var using = bgd.data("using");
						var i = $.inArray(_popup, using);
						if( i >= 0)
							using.splice(i,1);
						if(using.length > 0)
						{
							bgd.css("z-index",0);
							for(var i = 0; i < using.length; ++i)
							{
								if((using[i].css("z-index") - 1) > bgd.css("z-index"))
									bgd.css("z-index",using[i].css("z-index") - 1);
							}
						}
						else
							bgd.css("display","none");
						bgd.data("using",using);
					}
				}
			};
		
			function mouseMove(evt)
			{
				var mpos = { x: evt.pageX, y: evt.pageY };
				evt.cancelBubble = true;
				evt.returnValue = false;
					
				var pos = _popup.offset();
				var mx = pos.left + (mpos.x - opts.mousepos.x);
				var my = pos.top + (mpos.y - opts.mousepos.y);
				opts.mousepos = mpos;
				
				_popup.offset({ left: (mx < 0 ? 0 : mx), top: (my < 0 ? 0 : my) });
				$(window).unbind(".ls_popup_" + _popup.attr("id"));
			};
			
			function mouseUp(evt)
			{
				$(document).unbind(".ls_popup_" + _popup.attr("id"));
				enableSelect();
			};
			
			function disableSelect()
			{
                if(window.getSelection)
                    window.getSelection().removeAllRanges();
                else if(document.selection)
                    document.selection.empty();  
                var body = $("body")[0];
                if(typeof body.onselectstart != null)
                    body.onselectstart = function(){return false;};
                else
				{
                    body.style.MozUserSelect = "none";
                    body.style.webkitUserSelect = "none";
                }
            };
            
            function enableSelect()
			{
                var body = $("body")[0];
                if(typeof body.onselectstart != null)
                    body.onselectstart = null;
                else
				{
                    body.style.MozUserSelect = "auto";
                    body.style.webkitUserSelect = "auto";
                }
            };
		});
	};
})(jQuery);