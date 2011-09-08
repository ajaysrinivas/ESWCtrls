/*** Drop Dow Menu Code ***/
// This code relies on jquery

function ESW_DD_Setup(menu)
{
    menu.active = false;

    $(menu).click(function (e) { e.stopPropagation(); });

    $(menu).find("ul")
		.css({ visibility: "hidden", display: "block" })
		.css("width", function () { return $(this).width() + "px"; })
		.css({ visibility: "", display: "none" });

    $(menu).children("li")
		.click
		(
			function (e)
			{
			    if (!menu.active)
			    {
			        menu.active = true;
			        ESW_DD_Open(this, menu);
			    }
			}
		)
		.each(function (idx, elem) { elem.topLevel = true; });

    $(menu).find("li")
		.each
		(
			function (idx, elem)
			{
			    var intID = elem.id.replace(menu.id, "");
			    var hclass = "";
			    var oclass = "";

			    if (menu.HoverStyle != null)
			        hclass = menu.HoverStyle;

			    if (menu.OpenStyle != null)
			        oclass = menu.OpenStyle;

			    if (elem.topLevel)
			    {
			        if (menu.TopHoverStyle != null)
			            hclass += " " + menu.TopHoverStyle;

			        if (menu.TopOpenStyle != null)
			            oclass += " " + menu.TopOpenStyle;
			    }

			    var style = eval("menu.HoverStyle" + this.intID);
			    if (style != null)
			        hclass += " " + style;

			    style = eval("menu.OpenStyle" + this.intID);
			    if (style != null)
			        oclass += " " + style;

			    elem.hclass = jQuery.trim(hclass);
			    elem.oclass = jQuery.trim(oclass);
			}
		)
		.hover
		(
		 	function (e)
		 	{
		 	    $(this).addClass(this.hclass);
		 	    var item = this;
		 	    if (menu.active)
		 	    {
		 	        clearTimeout(menu.timeout);
		 	        menu.timeout = setTimeout(function () { ESW_DD_Open(item, menu); }, 200);
		 	    }
		 	    e.stopPropagation();
		 	},
			function (e)
			{
			    $(this).removeClass(this.hclass);
			}
		);

    if (menu.ArrowDisplay != "none")
    {
        var dwn = (menu.ArrowDown && menu.ArrowDown.length > 0);
        var rgt = (menu.ArrowRight && menu.ArrowRight.length > 0);
        var dwnStyle = "";
        if (menu.ArrowDownStyle)
            dwnStyle = menu.ArrowDownStyle;
        var rgtStyle = "";
        if (menu.ArrowRightStyle)
            rgtStyle = menu.ArrowRightStyle;

        $(menu).find("li > ul").parent().each(
			function ()
			{
			    if (this.topLevel)
			    {
			        if (menu.ArrowDisplay == "subonly")
			            return;
			    }
			    else if (menu.ArrowDisplay == "toponly")
			        return;

			    var i = $(this);
			    if (i.css("display") == "inline" || i.css("float") != "none")
			    {
			        if (dwn)
			            ESW_DD_AddArrow(i, menu.ArrowDown, dwnStyle);
			    }
			    else if (rgt)
			        ESW_DD_AddArrow(i, menu.ArrowRight, rgtStyle);
			}
		);
    }

    $(document).click(
		function ()
		{
		    if (menu.active)
		    {
		        menu.timeout = setTimeout(function ()
		        {
		            var close = $(menu).find("li[open=true]");
		            ESW_DD_Close(menu, close);
		            menu.active = false;
		        }, 100
		        );
		    }
		}
	);
}

function ESW_DD_Open(item, menu)
{
    var ji = $(item);
    var panel = ji.children("ul");
    if (panel.length > 0 && !item.open)
    {
        item.open = true;
        ji.addClass(item.oclass);
        var down = true;

        var offset = ji.offset();
        if (ji.css("display") == "inline")
        {
            if (ji.css("position") == "static")
                offset.top += ji.height();
            else
            {
                offset.left = 0;
                offset.top = ji.height();
            }
        }
        else if (ji.css("float") != "none")
        {
            offset.top += ji.height();
        }
        else
        {
            offset.left = ji.width();
            if (ji.css("position") == "static")
                offset.top = ji.position().top;
            else
                offset.top = 0;
            down = false;
        }
        panel.css({ left: offset.left, top: offset.top });
        switch (menu.ShowType)
        {
            case "fade": panel.fadeIn("slow"); break;
            case "slide":
                if (down)
                    panel.slideDown("slow");
                else
                {
                    var cw = panel.attr("origWidth") == undefined ? panel.width() : panel.attr("origWidth");
                    panel.css("width", "0px");
                    panel.animate({ width: cw }, "slow", "swing");
                }
                break;
            default: panel.show("fast"); break;
        }
    }

    var close = $(menu).find("li[open=true]").not(ji.parents("ul").parent().add(item));
    ESW_DD_Close(menu, close);
}

function ESW_DD_AddArrow(ji, arrow, style)
{
    if (ji.children("a").length > 0)
        ji.children("a").append("<img src=\"" + arrow + "\" alt=\"Open Arrow\" class=\"" + style + "\" />");
    else
        ji.append("<img src=\"" + arrow + "\" alt=\"Open Arrow\" class=\"" + style + "\" />");
}

function ESW_DD_Close(menu, items)
{
    items.children("ul").stop(true, true);
    switch (menu.HideType)
    {
        case "fade": items.children("ul").fadeOut("fast");
        case "slide":
            items.each(function ()
            {
                var item = $(this);
                var panel = item.children("ul");
                if (item.css("display") == "inline" || item.css("float") != "none")
                    panel.slideUp("fast");
                else
                {
                    panel.attr("origWidth", panel.width());
                    panel.animate({ width: "0px" }, "false", "swing", function () { $(this).css("width", this.origWidth + "px"); });
                }
            });
        default: items.children("ul").hide("fast");
    }

    items.attr("open", false).removeClass(function () { return this.oclass; });
}