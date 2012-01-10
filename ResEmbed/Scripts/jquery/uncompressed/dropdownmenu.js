/*
* jQuery.ls_dropmenu.js - dropdown menu plugin, specifically to work with asp.net control
*
* version 1.0.1 (2012/01/10) (C)opyright Leon Pennington 2012
* 
* Dual licensed under the MIT and GPL licenses: 
*   http://www.opensource.org/licenses/mit-license.php 
*   http://www.gnu.org/licenses/gpl.html 
*/
; (function ($)
{
    $.fn.ls_dropmenu = function (args)
    {
        var opts = $.extend(true,
        {
            itemClass: null,
            openClass: null,
            hoverClass: null,
            topItemClass: null,
            topHoverClass: null,
            disabledClass: null,
            arrowDisplayType: "none",
            arrowDown: { image: null, cssClass: null },
            arrowRight: { image: null, cssClass: null },
            showEffect: null, hideEffect: null
        }, args);

        this.each(function ()
        {

            var _menu = $(this);
            var _open = false;
            var _timer = null;

            _menu.click(function (evt) { evt.stopPropagation(); });
            _menu.find("ul").css({ visibility: "hidden", display: "block" })
                .css("width", function () { return $(this).width() + "px"; })
                .css({ visibility: "", display: "none" });

            _menu.children("li").click(
                function (evt)
                {
                    if (!_open)
                    {
                        _open = true;
                        openMenu(this);
                    }
                }
            ).each(function (idx, elem) { elem.topLevel = true; });

            _menu.find("li").each(
                function (idx, elem)
                {
                    var hCls = "";
                    var oCls = "";
                    if (opts.hoverClass != null)
                        hCls = opts.hoverClass;

                    if (opts.openClass != null)
                        oCls = opts.openClass;

                    if (elem.topLevel)
                    {
                        if (opts.topHoverClass != null)
                            hCls += " " + opts.topHoverClass;

                        if (opts.topOpenClass != null)
                            oCls += " " + opts.topOpenClass;
                    }

                    var item = $(elem);

                    if (item.attr("hoverclass") != null)
                        hCls += " " + i.attr("hoverclass");
                    if (item.attr("openclass") != null)
                        oCls += " " + i.attr("openclass");
                    elem.hclass = $.trim(hCls);
                    elem.oclass = $.trim(oCls);
                }
            ).hover(
                function (evt)
                {
                    $(this).addClass(this.hclass);
                    var elem = this;
                    if (_open)
                    {
                        clearTimeout(_timer);
                        _timer = setTimeout(function () { openMenu(elem) }, 200);
                    }
                    evt.stopPropagation();
                }
                , function (evt)
                {
                    $(this).removeClass(this.hclass);
                }
            );

            if (opts.arrowDisplayType != "none")
            {
                _menu.find("li > ul").parent().each(
                    function ()
                    {
                        if (this.topLevel)
                        {
                            if (opts.arrowDisplayType == "subonly")
                                return;
                        }
                        else if (opts.arrowDisplayType == "toponly")
                            return;

                        var item = $(this);

                        if (item.css("display") == "inline" || item.css("float") != "none")
                        {
                            if (opts.arrowDown.image != null)
                                addArrow(item, opts.arrowDown);
                        }
                        else if (opts.arrowRight.image != null)
                            addArrow(item, opts.arrowRight);
                    }
                )
            }

            function addArrow(item, img)
            {
                if (item.children("a").length > 0)
                    item.children("a").append('<img src="' + img.image + '" alt="Open Arrow" class="' + img.cssClass + '" />');
                else
                    item.append('<img src="' + img.image + '" alt="Open Arrow" class="' + img.cssClass + '" />')
            };

            function closeMenu(item)
            {
                item.children("ul").stop(true, true);
                if (opts.hideEffect != null)
                    item.children("ul").hide(opts.hideEffect.effect, opts.hideEffect.options, opts.hideEffect.speed, opts.hideEffect.callback);
                else
                    item.children("ul").css("display", "none");
                item.attr("open", false).removeClass(function () { return this.oclass; });
            };

            function openMenu(elem)
            {
                var item = $(elem);
                var panel = item.children("ul");
                var isOpen = item.attr("open");
                if (panel.length > 0 && item.attr("open") == undefined)
                {
                    item.attr("open", true);
                    item.addClass(elem.oclass);
                    var i = true;
                    var offSet = item.offset();
                    if (item.css("display") == "inline")
                    {
                        if (item.css("position") == "static")
                            offSet.top += item.height();
                        else
                        {
                            offSet.left = 0;
                            offSet.top = item.height();
                        }
                    }
                    else if (item.css("float") != "none")
                        offSet.top += item.height();
                    else
                    {
                        offSet.left = item.width();
                        if (item.css("position") == "static")
                            offset.top = item.position().top;
                        else offSet.top = 0;
                        i = false;
                    }

                    panel.css({ left: offSet.left, top: offSet.top });
                    if (opts.showEffect != null)
                        panel.show(opts.showEffect.effect, opts.showEffect.options, opts.showEffect.speed, opts.showEffect.callback);
                    else
                        panel.css("display", "block");
                }

                closeMenu(_menu.find("li[open]").not(item.parents("ul").parent().add(elem)));
            };

            $(document).click(
                function ()
                {
                    if (_open)
                        _timer = setTimeout(function () { closeMenu(_menu.find("li[open]")); _open = false; }, 100);
                }
            );
        });
    };
})(jQuery);