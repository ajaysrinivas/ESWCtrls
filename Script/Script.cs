using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Linq;

namespace ESWCtrls
{
    /// <summary>
    /// Control for installing useful scripts to a page, mainly deals with jquery
    /// </summary>
    [ToolboxData("<{0}:script runat=\"server\" />")]
    public class Script : WebControl
    {
        /// <summary>
        /// The list of scripts to add to the page
        /// </summary>
        public Script()
        {
            _resScripts = new List<string>();
            _startScripts = new List<string>();
        }

        /// <summary>
        /// Comma seperated list of scripts to use
        /// </summary>
        [Bindable(false), Category("Data")]
        public string scripts
        {
            get { return String.Join(",", _resScripts); }
            set
            {
                _resScripts.Clear();
                string[] names = value.Split(',');
                foreach(string name in names)
                    AddWithDepends(name.Trim());
            }
        }

        /// <summary>
        /// Try to use CDN instead of internal script
        /// </summary>
        [Bindable(false), Category("Behaviour"), DefaultValue(true)]
        public bool TryCDN
        {
            get
            {
                if (ViewState["TryCDN"] != null)
                    return (bool)ViewState["TryCDN"];
                else
                    return true;
            }
            set
            {
                if (value != true)
                    ViewState["TryCDN"] = value;
                else
                    ViewState.Remove("TryCDN");
            }
        }

        #region Render

        ///
        protected override void Render(HtmlTextWriter writer)
        {
            if(_resScripts.Count > 0)
            {
                List<string> rs = new List<string>(_resScripts);
                if(TryCDN)
                {
                    if(rs.Contains("jquery.js"))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, "http://ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js");
                        writer.RenderBeginTag(HtmlTextWriterTag.Script);
                        writer.RenderEndTag();
                        writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                        writer.RenderBeginTag(HtmlTextWriterTag.Script);
                        writer.Write(string.Format("if (typeof jQuery != 'object') document.write(unescape(\"%3Cscript type=\\\"text/javascript\\\" src=\\\"{0}\\\"%3E%3C/script%3E\"));",Page.Server.UrlEncode(Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Scripts.jquery.jquery.js"))));
                        writer.RenderEndTag();
                    }


                    if(rs.Contains("jquery.ui.core.js") || rs.Contains("jquery.effects.core.js"))
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, "http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.16/jquery-ui.min.js");
                        writer.RenderBeginTag(HtmlTextWriterTag.Script);
                        writer.RenderEndTag();
                        writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                        writer.RenderBeginTag(HtmlTextWriterTag.Script);
                        writer.Write(string.Format("if (!jQuery.ui) document.write(unescape(\"%3Cscript type=\\\"text/javascript\\\" src=\\\"{0}\\\"%3E%3C/script%3E\"));",Page.Server.UrlEncode(Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Scripts.jquery.jquery-ui-full.js"))));
                        writer.RenderEndTag();
                    }

                    rs.RemoveAll(x => x == "jquery.js" || x.Contains("jquery.ui.") || x.Contains("jquery.effects."));
                }

                if(!string.IsNullOrEmpty(Extension))
                {
                    string filename = CalcFilename(rs);
                    string fullFilename = Path.GetTempPath() + "esw_scripts\\" + filename;
                    string scriptContents = null;

                    if(!File.Exists(fullFilename + ".js"))
                    {
                        scriptContents = ScriptContents(rs);
                        if(!Directory.Exists(Path.GetTempPath() + "esw_scripts"))
                            Directory.CreateDirectory(Path.GetTempPath() + "esw_scripts");
                        using(StreamWriter sw = new StreamWriter(fullFilename + ".js"))
                        {
                            sw.Write(scriptContents);
                        }
                    }

                    if(!File.Exists(fullFilename + ".jsc"))
                    {
                        if(string.IsNullOrEmpty(scriptContents))
                            scriptContents = ScriptContents(rs);

                        byte[] bytes = Encoding.UTF8.GetBytes(scriptContents);
                        MemoryStream ms = new MemoryStream();
                        GZipStream gzip = new GZipStream(ms, CompressionMode.Compress, true);
                        gzip.Write(bytes, 0, bytes.Length);
                        gzip.Close();
                        bytes = ms.ToArray();

                        if(!Directory.Exists(Path.GetTempPath() + "esw_scripts"))
                            Directory.CreateDirectory(Path.GetTempPath() + "esw_scripts");
                        using(FileStream fs = new FileStream(fullFilename + ".jsc", FileMode.OpenOrCreate))
                        {
                            fs.Write(bytes, 0, bytes.Length);
                        }
                    }
                    
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, filename + Extension);
                    writer.RenderBeginTag(HtmlTextWriterTag.Script);
                    writer.RenderEndTag();
                }
                else
                {
                    foreach(string name in rs)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                        if(name == "jquery.js")
                            writer.AddAttribute(HtmlTextWriterAttribute.Src, Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Scripts.jquery.jquery.js"));
                        else
                            writer.AddAttribute(HtmlTextWriterAttribute.Src, Page.ClientScript.GetWebResourceUrl(this.GetType(), "ESWCtrls.ResEmbed.Scripts." + name));
                        writer.RenderBeginTag(HtmlTextWriterTag.Script);
                        writer.RenderEndTag();
                    }
                }
            }
            if(_startScripts.Count > 0)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                writer.RenderBeginTag(HtmlTextWriterTag.Script);

                if(!Context.IsDebuggingEnabled)
                {
                    writer.Write("$(document).ready(function(){");
                    foreach(string s in _startScripts)
                        writer.Write(s);
                    writer.Write("});");
                }
                else
                {
                    writer.WriteLine("$(document).ready(function(){");
                    foreach(string s in _startScripts)
                        writer.WriteLine(s);
                    writer.WriteLine("});");
                }

                
                writer.RenderEndTag();
            }
        }

