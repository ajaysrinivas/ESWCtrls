/*
* jQuery.ls_tabctrl.js - tab ctrl plugin, specifically to work with asp.net control
*
* version 1.0.0 (2011/08/28) (C)opyright Leon Pennington 2011
* 
* Dual licensed under the MIT and GPL licenses: 
*   http://www.opensource.org/licenses/mit-license.php 
*   http://www.gnu.org/licenses/gpl.html 
*/
;(function($)
{
    $.fn.ls_sortcol = function (args)
    {
        var opts = $.extend(true, {
            hasHeader: true,
            hasFooter: false
        }, args);
        
        return this.each(function()
        {
            // First sort out the table
            var table = $(this);
            
            var head = null; 
            var headRow = null;			
            if(opts.hasHeader)
            {
                head = $("<thead></thead>");
                headRow = table.find("tr:first");
                headRow.remove();
                head.append(headRow);
            }
            
            var foot = null;
            var footRow = null;
            if(opts.hasFooter)
            {
                foot = $("<tfoot></tfoot>");
                footRow = table.find("tr:last");
                footRow.remove();			
                foot.append(footRow);
            }	
            
            var body = $("<tbody></tbody>");
            bodyRows = table.find("tr");
            bodyRows.remove();
            body.append(bodyRows);
            
            // Remove ghosts
            table.find("thead").remove();
            table.find("tbody").remove();
            table.find("tfoot").remove();
            
            if(opts.hasHeader)
                table.append(head);
            
            table.append(body);
            
            if(opts.hasFooter)
                table.append(foot);
            
            body.sortable({
                axis:"y",
                handle:".sortCol",
                containment:"parent",
                helper:function(e,ui)
                {
                    ui.children().each(
                        function(){
                            $(this).width($(this).width());
                        }
                    );
                    return ui;
                }
            });
        });
    };
})(jQuery);