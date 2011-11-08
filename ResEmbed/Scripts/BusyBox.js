/*** Code for showing the busy box ***/
var ESW_BB_PB;
var ESW_BB_DCP = false;
var ESW_BB_AX;

function ESW_SetupBusyBox( busyBox )
{
    if( busyBox.PB_Enabled == "true" )
    {
        var bb = document.getElementById(busyBox.id + "_pb");
        if( bb.SubmitButton != "" )
        {
            var ctrl = document.getElementById( bb.SubmitButton );
            if( ctrl != null )
            {
                ctrl.busyBox = bb;
                ctrl.onclick = function() { ESW_ShowBusyBox(this); }
            }
        }
        else
        {
            $(window).bind('beforeunload', function () { ESW_ShowBusyBox(); });
            ESW_BB_PB = bb;
        }
        
        //Double click prevention
        if( bb.modal == "false" )
        {
            for( var i = 0; i < document.forms.length; ++i )
            {
                document.forms[i].onsubmit = function()
                {
                    if( !ESW_BB_DCP )
                    {
                        ESW_BB_DCP = true;
                        return true;
                    }
                    else
                    return false;
                }
            }
        }
    }
    
    if( busyBox.AX_Enabled == "true" )
    {
        ESW_BB_AX = document.getElementById(busyBox.id + "_ax");
        if (busyBox.AX_Cover == "true")
        {
            ESW_BB_AX.cover = true;
            ESW_BB_AX.coverElem = document.getElementById(busyBox.id + "_ax_cover");
        }
        else
            ESW_BB_AX.cover = false;
        
        var req = Sys.WebForms.PageRequestManager.getInstance();
        req.add_initializeRequest(ESW_BB_onInitRequest);
        req.add_beginRequest(ESW_BB_onStartedRequest);
        req.add_endRequest(ESW_BB_onFinishedRequest); 
    }
}

// Postback busy Box

function ESW_ShowBusyBox(ctrl)
{
    if( ESW_BB_PB == null && ctrl != null )
        ESW_BB_PB = ctrl.busyBox;

    $(ESW_BB_PB).ls_popup({modal:ESW_BB_PB.modal,position:eval(ESW_BB_PB.position),open:true});
    ESW_BB_GfxShow(0);
}

function ESW_BB_GfxShow( frame )
{
    ++frame;
    if( frame >= parseInt( ESW_BB_PB.GFXFrameCount ) )
        frame = 0;        
        
    var gfx = document.getElementById( ESW_BB_PB.id + "_gfx" );
    
    if( ESW_BB_PB.GFXBase != "")
    {
        if( frame < 10 )
            gfx.src = ESW_BB_PB.GFXBase + "0" + frame + ESW_BB_PB.GFXPostFix;
        else
            gfx.src = ESW_BB_PB.GFXBase + frame + ESW_BB_PB.GFXPostFix;
    }
    else
    {
        gfx.src = eval( "ESW_BB_PB.ResImg" + frame );
    }
        
    ESW_BB_PB.timeout = setTimeout( 'ESW_BB_GfxShow(' + frame + ')', 100 );
}

// AJAX Busy Box

function ESW_BB_onInitRequest(sender, args)
{
    if( sender.get_isInAsyncPostBack())
        args.set_cancel(true);
}

function ESW_BB_onFinishedRequest(sender, args)
{   
    if( ESW_BB_AX.parentNode != null )
    {
        ESW_BB_AX.style.display = "none";
        if (ESW_BB_AX.cover)
            ESW_BB_AX.coverElem.style.display = "none";
        else
        {
            if (ESW_BB_AX.busyElement)
                ESW_BB_AX.busyElement.style.visibility = "visible";
        }
    }
}

function ESW_BB_onStartedRequest(sender, args)
{
    var pbe = args.get_postBackElement();
    for( var i = 0; i < sender._updatePanelClientIDs.length; ++i )
    {
        var panel = document.getElementById(sender._updatePanelClientIDs[i]);
        var parent = pbe.parentNode;
        while( parent )
        {
            if( parent.id == panel.id )
            {
                ESW_BB_AddImage( panel );
                return;
            }
            else
                parent = parent.parentNode;
        }
    }
}

function ESW_BB_AddImage( elem )
{
    var e = $(elem);
    var pos = e.offset();
    var zidx = isNaN(parseInt(e.css("z-index"))) ? 0 : parseInt(e.css("z-index"));
    e.parents().each(function () { var ti = parseInt($(this).css("z-index")); zidx = isNaN(ti) ? zidx : (ti > zidx ? ti : zidx); });

    ESW_BB_AX.style.display = "block";
    ESW_BB_AX.style.left = (pos.left + ((e.outerWidth() - ESW_BB_AX.offsetWidth) / 2)) + "px";
    ESW_BB_AX.style.top = (pos.top + ((e.outerHeight() - ESW_BB_AX.offsetHeight) / 2)) + "px";
    ESW_BB_AX.style.zIndex = zidx + 2;

    if (ESW_BB_AX.cover)
    {
        ESW_BB_AX.coverElem.style.left = pos.left + "px";
        ESW_BB_AX.coverElem.style.top = pos.top + "px";
        ESW_BB_AX.coverElem.style.width = e.outerWidth() + "px";
        ESW_BB_AX.coverElem.style.height = e.outerHeight() + "px";
        ESW_BB_AX.coverElem.style.zIndex = zidx + 1;
        ESW_BB_AX.coverElem.style.display = "block";
    }
    else
    {
        e.css("visibility", "hidden");
        ESW_BB_AX.busyElement = elem;
    }
}