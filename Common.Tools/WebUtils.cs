using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Linq;
using System.Diagnostics;

namespace Ujeby.Common.Tools
{
	public class WebUtils
	{
		private static string CurrentClassName { get { return typeof(WebUtils).Name; } }

		public static string WebRequest(string url, string method = "GET", string postData = null)
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			var responseData = WebRequestInternal(url, method, postData);

			stopwatch.Stop();

			Log.WriteLine($"{ CurrentClassName }.{ Utils.GetCurrentMethodName() }({ url }) in { stopwatch.ElapsedMilliseconds }ms");
			return responseData;
		}

		public static string SilentWebRequest(string url, string method = "GET", string postData = null)
		{
			return WebRequestInternal(url, method, postData);
		}

		public static string Post(string url, string postData, Dictionary<string, string> headers = null)
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			var responseData = WebRequestInternal(url, "POST", postData, headers);

			stopwatch.Stop();

			Log.WriteLine($"{ CurrentClassName }.{ Utils.GetCurrentMethodName() }({ url }) in { stopwatch.ElapsedMilliseconds }ms");
			return responseData;
		}

		private static string WebRequestInternal(string url, string method = "GET", string postData = null, Dictionary<string, string> headers = null)
		{
			var responseData = "";
			try
			{
				var cookieJar = new System.Net.CookieContainer();
				var hwrequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
				hwrequest.CookieContainer = cookieJar;
				hwrequest.Accept = "*/*";
				hwrequest.AllowAutoRedirect = true;
				hwrequest.UserAgent = "http_requester/0.1";
				hwrequest.Timeout = 60000;
				hwrequest.Method = method;

				if (headers != null)
				{
					foreach (var header in headers)
						hwrequest.Headers.Add(header.Key, header.Value);
				}

				if (hwrequest.Method == "POST")
				{
					hwrequest.ContentType = "application/x-www-form-urlencoded";
					// Use UTF8Encoding instead of ASCIIEncoding for XML requests:
					var encoding = new System.Text.ASCIIEncoding();
					var postByteArray = encoding.GetBytes(postData);
					hwrequest.ContentLength = postByteArray.Length;
					var postStream = hwrequest.GetRequestStream();
					postStream.Write(postByteArray, 0, postByteArray.Length);
					postStream.Close();
				}

				var hwresponse = (System.Net.HttpWebResponse)hwrequest.GetResponse();
				if (hwresponse.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var responseStream = hwresponse.GetResponseStream();
					var myStreamReader = new System.IO.StreamReader(responseStream);
					responseData = myStreamReader.ReadToEnd();
				}
				hwresponse.Close();
			}
			catch (Exception ex)
			{
				Log.WriteLine($"{ CurrentClassName }.{ Utils.GetCurrentMethodName() }({ url }): null, { ex.ToString() }");
				return null;
			}

			return responseData;
		}

		public static Image DownloadImage(string url, out ImageFormat imageFormat)
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			try
			{
				var webClient = new WebClient();
				webClient.Headers.Add("User-Agent: Other");

				var data = webClient.DownloadData(url);

				var mem = new MemoryStream(data);
				var image = Image.FromStream(mem);

				var extension = url.Substring(url.LastIndexOf('.') + 1);
				imageFormat = Tools.Graphics.GetImageFormat(extension);

				stopwatch.Stop();
				Log.WriteLine($"{ CurrentClassName }.{ Utils.GetCurrentMethodName() }({ url }) in { stopwatch.ElapsedMilliseconds }ms");

				return image;
			}
			catch (Exception ex)
			{
				stopwatch.Stop();
				Log.WriteLine($"{ CurrentClassName }.{ Utils.GetCurrentMethodName() }({ url }):null { ex.ToString() } in { stopwatch.ElapsedMilliseconds }ms");
			}

			imageFormat = ImageFormat.Jpeg;
			return null;
		}

		public static string[] ScrapeImages(string url)
		{
			var images = new List<string>();

			var content = WebRequest(url);
			if (content == null)
				return images.ToArray();

			var currentUrlStart = content.IndexOf("=\"");
			while (currentUrlStart > 0)
			{
				currentUrlStart += "=\"".Length;
				var currentUrlEnd = content.IndexOf("\"", currentUrlStart);
				var currentUrl = content.Substring(currentUrlStart, currentUrlEnd - currentUrlStart);

				if (currentUrl.EndsWith(".jpg") || currentUrl.EndsWith(".png"))
					images.Add(currentUrl);

				currentUrlStart = content.IndexOf("=\"", currentUrlEnd);
			}

			return images.Distinct().ToArray();
		}
	}
}
