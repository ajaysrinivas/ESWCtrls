/*
* jQuery.ls_popup.js - popup plugin, specifically to work with asp.net control
*
* version 1.1.0 (2011/09/30) (C)opyright Leon Pennington 2011
* 
* Dual licensed under the MIT and GPL licenses: 
*   http://www.opensource.org/licenses/mit-license.php 
*   http://www.gnu.org/licenses/gpl.html 
*/
;(function($){
    var ls_popup_methods = {
        init: function(args) {
            var opts = $.extend(true,{
                modal:false,
                position:{of:window},
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
            
            return this.each(function() {
                var popup = $(this);
                popup.data("opts",opts);
                var shownHid = $("#" + popup.attr("id") + "_shown");
                
                if(shownHid.val() == "true")
                    showPopup(popup,opts,true);
                else if(opts.open)
                    showPopup(popup,opts);
                
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
                                showPopup(popup,opts);
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
                                hidePopup(popup,opts);
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
                            $(document).bind("mousemove.ls_popup_" + popup.attr("id"), mouseMove);
                            $(document).bind("mouseup.ls_popup_" + popup.attr("id"), mouseUp);
                            evt.preventDefault();
                            disableSelect();
                            });
                    }
                }
                
                function mouseMove(evt)
                {
                    var mpos = { x: evt.pageX, y: evt.pageY };
                    evt.cancelBubble = true;
                    evt.returnValue = false;
                        
                    var pos = popup.offset();
                    var mx = pos.left + (mpos.x - opts.mousepos.x);
                    var my = pos.top + (mpos.y - opts.mousepos.y);
                    opts.mousepos = mpos;
                    
                    popup.offset({ left: (mx < 0 ? 0 : mx), top: (my < 0 ? 0 : my) });
                    $(window).unbind(".ls_popup_" + popup.attr("id"));
                };

                function mouseUp(evt)
                {
                    $(document).unbind(".ls_popup_" + popup.attr("id"));
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
        },
        
        show: function(skipEffect) {
            return this.each(function() {
                var popup = $(this);
                var opts = popup.data("opts");
                showPopup(popup,opts,skipEffect);
            });
        },
        
        hide: function() {
            return this.each(function() {
                var popup = $(this);
                var opts = popup.data("opts");
                hidePopup(popup,opts);
            });
        },

        option: function(option, value) {
            return this.each(function() {
                var popup = $(this);
                var opts = popup.data("opts");
                if(value != null)
                {
                    opts[option] = value;
                    popup.data("opts",opts);
                }
                else
                    return opts[option];
            });
        }
    };
    
    function showPopup(popup,opts,skipEffect)
    {
        if(opts.beforeopen != null)
        {
            if(!opts.beforeopen())
                return;
        }
        
        popup.css("display","block");
        popup.css("position","absolute");
        
        popup.position(opts.position);
        
        if(opts.modal)
            setBackground();
        
        if((skipEffect == null || !skipEffect) && opts.showeffect != null)
        {
            popup.css("display","none");
            popup.show(opts.showeffect.effect, opts.showeffect.options, opts.showeffect.speed, postShow );						
        }
        else
            postShow();
    
        function postShow()
        {
            var shownHid = $("#" + popup.attr("id") + "_shown");
            shownHid.val("true");

            $(window).bind("resize.ls_popup_" + popup.attr("id"), function(event) { popup.position(opts.position); });
            popup.bind("resize", function(event) { popup.position(opts.position); });
            
            if(opts.scrolling)
                $(window).bind("scroll.ls_popup_" + popup.attr("id"), function(event) { popup.position(opts.position); });
            
            if(opts.showeffect != null && opts.showeffect.callback != null)
                opts.showeffect.callback();
            
            if(opts.afteropen != null)
                opts.afteropen();
        };

        function setBackground()
        {
            if(opts.usejqueryoverlay)
            {
                var bgd = $(".ui-widget-overlay");
                if(bgd.length > 0)
                {
                    opts.bgd_zindex = bgd.css("z-index");
                    bgd.css("z-index", popup.css("z-index") - 1);
                }
                else
                {
                    $("<div class='ui-widget-overlay'></div>").insertBefore(popup);						
                    var bgd = $(".ui-widget-overlay");
                    opts.bgd_zindex = 0;
                    bgd.css({
                        "z-index":popup.css("z-index") - 1,
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
                    if($.inArray(popup[0],using) < 0)
                        using.push(popup[0]);
                        
                    bgd.css("z-index",0);
                    for(var i = 0; i < using.length; ++i)
                    {
                        if(($(using[i]).css("z-index") - 1) > bgd.css("z-index"))
                            bgd.css("z-index",$(using[i]).css("z-index") - 1);
                    }
                    
                    bgd.data("using",using);
                }
                else
                {						
                    $("<div id='ls_popup_bgd'></div>").insertBefore(popup);
                    var bgd = $("#ls_popup_bgd").first();
                    bgd.css({
                        zIndex:popup.css("z-index") - 1,
                        position:"fixed", left:"0px", top:"0px", backgroundColor:"#000000",
                        width:"100%",height:"100%",opacity:"0.5"
                    });
                    var using = [];
                    using.push(popup[0]);
                    bgd.data("using", using );
                }
            }
        };
    };
        
    function hidePopup(popup,opts)
    {
        if(opts.beforeclose != null)
        {
            if(!opts.beforeclose())
                return;
        }
        
        if(opts.hideeffect != null)
            popup.hide(opts.hideeffect.effect, opts.hideeffect.options, opts.hideeffect.speed, postHide);
        else
        {
            popup.css("display","none");
            postHide();
        }	

        function postHide()
        {
            $(window).unbind(".ls_popup_" + popup.attr("id"));
            popup.unbind("resize");
            
            var _shownHid = $("#" + popup.attr("id") + "_shown");
            _shownHid.val("false");
            
            if(opts.modal)
                hideBackground();
        
            if(opts.hideeffect != null && opts.hideeffect.callback != null)
                opts.hideeffect.callback();
        
            if(opts.afterclose != null)
                eval(opts.afterclose());
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
                    var i = $.inArray(popup[0], using);
                    if( i >= 0)
                        using.splice(i,1);
                    if(using.length > 0)
                    {
                        bgd.css("z-index",0);
                        for(var i = 0; i < using.length; ++i)
                        {
                            if(($(using[i]).css("z-index") - 1) > bgd.css("z-index"))
                                bgd.css("z-index",$(using[i]).css("z-index") - 1);
                        }
                    }
                    else
                        bgd.css("display","none");
                    bgd.data("using",using);
                }
            }
        };
    };
    
    $.fn.ls_popup = function(method) {
        if(ls_popup_methods[method])
            return ls_popup_methods[method].apply(this,Array.prototype.slice.call(arguments, 1));
        else if(typeof method == 'object' || !method)
            return ls_popup_methods.init.apply(this, arguments);
        else
            $.error("Method " + method + " does not exists in ls_popup");
    };
})(jQuery);