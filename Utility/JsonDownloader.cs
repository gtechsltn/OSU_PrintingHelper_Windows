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
        public JsonDownloader(EventHandler Handler)
        {
            this.Handler = Handler;
            WebClient downloader = new WebClient();
            downloader.DownloadStringCompleted += new DownloadStringCompletedEventHandler(OnDownloadStringCompleted);
            downloader.DownloadStringAsync(new Uri(ConstFields.PRINTER_LIST_URL));
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
