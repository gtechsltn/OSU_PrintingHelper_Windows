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
        private event EventHandler _handler;
        public JsonDownloader(Uri url, EventHandler handler)
        {
            this._handler = handler;
            WebClient downloader = new WebClient();
            downloader.DownloadStringCompleted += new DownloadStringCompletedEventHandler(OnDownloadStringCompleted);
            downloader.DownloadStringAsync(url);
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
            _handler(JsonConvert.DeserializeObject<T>(info), e);
        }
    }
}
