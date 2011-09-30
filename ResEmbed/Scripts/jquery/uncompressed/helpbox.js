/*
* jQuery.ls_helpbox.js - helpbox plugin, specifically to work with asp.net control
*
* version 1.0.0 (2011/09/30) (C)opyright Leon Pennington 2011
* 
* Dual licensed under the MIT and GPL licenses: 
*   http://www.opensource.org/licenses/mit-license.php 
*   http://www.gnu.org/licenses/gpl.html 
*/
; (function ($)
{
    $.fn.ls_helpbox = function (args)
    {
        var opts = $.extend(true, {
            controls: null, // array of { id:"ctrlid", helpId:"helpid", hasHoverHelp:true, hoverHelp:"help for hover", hoverPosition:{PositionInfo}, hasClickHelp:false, clickHelp:"help for click", clickPosition:{PositionInfo} }
            clickModal: false,
            ajaxCallback: null // { url:"pagemethodUrl", paramHelpId: "parameterName", paramClick: "parameterName" }
        }, args);

        return this.each(function ()
        {
            var helpbox = $(this);
            var hoverbox = helpbox.children("[id$='_hover']").ls_popup();
            var clickbox = helpbox.children("[id$='_click']").ls_popup({ modal: opts.clickModal }).click(clickClose);
            var clickLock = false;

            for (var i = 0; i < opts.controls.length; ++i)
            {
                var ctrlOpts = opts.controls[i];
                var ctrl = $("#" + ctrlOpts.id);
                ctrl.data("opts", ctrlOpts);

                if (ctrlOpts.hasHoverHelp)
                    ctrl.hover(hoverOpen, hoverClose);

                if (ctrlOpts.hasClickHelp)
                    ctrl.click(clickOpen);
            }

            function hoverOpen()
            {
                if (clickLock)
                    return;

                var hp = $(this);
                var hpOpts = hp.data("opts");
                if (hpOpts.hoverPosition != null)
                    hoverbox.ls_popup("option", "position", hpOpts.hoverPosition);
                else if (hpOpts.forCtrl != null)
                    hoverbox.ls_popup("option", "position", { my: "left bottom", at: "right top", of: "#" + hpOpts.forCtrl });
                else
                    hoverbox.ls_popup("option", "position", { my: "left bottom", at: "right top", of: this });

                if (hpOpts.hoverHelp != null)
                    hoverOpenText(hpOpts.hoverHelp);
                else
                    ajaxHelp(hpOpts.helpId, false, hoverOpenText);
            };

            function hoverOpenText(text)
            {
                hoverbox.html(text);
                hoverbox.ls_popup("show");
            };

            function hoverClose()
            {
                if (clickLock)
                    return;

                hoverbox.ls_popup("hide");
            };

            function clickOpen()
            {
                if (clickLock)
                    return;
                hoverClose();
                clickLock = true;

                var hp = $(this);
                var hpOpts = hp.data("opts");
                if (hpOpts.clickPosition != null)
                    clickbox.ls_popup("option", "position", hpOpts.clickPosition);
                else if (hpOpts.forCtrl != null)
                    clickbox.ls_popup("option", "position", { my: "left top", at: "left bottom", of: "#" + hpOpts.forCtrl });
                else
                    clickbox.ls_popup("option", "position", { my: "left top", at: "left bottom", of: this });

                if (hpOpts.clickHelp != null)
                    clickOpenText(hpOpts.clickHelp);
                else
                    ajaxHelp(hpOpts.helpId, true, clickOpenText);
            };

            function clickOpenText(text)
            {
                clickbox.html(text);
                clickbox.ls_popup("show");
            };

            function clickClose()
            {
                if (!clickLock)
                    return;

                clickbox.ls_popup("hide");
                clickLock = false;
            };

            function ajaxHelp(helpId, click, callback)
            {
                if (opts.ajaxCallback == null)
                {
                    callback("Help is not available. Missing ajax callback.");
                    return;
                }

                $.ajax({
                    url: opts.ajaxCallback.url,
                    data: "{" + opts.ajaxCallback.paramHelpId + ":\"" + helpId + "\"," + opts.ajaxCallback.paramClick + ":" + click + "}",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    success: function (msg) { callback(msg.d); },
                    errror: function (msg, text) { callback(msg + "<br />" + text); }
                });
            };
        });
    };
})(jQuery);