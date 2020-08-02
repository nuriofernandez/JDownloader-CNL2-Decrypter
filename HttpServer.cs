using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace JDownloaderService
{

	class HttpServer
	{

		protected HttpListener Listener;

		public void Start()
		{
			Listener = new HttpListener();
			Listener.Prefixes.Add("http://*:9666/");
			Listener.Start();
			WaitConnection();
			Console.WriteLine("Web server started.");
		}

		public void Stop()
		{
			Listener.Stop();
			Listener = null;
		}

		private void WaitConnection()
		{
			Listener.BeginGetContext(new AsyncCallback(WebRequestCallback), this.Listener);
		}

		private void WebRequestCallback(IAsyncResult result)
		{
			HttpListenerContext context = Listener.EndGetContext(result);
			WaitConnection();

			ProcessRequest(context);
		}

		private void ProcessRequest(HttpListenerContext context)
		{
			Debug.WriteLine(context.Request.RawUrl);

			HttpRequest request = new HttpRequest(context);
			request.Context.Response.StatusCode = 200;
			request.Context.Response.Headers.Add("Content-Type: text/html");

			if (context.Request.RawUrl.StartsWith("/flash"))
			{
				if (context.Request.RawUrl.Contains("addcrypted2"))
				{
					Stream body = context.Request.InputStream;
					StreamReader reader = new StreamReader(body, context.Request.ContentEncoding);

					String requestBody = HttpUtility.UrlDecode(reader.ReadToEnd());

					// get encrypted data
					Regex rgxData = new Regex("crypted=(.*?)(&|$)");
					String data = rgxData.Match(requestBody).Groups[1].ToString();

					// get encrypted pass
					Regex rgxPass = new Regex("jk=(.*?){(.*?)}(&|$)");
					String pass = rgxPass.Match(requestBody).Groups[2].ToString();

					var jsEngine = new Jurassic.ScriptEngine();
					pass = jsEngine.Evaluate("(function (){" + pass + "})()").ToString();

					string link = Decrypter.DecryptLinks(pass, data);
					request.SendResponse("<script>window.location.href = '" + link + "'</script>");
				}

			}

		}

	}

}
