using System;
using System.IO;
using System.IO.Compression;
using System.Web.UI;

namespace ESWCtrls
{

    /// <summary>
    /// Base class for eliminating white space from pages, and compressing viewstate
    /// </summary>
    public class CompressPage : System.Web.UI.Page
    {

        /// <summary>
        /// Loads the compressed viewstate
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        protected override object LoadPageStateFromPersistenceMedium()
        {
            string viewState = Request.Form["__VSTATE"];
            if(viewState.StartsWith("C$"))
            {
                byte[] bytes = Convert.FromBase64String(viewState.Substring(2));

                MemoryStream ms = new MemoryStream();
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                GZipStream gzip = new GZipStream(ms, CompressionMode.Decompress, true);
                MemoryStream ms2 = new MemoryStream();
                byte[] buff = new byte[65];
                int read = -1;
                read = gzip.Read(buff, 0, buff.Length);
                while(read > 0)
                {
                    ms2.Write(buff, 0, read);
                    read = gzip.Read(buff, 0, buff.Length);
                }
                gzip.Close();
                bytes = ms2.ToArray();

                LosFormatter format = new LosFormatter();
                return format.Deserialize(Convert.ToBase64String(bytes));
            }
            else
            {
                LosFormatter format = new LosFormatter();
                return format.Deserialize(viewState);
            }
        }

        /// <summary>
        /// saves the compressed viewstate
        /// </summary>
        /// <param name="viewState">The viewstate object to compress</param>
        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            LosFormatter format = new LosFormatter();
            StringWriter sw = new StringWriter();
            format.Serialize(sw, viewState);
            string vss = sw.ToString();


            if(vss.Length > 512)
            {
                byte[] bytes = Convert.FromBase64String(vss);



                MemoryStream ms = new MemoryStream();
                GZipStream gzip = new GZipStream(ms, CompressionMode.Compress, true);
                gzip.Write(bytes, 0, bytes.Length);
                gzip.Close();
                bytes = ms.ToArray();

                string vsc = Convert.ToBase64String(bytes);
                if(vsc.Length < vss.Length)
                    ScriptManager.RegisterHiddenField(Page, "__VSTATE", "C$" + vsc);
                else
                    ScriptManager.RegisterHiddenField(Page, "__VSTATE", vss);
            }
            else
                ScriptManager.RegisterHiddenField(Page, "__VSTATE", vss);
        }

        ///
        protected override System.Web.UI.HtmlTextWriter CreateHtmlTextWriter(System.IO.TextWriter tw)
        {
            if (Context.IsDebuggingEnabled)
                return base.CreateHtmlTextWriter(tw);
            else
                return new CompactHtmlTextWriter(tw);
        }

    }

    /// <summary>
    /// HtmlWriter that eliminates white space from HTML Output
    /// </summary>
    public class CompactHtmlTextWriter : HtmlTextWriter
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="writer">The text writer to use</param>
        public CompactHtmlTextWriter(TextWriter writer)
            : base(writer)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="writer">The text writer to use</param>
        /// <param name="tabString">Whatever character is given is replaced by string.Empty</param>
        public CompactHtmlTextWriter(TextWriter writer, string tabString)
            : base(writer, string.Empty)
        { }

        ///
        protected override void OutputTabs()
        { }

        ///
        public override string NewLine
        {
            get { return " ";  }
            set { base.NewLine = value; }
        }

        #region WriteLine

        ///
        public override void WriteLine()
        { }

        ///
        public override void WriteLine(string format, object arg0)
        {
            base.WriteLine(StripWhiteSpace(format).Trim(), arg0);
        }

        ///
        public override void WriteLine(string format, object arg0, object arg1)
        {
            base.WriteLine(StripWhiteSpace(format).Trim(), arg0, arg1);
        }

        ///
        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            base.WriteLine(StripWhiteSpace(format).Trim(), arg0, arg1, arg2);
        }

        ///
        public override void WriteLine(string format, params object[] arg)
        {
            base.WriteLine(StripWhiteSpace(format).Trim(), arg);
        }

        ///
        public override void WriteLine(string s)
        {
            base.WriteLine(this.StripWhiteSpace(s).Trim());
        }

        #endregion

        #region Tags

        ///
        protected override string RenderAfterContent()
        {
            return null;
        }

        ///
        protected override string RenderAfterTag()
        {
            return null;
        }

        ///
        protected override string RenderBeforeContent()
        {
            return null;
        }

        ///
        protected override string RenderBeforeTag()
        {
            return null;
        }

        #endregion

        ///
        private string StripWhiteSpace(string s)
        {
            s = s.Replace("\t", " ");
            while (s.Contains("  "))
                s = s.Replace("  ", " ");

            return s;
        }

    }
}