using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Utility
{
    public class JsonDownloader<T>
    {
        private event EventHandler Handler;
        public JsonDownloader(string url, EventHandler Handler)
        {
            this.Handler = Handler;
            WebClient downloader = new WebClient();
            downloader.DownloadStringCompleted += new DownloadStringCompletedEventHandler(OnDownloadStringCompleted);
            downloader.DownloadStringAsync(new Uri(url));
        }
        private void OnDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            FireHandler_PrinterInformation(e.Result, e);
        }
        private void FireHandler_PrinterInformation(string info, EventArgs e)
        {
            Handler(JsonConvert.DeserializeObject<T>(info), e);
        }
    }
}
