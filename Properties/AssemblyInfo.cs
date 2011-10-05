using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.UI;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("ESWCtrls")]
[assembly: AssemblyDescription("Web Controls")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Economic Solutions")]
[assembly: AssemblyProduct("ESWCtrls")]
[assembly: AssemblyCopyright("Copyright ©Leon Pennington 2008-2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("e1bf46d1-75e8-4710-a519-eb013c011d4a")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("3.1.*")]


// jQuery Scripts
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.jquery.js", "text/javascript")]

// jQuery UI
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.jquery-ui-full.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.ui.core.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.ui.accordion.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.ui.autocomplete.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.ui.button.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.ui.datepicker.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.ui.dialog.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.ui.draggable.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.ui.droppable.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.ui.mouse.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.ui.position.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.ui.progressbar.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.ui.resizable.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.ui.selectable.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.ui.slider.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.ui.sortable.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.ui.tabs.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.ui.widget.js", "text/javascript")]

// jQuery Effects
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.effects.core.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.effects.blind.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.effects.bounce.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.effects.clip.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.effects.drop.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.effects.explode.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.effects.fade.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.effects.fold.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.effects.highlight.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.effects.pulsate.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.effects.scale.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.effects.shake.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.effects.slide.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.effects.transfer.js", "text/javascript")]

//My Jquery Layout
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.panels.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.sortcolumn.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.helpbox.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.tabctrl.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.popup.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.datepicker.js", "text/javascript")]

//Number Validator
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.NumberValidator.js", "text/javascript")]

//Other Validators
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.Validators.js", "text/javascript")]

//Busy Box
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.BusyBox.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.busybox.busy.gif", "img/gif")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.busybox.busy00.png", "img/png")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.busybox.busy01.png", "img/png")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.busybox.busy02.png", "img/png")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.busybox.busy03.png", "img/png")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.busybox.busy04.png", "img/png")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.busybox.busy05.png", "img/png")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.busybox.busy06.png", "img/png")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.busybox.busy07.png", "img/png")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.busybox.busy08.png", "img/png")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.busybox.busy09.png", "img/png")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.busybox.busy10.png", "img/png")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.busybox.busy11.png", "img/png")]

//DispBox
[assembly: WebResource("ESWCtrls.ResEmbed.Styles.DispBox.css", "text/css")]

//Drop Down Menu
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.jquery.dropdownmenu.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.arrow_down.gif", "img/gif")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.arrow_right.gif", "img/gif")]

//Tree View
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.TreeView.js", "text/javascript")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.tree.bline.gif", "img/gif")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.tree.box.gif", "img/gif")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.tree.cbox.gif", "img/gif")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.tree.ebox.gif", "img/gif")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.tree.mline.gif", "img/gif")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.tree.sline.gif", "img/gif")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.tree.tline.gif", "img/gif")]
[assembly: WebResource("ESWCtrls.ResEmbed.Gfxs.tree.vline.gif", "img/gif")]

//Password
[assembly: WebResource("ESWCtrls.ResEmbed.Scripts.Password.js", "text/javascript")]
