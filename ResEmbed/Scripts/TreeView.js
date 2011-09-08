/* Tree View Functions */
function ESW_Tree_expcolBranch( tree, nodeID )
{
    if( tree.ClientBeforeExpandCollapse != null )
    {
        var rst = eval( tree.ClientBeforeExpandCollapse );
        if( rst != null )
            return rst;
    }

    var childDiv = document.getElementById( tree.id + "_" + nodeID + "_children" );
    var ecImg = document.getElementById( tree.id + "_" + nodeID + "_ecImg" );
    var expanded = document.getElementById( tree.id + "_" + nodeID + "_expanded" );
    
    if( childDiv != null )
    {
        if( expanded.value == "True" )
        {
            expanded.value = "False";
            childDiv.style.display = "none";
            ecImg.src = tree.CBoxImg;
            ecImg.alt = "Expand";
        }
        else
        {
            expanded.value = "True";
            childDiv.style.display = "block";
            ecImg.src = tree.EBoxImg;
            ecImg.alt = "Collapse";
        }
    }    
    
    if( tree.ClientAfterExpandCollapse != null )
    {
        var rst = eval( tree.ClientAfterExpandCollapse );
        if( rst != null )
            return rst;
    }
    
    return false;
}

function ESW_Tree_clickNode( tree, nodeID )
{
    var result = true;

    if( tree.ClientBeforeClick != null )
    {
        var rst = eval( tree.ClientBeforeClick );
        if( rst != null )
            return rst;
    }

    if( tree.SelectMode == "Single" )
    {
        var selected = document.getElementById( tree.id + "_selected" );
        if( selected.value == nodeID )
        {
            document.getElementById( tree.id + "_" + nodeID ).className = ESW_Tree_HoverStyle( tree, nodeID );
            selected.value = "";
        }
        else
        {
            if( selected.value.length > 0 )
                document.getElementById( tree.id + "_" + selected.value ).className = ESW_Tree_NodeStyle( tree, selected.value );
                
            document.getElementById( tree.id + "_" + nodeID ).className = ESW_Tree_SelectHoverStyle( tree, nodeID );
            selected.value = nodeID;
        }
        result = false;
    }
    else if( tree.SelectMode == "Multiple" )
    {
        var nodeSelect = document.getElementById( tree.id + "_" + nodeID + "_select" );
        
        if( nodeSelect.value == "True" )
        {
            document.getElementById( tree.id + "_" + nodeID ).className = ESW_Tree_HoverStyle( tree, nodeID );
            nodeSelect.value = "False";
        }
        else
        {
            document.getElementById( tree.id + "_" + nodeID ).className = ESW_Tree_SelectHoverStyle( tree, nodeID );
            nodeSelect.value = "True";
        }
        result = false;
    }
    
    if( tree.ClientAfterClick != null )
    {
        var rst = eval( tree.ClientAfterClick );
        if( rst != null )
            return rst;
    }
    
    return result;
}

function ESW_Tree_mouseOver( tree, nodeID )
{
    var node = document.getElementById( tree.id + "_" + nodeID );
    if( ESW_Tree_isNodeSelected( tree, nodeID ) )
        node.className = ESW_Tree_SelectHoverStyle( tree, nodeID );
    else
        node.className = ESW_Tree_HoverStyle( tree, nodeID );
}

function ESW_Tree_mouseOut( tree, nodeID )
{
    var node = document.getElementById( tree.id + "_" + nodeID );
    if( ESW_Tree_isNodeSelected( tree, nodeID ) )
        node.className = ESW_Tree_SelectStyle( tree, nodeID );
    else
        node.className = ESW_Tree_NodeStyle( tree, nodeID );
}

function ESW_Tree_isNodeSelected( tree, nodeID )
{
    if( tree.SelectMode == "Single" || tree.SelectMode == "SinglePostback" )
    {
        var selectedID = document.getElementById( tree.id + "_selected" ).value;
        if( selectedID == nodeID )
            return true;
        else
            return false;
    }
    else if( tree.SelectMode == "Multiple"  || tree.SelectMode == "MultiplePostback" )
    {
        var nodeSelect = document.getElementById( tree.id + "_" + nodeID + "_select" );
        if( nodeSelect.value == "True" )
            return true;
        else
            return false;
    }
    else
        return false;
}

function ESW_Tree_Selected( tree )
{
    if( tree.SelectMode == "Single" || tree.SelectMode == "SinglePostback" )
        return document.getElementById( tree.id + "_selected" ).value;
    else if( tree.SelectMode == "Multiple"  || tree.SelectMode == "MultiplePostback" )
    {
        var selected = [];
        var nodes = document.getElementsByTagName( "input" );
        for( var i = 0; i < nodes.length; ++i )
        {
            if( nodes[i].type == "hidden" )
            {
                var parts = nodes[i].id.split( "_" );
                if( nodes[i].value == "True" )
                    selected.push( parts[0] + "_" + parts[1] );
            }
        }
        
        return selected;
    }
    else
        return null;
}

function ESW_Tree_NodeStyle( tree, nodeID )
{
    var nodeStyle = eval( "tree.NodeStyle_" + nodeID );
    if( nodeStyle == null )
    {
        if( tree.NodeStyle != null )
            nodeStyle = tree.NodeStyle;
        else
            nodeStyle = "";
    }
    else if( tree.NodeStyle != null && tree.NodeStyle.length > 0 )
        nodeStyle = tree.NodeStyle + " " + nodeStyle;
        
    return nodeStyle;
}

function ESW_Tree_SelectStyle( tree, nodeID )
{
    var selectStyle = eval( "tree.SelectedStyle_" + nodeID );
    if( selectStyle == null )
    {
        if( tree.SelectedStyle != null )
            selectStyle = tree.SelectedStyle;
        else
            selectStyle = "";
    }
    else if( tree.SelectedStyle != null && tree.SelectedStyle.length > 0 )
        selectStyle = tree.SelectedStyle + " " + selectStyle;
 
    var nodeStyle = ESW_Tree_NodeStyle( tree, nodeID );       
    if( nodeStyle.length > 0 )
        selectStyle = nodeStyle + " " + selectStyle;
        
    return selectStyle;
}

function ESW_Tree_HoverStyle( tree, nodeID )
{
    var hoverStyle = eval( "tree.HoverStyle_" + nodeID );
    if( hoverStyle == null )
    {
        if( tree.HoverStyle != null )
            hoverStyle = tree.HoverStyle;
        else
            hoverStyle = "";
    }
    else if( tree.HoverStyle != null && tree.HoverStyle.length > 0 )
        hoverStyle = tree.HoverStyle + " " + hoverStyle;
 
    var nodeStyle = ESW_Tree_NodeStyle( tree, nodeID );       
    if( nodeStyle.length > 0 )
        hoverStyle = nodeStyle + " " + hoverStyle;
        
    return hoverStyle;
}

function ESW_Tree_SelectHoverStyle( tree, nodeID )
{
    var hoverStyle = eval( "tree.HoverStyle_" + nodeID );
    if( hoverStyle == null )
    {
        if( tree.HoverStyle != null )
            hoverStyle = tree.HoverStyle;
        else
            hoverStyle = "";
    }
    else if( tree.HoverStyle != null && tree.HoverStyle.length > 0 )
        hoverStyle = tree.HoverStyle + " " + hoverStyle;
 
    var selectStyle = ESW_Tree_SelectStyle( tree, nodeID );
    if( selectStyle.length > 0 )
        hoverStyle = selectStyle + " " + hoverStyle;
        
    return hoverStyle;
}