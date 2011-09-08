using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace ESWCtrls.Graphic
{
    /// <summary>
    /// The handler for WebGfx classes
    /// </summary>
    /// <remarks>
    /// This handler is essential to any application that uses the <see cref="WebGfx">WebGfx class.</see>
    /// 
    /// To provide the appropriate functionality you need to add this handler to the web.config
    /// of the web application thats using <see cref="WebGfx">WebGfx class</see>.
    /// <code>
    /// <![CDATA[
    /// <configuration>
    ///     ...
    ///     <system.web>
    ///         ...
    ///         <httpHandlers>
    ///             <add verb="GET" path="*.YOUREXT" type="ESWCtrls.WebGfxHandler, ESWCtrls" />
    ///             ...
    ///         </httpHandlers>
    ///         ...
    ///     </system.web>
    /// <configuration>
    /// ]]>
    /// </code>
    /// 
    /// You also need to add a mapping to IIS for the extension you use; if it's not one of the standard types. 
    /// To do this go into IIS Right click the application folder, and select properties. In the "Directory" 
    /// tab in "Application Settings", click the "Configuration..." button. 
    /// 
    /// In the new dialog that appears in the "Mappings" tab click "Add" The executable in this new dialog 
    /// needs to be pointed at the aspnet_isapi.dll that is on the current framework your using usually the 
    /// path is something like "C:\WINDOWS\Microsoft.NET\Framework\v2.0.xxxx\aspnet_isapi.dll".
    /// 
    /// You should set the limit to radio button, with the text field set to "GET" make sure that 
    /// "Check that file exists" is unchecked and click okay.
    /// 
    /// In certain version of IIS the okay button sometimes will not enable until you have click the excecutable text
    /// field so that it expands. 
    /// </remarks>
    class WebGfxHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(System.Web.HttpContext context)
        {
            string id = "wgx_" + Path.GetFileNameWithoutExtension(context.Request.FilePath);
            if(context.Application[id] is WebGfx)
            {
                WebGfx img = (WebGfx)context.Application[id];
                context.ClearError();
                context.Response.Clear();

                ImageCodecInfo codec = FindEncoder(img.ImageFormat);
                context.Response.ContentType = codec.MimeType;

                EncoderParameters cp = new EncoderParameters(1);
                Encoder q = Encoder.Quality;
                EncoderParameter qp = new EncoderParameter(q, img.Quality);
                cp.Param[0] = qp;

                Bitmap image = new Bitmap( img.Image());
                if(codec.MimeType == "image/png")
                {
                    MemoryStream ms = new MemoryStream();
                    image.Save(ms, codec, cp);
                    ms = StripGAMA(ms.ToArray());
                    ms.WriteTo(context.Response.OutputStream);
                }
                else
                    image.Save(context.Response.OutputStream, codec, cp);
                    
                image.Dispose();
                context.Response.StatusCode = 200;
                context.ApplicationInstance.CompleteRequest();
            }

            System.Collections.Generic.List<WebGfx> list = new System.Collections.Generic.List<WebGfx>();
            foreach(object obj in context.Application)
            {
                if(obj is WebGfx)
                {
                    if(((WebGfx)obj).CheckExpire())
                        list.Add((WebGfx)obj);
                }
            }

            foreach(WebGfx obj in list)
                context.Application.Remove("wgx_" + obj.FileName);
        }

        private MemoryStream StripGAMA(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(bytes, 0, 8);

            int offset = 8;
            byte[] chkLenBytes = new byte[4];
            int chkLength = 0; 
            string chkType = string.Empty; 

            while (offset < bytes.Length-12)
            {
                chkLenBytes[0] = bytes[offset];
                chkLenBytes[1] = bytes[offset + 1];
                chkLenBytes[2] = bytes[offset + 2];
                chkLenBytes[3] = bytes[offset + 3];
                if (System.BitConverter.IsLittleEndian)
                    System.Array.Reverse(chkLenBytes);
                chkLength = System.BitConverter.ToInt32(chkLenBytes, 0);
                chkType = System.Text.Encoding.ASCII.GetString(bytes, offset + 4, 4);

                if (chkType != "gAMA")
                {
                    if (chkType == "IDAT" || chkType == "PLTE")
                    {
                        ms.Write(bytes, offset, bytes.Length - offset);
                        break;
                    }
                    else
                    {
                        ms.Write(bytes, offset, 12 + chkLength);
                        offset += 12 + chkLength;
                    }
                }
                else
                {
                    offset += 12 + chkLength;
                    ms.Write(bytes, offset, bytes.Length - offset);
                    break;
                }
            }

            return ms;
        }


        private ImageCodecInfo FindEncoder(ImageFormat format)
        {
            foreach(ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
            {
                if(codec.FormatID.Equals(format.Guid))
                    return codec;
            }

            return null;
        }

    }
}
