/*Valid Number Functions*/
function ESW_NumberValidator( validator )
{
    var ctrl = document.getElementById( validator.controltovalidate );
    var text = ctrl.value.replace( ",", "" ).replace( " ", "" );
    
    var neg = false;
    if( text.startsWith( "-" ) )
    {
        neg = true;
        text = text.substring( 1 );   
    }
    
    if( validator.prefix != null && validator.prefix.length > 0 && text.startsWith( validator.prefix ) )
        text = text.substring( 1 );
        
    if( text.length == 0 || text == "-" )
    {
        ctrl.value = "";
        if( validator.required == "true" )
            return false;
        else
            return true;
    }
        
    if( isNaN( text ) )
        return false;
        
    var val = neg ? 0 - parseFloat( text ) : parseFloat( text );
    if( validator.minimum != null && val < validator.minimum )
        return false;
    
    if( validator.maximum != null && val > validator.maximum )
        return false;
    
    if( val < 0 )
        neg = true;
        
    var bd = Math.abs( val ).toString();
    var ad = "";
    if( bd.indexOf( "." ) > -1 )
    {
        ad = bd.substring( bd.indexOf( "." ) + 1 );
        bd = bd.substring( 0, bd.indexOf( "." ) );
    }
        
    if( validator.thouSeps == "true" )
    {
        var rgx = /(\d+)(\d{3})/;
	    while (rgx.test(bd)) {
		    bd = bd.replace(rgx, '$1' + ',' + '$2');
	    }
    }
    
    if( neg )
        text = "-";
    else
        text = "";
    
    if( validator.prefix != null )
        text += validator.prefix;
    
    if( bd.length == 0 )
        text += "0";
    else
        text += bd;
    
    if( validator.decPlaces > 0 )
    {
        if( ad.length < validator.decPlaces )
        {
            text += ".";
            text += ad;
            if( validator.padDecPlaces == "true" )
            {
                for( var i = ad.length; i < validator.decPlaces; ++i )
                    text += "0";
            }
        }
        else            
            text += "." + ad.substring( 0, validator.decPlaces );
    }
    
    ctrl.value = text;
    
    return true;    
}

function ESW_AddCommas( str )
{
    str += '';
	x = str.split('.');
	x1 = x[0];
	x2 = x.length > 1 ? '.' + x[1] : '';
	var rgx = /(\d+)(\d{3})/;
	while (rgx.test(x1)) {
		x1 = x1.replace(rgx, '$1' + ',' + '$2');
	}
	return x1 + x2;
}

function ESW_NumberValidatorSetup( validator )
{
    if (validator != null)
    {
        var ctrl = document.getElementById(validator.controltovalidate);
        if (ctrl != null)
        {
            ctrl.validator = validator;
            $(ctrl).keypress(ESW_NumberValidatorKeyPress);
            $(ctrl).blur(ESW_NumberValidatorBlur);
        }
    }
}

function ESW_NumberValidatorBlur()
{
    if( this.validator.required == "true" )
    {
        if( this.value == "" )
        {
            this.focus();
            if( this.validator.style.display == "none" )
                this.validator.style.display = "inline";
            else
                this.validator.style.visibility = "visible";
            return;
        }
    }
}

function ESW_NumberValidatorKeyPress( evt )
{    
    var validator = this.validator;
    var text = this.value;
    
    var keyNum;
    var isCtrl = false;
    if( window.event )
    {
        keyNum = window.event.keyCode;
        isCtrl = window.event.ctrlKey;
    }
    else if( evt.which )
    {
        keyNum = evt.which;
        isCtrl = evt.ctrlKey;
    }
    else
        return true;
    if( isNaN( keyNum ) || keyNum == 8 || isCtrl ) 
        return true;
    
    var keyed = String.fromCharCode( keyNum );        
    var selStart = ESW_getSelectionStart( this );

    if( isNaN( keyed ) )
    {
        if( keyed == "." )
        {
            if( validator.decPlaces < 1 || text.indexOf( "." ) > -1 )
                return false;
            else
                return true;
        }
        else if( keyed == "," )
        {
            if( validator.thouSeps == "true" )
                return true;
        }
        else
        {
            if( validator.prefix != null )
            {
                if( selStart == 0 && ( keyed == "-" || keyed == validator.prefix ) )
                    return true;
                else if( selStart == 1 && ( ( keyed == "-" && text.charAt(0) == validator.prefix ) || ( keyed == validator.prefix && text.charAt(0) == "-" ) ) )
                    return true;                    
            }
            else
            {
                if( selStart == 0 && keyed == "-" )
                    return true;
            }
        }
    }
    else if( keyed != " " )
    {
        var decPlace = text.indexOf( "." );
        if( validator.decPlaces > 0 && decPlace > -1 && selStart > decPlace )
        {
            var dec = text.substring( text.indexOf( "." ) + 1 );
            if( dec.length >= validator.decPlaces )
                return false;
        }
        
        return true;
    }
    
    return false;
}

/*** Seelction Handling ***/
function ESW_setSelectionRange(input, start, end)
{
    if (typeof (input.setSelectionRange) != "undefined")
        input.setSelectionRange(start, end);
    else
    {   // IE
        var range = input.createTextRange();
        range.collapse(true);
        range.moveStart("character", start);
        range.moveEnd("character", end - start);
        range.select();
    }
}

function ESW_getSelectionStart(input)
{
    if (typeof (input.selectionStart) != "undefined")
        return input.selectionStart;

    // IE 5.5,6,7
    var range = document.selection.createRange();
    var r2 = range.duplicate();
    return (0 - r2.moveStart("character", -100000));
}

function ESW_getSelectionEnd(input)
{
    if (typeof (input.selectionStart) != "undefined")
        return input.selectionEnd;

    // IE 5.5,6,7
    var range = document.selection.createRange();
    var r2 = range.duplicate();
    var start = 0 - r2.moveStart("character", -100000);
    return (start + range.text.length);
}