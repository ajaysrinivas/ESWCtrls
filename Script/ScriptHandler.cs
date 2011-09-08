using System.IO;
using System.Web;

namespace ESWCtrls
{
    /// <summary>
    /// Handles the request for scripts
    /// </summary>
    public class ScriptHandler : IHttpHandler
    {
        /// <summary>
        /// Does the request processing
        /// </summary>
        public void ProcessRequest(HttpContext context)
        {
            string filename = Path.GetTempPath() + "esw_scripts\\" + Path.GetFileNameWithoutExtension(context.Request.FilePath);
            string encoding = context.Request.Headers["Accept-Encoding"];
            if(File.Exists(filename + ".jsc") && !string.IsNullOrEmpty(encoding) && (encoding.Contains("gzip") || encoding.Contains("deflate")))
            {
                byte[] scriptComp = null;
                using(FileStream fs = new FileStream(filename + ".jsc", FileMode.Open))
                {
                    MemoryStream ms = new MemoryStream();
                    byte[] bytes = new byte[1048576];
                    int read = fs.Read(bytes, 0, 1048576);
                    while(read > 0)
                    {
                        ms.Write(bytes, 0, read);
                        read = fs.Read(bytes, 0, 1048576);
                    }

                    scriptComp = ms.ToArray();
                }

                context.Response.ContentType = "text/javascript";
                context.Response.AppendHeader("Content-Encoding", "gzip");
                context.Response.AppendHeader("Content-Length", scriptComp.Length.ToString());
                context.Response.BinaryWrite(scriptComp);
                context.Response.StatusCode = 200;
                context.ApplicationInstance.CompleteRequest();
            }
            else if(File.Exists(filename + ".js"))
            {
                string scriptContent = null;
                using(StreamReader sr = new StreamReader(filename + ".js"))
                {
                    scriptContent = sr.ReadToEnd();
                }

                context.Response.ContentType = "text/javascript";
                context.Response.AppendHeader("Content-Length", scriptContent.Length.ToString());
                context.Response.Write(scriptContent);
                context.Response.StatusCode = 200;
                context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                context.Response.ContentType = "text/html";
                context.Response.Write("<html><body><h1>Missing script file</h1><p>Something is misconfigured in the setup</p></body></html>");
                context.Response.StatusCode = 404;
                context.ApplicationInstance.CompleteRequest();
            }
        }

        /// <summary>
        /// Whether the handler is resuable
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }

    }
}
