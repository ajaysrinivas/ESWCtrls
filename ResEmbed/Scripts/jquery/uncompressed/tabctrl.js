/*
* jQuery.ls_tabctrl.js - tab ctrl plugin, specifically to work with asp.net control
*
* version 1.0.0 (2011/08/28) (C)opyright Leon Pennington 2011
* 
* Dual licensed under the MIT and GPL licenses: 
*   http://www.opensource.org/licenses/mit-license.php 
*   http://www.gnu.org/licenses/gpl.html 
*/
; (function ($)
{
    $.fn.ls_tabctrl = function (args)
    {
        var opts = $.extend(true, {
            useImages: false,
            activeClass: null,
            inactiveClass: null,
            left: { active: null, inactive: null },
            middle: { active: null, inactive: null },
            right: { active: null, inactive: null }
        }, args);

        return this.each(function ()
        {
            var _tabctrl = $(this);
            var _tabid = _tabctrl.attr("id");
            var _actTab = $("#" + _tabid + "_actIdx");

            var links = $("#" + _tabid + "_tabbar a");
            links.each(function ()
            {
                var link = $(this);
				this.idx = link.attr("oa");
                link.attr("href", null);
                link.css("cursor", "pointer");
                link.bind("click", tabChange);
            });

            function tabChange()
            {
                var actIdx = _actTab.val();
                if (this.idx == actIdx)
                    return false;

                if (opts.useImages)
                {
                    $("#" + _tabid + "_ld_" + actIdx).addClass(opts.inactiveClass).removeClass(opts.activeClass).css("background-image", opts.left.inactive);
                    $("#" + _tabid + "_rd_" + actIdx).addClass(opts.inactiveClass).removeClass(opts.activeClass).css("background-image", opts.right.inactive);
                    $("#" + _tabid + "_btn_" + actIdx).addClass(opts.inactiveClass).removeClass(opts.activeClass).css("background-image", opts.middle.inactive);

                    $("#" + _tabid + "_ld_" + this.idx).addClass(opts.activeClass).removeClass(opts.inactiveClass).css("background-image", opts.left.active);
                    $("#" + _tabid + "_rd_" + this.idx).addClass(opts.activeClass).removeClass(opts.inactiveClass).css("background-image", opts.right.active);
                    $(this).addClass(opts.activeClass).removeClass(opts.inactiveClass).css("background-image", opts.middle.active);
                }
                else
                {
                    $("#" + _tabid + "_btn_" + actIdx).parent().removeClass(opts.activeClass).addClass(opts.inactiveClass);
                    $(this).parent().removeClass(opts.inactiveClass).addClass(opts.activeClass);
                }

                $("#" + _tabid + "_page_" + actIdx).css("display", "none");
                $("#" + _tabid + "_page_" + this.idx).css("display", "block");

                _actTab.val($(this).attr("oa"));
            };

        });
		
    };
})(jQuery);