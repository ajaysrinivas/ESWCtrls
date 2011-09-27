/* Regular expresion validator */
function ESW_RegExValidator( validator )
{
    var value = ValidatorTrim(ESW_ControlValue(validator.controltovalidate));
    if(value.length == 0)
    {
        if(validator.required == "true")
            return false;
        else
            return true;
    }
    var rx = new RegExp(validator.validationexpression, validator.options);
    var matches = rx.exec(value);
    return (matches != null && value == matches[0]);
}

function ESW_MultiValidatorSetup( validator )
{
    var ctrls = validator.controlstovalidate.split(";");
    for( var i = 0; i < ctrls.length; ++i )
    {
        var ctrl = document.getElementById( ctrls[i] );
        if( ctrl != null )
        {
            switch( ctrl.type )
            {
                case "select-one":
                case "text":
                    ctrl.validator = validator;
                    ctrl.onchange = function() { ESW_MultiValidatorChange( this.validator ); }
                    break;
                case "checkbox":
                case "radiobox":
                    ctrl.validator = validator;
                    ctrl.onclick = function() { ESW_MultiValidatorChange( this.validator ); }
                    break;
            }
        }
    }
}

function ESW_MultiValidatorChange( validator )
{
    if( ESW_MultiValidator( validator ) )
    {
        if( validator.display == "Dynamic" )
            validator.style.display = "none";
        else
            validator.style.visibility = "hidden";
    }
    else
    {
        if( validator.display == "Dynamic" )
            validator.style.display = "inline";
        else
            validator.style.visibility = "visible";
    }
}

function ESW_MultiValidator( validator )
{
    var ctrls = validator.controlstovalidate.split(";");
    switch( validator.condition )
    {
        case "OR":
            for(var i = 0; i < ctrls.length; ++i )
            {
                var value = ValidatorTrim(ESW_ControlValue(ctrls[i]));
                if( value != '' && value != "ESW_Value_False" ) 
                    return true;
            }
            return false;
            break;
        case "XOR":
            var found = false;
            for( var i = 0; i < ctrls.length; ++i )
            {
                var value = ValidatorTrim(ESW_ControlValue(ctrls[i]));
                if( value != '' && value != "ESW_Value_False" )
                {
                    if(found)
                        return false;
                    else
                        found = true;
                }
            }
            return found;
            break;
        case "AND":
            for( var i = 0; i < ctrls.length; ++i )
            {
                var value = ValidatorTrim(ESW_ControlValue(ctrls[i]));
                if( value == '' || value == "ESW_Value_False" )
                    return false;
            }
            return true;
            break;
    }
    return false;
}

function ESW_ControlValue( ctrlID )
{
    var ctrl = document.getElementById( ctrlID );
    if( ctrl != null )
    {
        switch( ctrl.type )
        {
            case "text": case "file":
                return ctrl.value;
                break;
            case "checkbox":
            case "radiobox":
                if( ctrl.checked )
                    return "ESW_Value_True";
                else
                    return "ESW_Value_False";
                break;
            case "select-one":
                return ctrl.options[ctrl.selectedIndex].value;
                break;
        }
    }
    return null;
}