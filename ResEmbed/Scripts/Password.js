/* Valid password control */
function ESW_Password_Valid( ctrlValues )
{
    var pwdCtrl = document.getElementById( ctrlValues.id + "_pwd" );
    var cnfCtrl = document.getElementById( ctrlValues.id + "_conf" );
    var errMsg = document.getElementById( ctrlValues.id + "_err" );
    
    if( ctrlValues.required == "True" &&  pwdCtrl.value.length == 0 )
        errMsg.innerHTML = ctrlValues.reqMsg;
    else if( pwdCtrl.value != cnfCtrl.value )
        errMsg.innerHTML = ctrlValues.nmMsg;
    else if( pwdCtrl.value.length < ctrlValues.minLen )
        errMsg.innerHTML = ctrlValues.mlMsg;
    else
    {
        if( ctrlValues.errDisplay == "Dynamic" )
            errMsg.style.display = "none";
        else
            errMsg.style.visibility = "hidden";
        return true;
    }
    
    if( ctrlValues.errDisplay == "Dynamic" )
        errMsg.style.display = "block";
    else
        errMsg.style.visibility = "visible";
    return false;
}
