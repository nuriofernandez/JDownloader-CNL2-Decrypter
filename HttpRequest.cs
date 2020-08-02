using System;
using System.IO;
using System.Net;
using System.Text;

namespace JDownloaderService
{
	class HttpRequest
	{

		public HttpListenerContext Context;

		public HttpRequest(HttpListenerContext context)
		{
			this.Context = context;
		}

		public void SendResponse(String body)
		{
			byte[] buffer = Encoding.UTF8.GetBytes(body);
			Context.Response.ContentLength64 = buffer.Length;

			// output response
			Stream output = Context.Response.OutputStream;
			output.Write(buffer, 0, buffer.Length);
			output.Close();
		}

	}
}