        #endregion

        #region Internal

        /// <summary>
        /// Adds a script to the script tag to register
        /// </summary>
        /// <param name="page">The page we are working with</param>
        /// <param name="names">The names of the scripts to add</param>
        internal static void AddResourceScript(Page page, params string[] names)
        {
            Script s = Current(page);
            foreach (string n in names)
                s.AddWithDepends(n);
        }

        internal static void AddStartupScript(Control ctrl, string script)
        {
            if (Internal.Util.InUpdatePanel(ctrl))
                ScriptManager.RegisterStartupScript(ctrl, ctrl.GetType(), ctrl.ClientID, script, true);
            else
            {
                Script s = Current(ctrl.Page);
                s._startScripts.Add(script);
            }
        }

        internal static void AddStartupScript(Control ctrl, string jQueryFunction, List<string> opts)
        {
            string scrText = string.Format("$(\"#{0}\").{1}({{{2}}});", ctrl.ClientID, jQueryFunction, string.Join(",", opts));
            if (Internal.Util.InUpdatePanel(ctrl))
                ScriptManager.RegisterStartupScript(ctrl, ctrl.GetType(), ctrl.ClientID, scrText, true);
            else
            {
                Script s = Current(ctrl.Page);
                s._startScripts.Add(scrText);
            }
        }

        #endregion

        private void AddWithDepends(string name)
        {
            if(name.StartsWith("jquery") && name != "jquery.js")
            {
                if(name == "jquery.effects.core.js" || name == "jquery.ui.core.js")
                    AddWithDepends("jquery.js");
                else if(name.StartsWith("jquery.effects."))
                    AddWithDepends("jquery.effects.core.js");
                else if(name.StartsWith("jquery.ui."))
                {
                    using(StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ESWCtrls.ResEmbed.Scripts." + name)))
                    {
                        string line = sr.ReadLine();
                        bool depends = false;
                        while(!line.Contains("*/"))
                        {
                            if(!depends)
                            {
                                if(line.Contains("Depends:"))
                                    depends = true;
                            }
                            else
                            {
                                line = line.Replace('*', ' ').Trim();
                                if(!string.IsNullOrEmpty(line))
                                    AddWithDepends(line);
                            }
                            line = sr.ReadLine();
                        }
                    }
                }
                else
                    AddWithDepends("jquery.js");
            }

            if(!_resScripts.Contains(name))
                _resScripts.Add(name);
        }

        private static Script Current(Page page)
        {
            Script s = null;
            foreach(Control c in page.Header.Controls)
            {
                if(c is ESWCtrls.Script)
                {
                    s = c as ESWCtrls.Script;
                    break;
                }
            }

            if(s == null)
            {
                s = new Script();
                page.Header.Controls.Add(s);
            }

            return s;
        }

        private string CalcFilename(List<string> nameList)
        {
            SHA1Managed hash = new SHA1Managed();
            string names = string.Join("|", nameList).Replace(".js", "").Replace("jquery", "jq").Replace("effects", "fx");
            byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(names));
            return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "a");
        }

        private string ScriptContents(List<string> nameList)
        {
            StringBuilder rst = new StringBuilder();
            foreach(string name in nameList)
            {
                string resName = "ESWCtrls.ResEmbed.Scripts." + name;
                if(name == "jquery.js")
                    resName = "ESWCtrls.ResEmbed.Scripts.jquery.jquery.js";

                using(StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resName)))
                {
                    rst.Append(sr.ReadToEnd());
                    rst.AppendLine(";");
                }
            }
            return rst.ToString();
        }


        /// <summary>
        /// The extension to use for the scripts
        /// </summary>
        public static string Extension
        {
            get
            {
                if(_wscExt == null)
                {
                    // Not using system.configuration as its a bit cumbersome, and doesn't work with iis7, and there is no equivalent for iis7
                    // that can be used everywhere, since they decided not to put the dll's in .net but with iis7 itself. which makes them almost
                    // completly useless to everyone. I don't think they're even in the test server.
                    XmlDocument doc = new XmlDocument();
                    doc.Load(HttpContext.Current.Server.MapPath("~/web.config"));

                    // look for iis6 and below section
                    XmlNodeList nodes = doc.SelectNodes("//system.web/httpHandlers/add[@type='ESWCtrls.ScriptHandler, ESWCtrls']");
                    if(nodes.Count == 0) // now check iis7
                        nodes = doc.SelectNodes("//system.webServer/handlers/add[@type='ESWCtrls.ScriptHandler, ESWCtrls']");
                        
                    if(nodes.Count > 0)
                    {
                        XmlAttribute a = nodes[0].Attributes["path"];
                        if(a != null)
                            _wscExt = a.Value.Substring(1);
                    }
                    else
                    {
                        // Can't find them mark as unknown
                        _wscExt = "";
                    }
                        
                }
                return _wscExt;
            }
        }

        private List<String> _resScripts;
        private List<string> _startScripts;
        private static string _wscExt = null;
    }
}
