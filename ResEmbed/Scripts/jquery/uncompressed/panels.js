/*
* jQuery.ls_panels.js - layout panels plugin
*
* version 1.0.4 (2010/08/09) (C)opyright Leon Pennington 2010
* 
* Dual licensed under the MIT and GPL licenses: 
*   http://www.opensource.org/licenses/mit-license.php 
*   http://www.gnu.org/licenses/gpl.html 
*/
;(function($){
    $.fn.ls_panels = function(args){
        var opts = $.extend(true,{
            centrePanel:null,     // The centre panel must ALWAYS be available or nothing happens
            centerPanel:null,     // same as above just a diffrent spelling
            barClass:null,        // The default class for all dividing bars
            ghostDrag:false,      // Use a ghost bar, rather than resizing everything while dragging
            ghostOpacity:0.5,     // The opacity to use when ghosting
            store:null,           // An input to store the values of the panels for gathering during postback must be an ID
            left:{
                    panel:null,             // The selector for the panel
                    resizeable:true,        // If this panel is resizable
                    minSize:0,              // The minimum size of the panel
                    maxSize:0,              // The maximum size of the panel
                    barClass:null,          // The class to use for its bar, if you don't want the default
                    autoHide:false,         // Whether the panel auto hides to start with
                    pinElem:null,           // Any element to use for pinning the panels toggles autoHide
                    pinElemClass:null       // The class to toggle on the pin for switching it on and off
                },
            right:{
                    panel:null,
                    resizeable:true,
                    minSize:0,
                    maxSize:0,
                    barClass:null,
                    autoHide:false,
                    pinElem:null,
                    pinElemClass:null
                }
        },args);

        this.each(function(){
            var _main = $(this);
            
            var _cPnl = null;
            if(opts.centrePanel == null){
                if(opts.centerPanel == null)
                    return;
                else
                    _cPnl = _main.children(opts.centerPanel);
            }
            else
                _cPnl = _main.children(opts.centrePanel);    
                
            
            if(opts.store != null)
                _main.store = $(opts.store);
            
            
            _cPnl.css("overflow","auto");
            var hgt = _cPnl.height();
            
            var _lPnl = null;
            if(opts.left.panel != null)
                _lPnl = setupPanel(opts.left,"left");         
            
            var _rPnl = null;
            if( opts.right.panel != null)
                _rPnl = setupPanel(opts.right,"right");
            
            hgt = _main.height() > hgt ? _main.height() : hgt;
            _main.height(hgt); _cPnl.height(hgt);
            if(_lPnl != null){
                _lPnl.height(hgt); _lPnl.outPnl.height(hgt); _lPnl.bar.height(hgt);
            }
            if(_rPnl != null){
                _rPnl.height(hgt); _rPnl.outPnl.height(hgt); _rPnl.bar.height(hgt);
            }
            
            calcSizes();
            $(window).bind("resize",calcSizes);
            store();
            
            function setupPanel(options,position)
            {
                var pnl = _main.children(options.panel);
                pnl.opts = options;
                pnl.pos = position;
                hgt = pnl.height() > hgt ? pnl.height() : hgt;
                pnl.css({overflow:"auto",float:"left"});
                
                pnl.outPnl = $("<div style='float:" + position +";'></div>");
                pnl.bar = $("<div style='float:left;'><span></span></div>");
                pnl.outPnl.append(pnl);
                if(position == "left")
                	pnl.outPnl.append(pnl.bar);
                else
                	pnl.outPnl.prepend(pnl.bar);
                
                pnl.bar[0].pnl = pnl;
                pnl.outPnl[0].pnl = pnl;
                
                if(position == "left")
                    _main.prepend(pnl.outPnl);
                else{
                    _main.append(pnl.outPnl);
                    _main.append(_cPnl);
                }
                
                if(options.resizeable)
                    pnl.bar.css("cursor","e-resize").bind("mousedown",startDrag);
                    
                var bCls = options.barClass != null ? options.barClass : opts.barClass;
                if(bCls != null)
                    pnl.bar.addClass(bCls);
                else{
                    pnl.bar.css({width:"6px",background:"red"});
                    pnl.bar.hover(function(){$(this).css("background","blue");},function(){$(this).css("background","red");});
                }                   
                
                pnl.origWidth = pnl.width();
                    
                if(position == "right")
                    pnl.outPnl.css("right", $(window).width() - (pnl.outPnl.offset().left + pnl.outPnl.width()));
                    
                if(options.autoHide){
                    pnl.outPnl.css({position:"absolute"});
                    pnl.outPnl.hover(unhidePanel,hidePanel);
                    pnl.animate({width:"0px"},0).css("display","none");
                    _cPnl.css("padding-" + position,pnl.bar.outerWidth() + "px");                
                }
                
                if(options.pinElem != null){
                    pnl.pin = pnl.find(options.pinElem); 
                    pnl.pin[0].pnl = pnl;
                    pnl.pin.click(pinPanel);
                }
                
                return pnl;
            };
                        
            function calcSizes(){
                if(_lPnl != null){
                    _lPnl.minDrag = _lPnl.offset().left + opts.left.minSize;
                    _lPnl.maxDrag = opts.left.maxSize > 0 ? _lPnl.offset().left + opts.left.maxSize : _main.offset().left + _main.innerWidth();
                }
                
                if(_rPnl != null){
                    _rPnl.minDrag = _main.offset().left + ((opts.right.maxSize > 0) ? (_main.innerWidth() - opts.right.maxSize) : 0);
                    _rPnl.maxDrag = _main.offset().left + _main.innerWidth() - opts.right.minSize;
                }
            };
            
            function startDrag(e){
                if(e.target != this)
                    return;
                _main.chgPnl = this.pnl;                    
                if(this.pnl.outPnl.css("position") == "absolute")
                    this.pnl.outPnl.unbind("mouseenter mouseleave");
                    
                if(opts.ghostDrag){
                    _main.ghost = this.pnl.bar.clone();
                    _main.ghost.css({position:"absolute",zindex:100,opacity:opts.ghostOpacity});
                    $(_main).append(_main.ghost);
                    _main.ghost.css({top:this.pnl.bar.offset().top + "px",left:this.pnl.bar.offset().left + "px"});
                    _main.incr = 0;
                }
                    
                _main.initPos = e.pageX;
                $(document).bind("mousemove",performDrag).bind("mouseup",endDrag);
                e.preventDefault();
                disableSelect();
            };
            
            function performDrag(e){
                if(e.pageX < _main.chgPnl.minDrag || e.pageX > _main.chgPnl.maxDrag)
                    return;
                var incr = _main.initPos - e.pageX;
                
                if(opts.ghostDrag){
                    var pos = _main.ghost.offset();
                    pos.left -= incr;
                    _main.incr += incr;
                    _main.ghost.offset(pos);
                }
                else{
                    if(_main.chgPnl.pos == "left")
                        incr =  0 - incr;    
                    _main.chgPnl.width(_main.chgPnl.width() + incr);
                }
                
                _main.initPos = e.pageX;
            };
            
            function endDrag(e){
                $(document).unbind("mousemove", performDrag).unbind("mouseup", endDrag);
                
                if(opts.ghostDrag){
                    if(_main.chgPnl.pos == "left")
                        _main.incr = 0 - _main.incr;
                    _main.chgPnl.width(_main.chgPnl.width() + _main.incr);
                    _main.ghost.remove();
                }
                
                _main.chgPnl.origWidth = _main.chgPnl.width();
                if(_main.chgPnl.outPnl.css("position") == "absolute")
                    _main.chgPnl.outPnl.hover(unhidePanel,hidePanel);
                store();
                
                enableSelect();
            };
            
            function disableSelect(){
                if(window.getSelection)
                    window.getSelection().removeAllRanges();
                else if(document.selection)
                    document.selection.empty();  
                var body = $("body")[0];
                if(typeof body.onselectstart != null)
                    body.onselectstart = function(){return false;};
                else{
                    body.style.MozUserSelect = "none";
                    body.style.webkitUserSelect = "none";
                }
            };
            
            function enableSelect(){
                var body = $("body")[0];
                if(typeof body.onselectstart != null)
                    body.onselectstart = null;
                else{
                    body.style.MozUserSelect = "auto";
                    body.style.webkitUserSelect = "auto";
                }
            };
            
            function unhidePanel(e){
                this.pnl.stop(true);
                this.pnl.delay(400).show(1).animate({width:this.pnl.origWidth + "px"},"slow");
            };
            
            function hidePanel(e){
                this.pnl.stop(true); clearTimeout(this.pnl.timout);
                this.pnl.delay(400).animate({width:"0px"},"slow").hide(1);
            };
            
            function pinPanel(e){
                if(this.pnl.outPnl.css("position") == "absolute"){
                    this.pnl.outPnl.css("position","static");
                    this.pnl.outPnl.unbind("mouseenter mouseleave");
                    if(this.pnl.opts.pinElemClass != null)
                        this.pnl.pin.addClass(this.pnl.opts.pinElemClass);
                    _cPnl.css("padding-" + this.pnl.pos,"0"); 
                }
                else{
                    this.pnl.outPnl.css("position","absolute");
                    this.pnl.outPnl.hover(unhidePanel,hidePanel);
                    if(this.pnl.opts.pinElemClass != null)
                        this.pnl.pin.removeClass(this.pnl.opts.pinElemClass);
                    _cPnl.css("padding-" + this.pnl.pos,this.pnl.bar.outerWidth() + "px"); 
                }
                store();
            };
            
            function store(){
                if(opts.store == null)
                    return;
                    
                var rst = "";
                if(_lPnl != null)
                    rst = "left{width:" + _lPnl.origWidth + ",autoHide:" + ((_lPnl.outPnl.css("position") == "absolute")?"true":"false") + "}";
                rst += ";";
                if(_rPnl != null)
                    rst += "right{width:" + _rPnl.origWidth + ",autoHide:" + ((_rPnl.outPnl.css("position") == "absolute")?"true":"false") + "}";
                    
                _main.store.val(rst);
            };
        });
    };
})(jQuery);