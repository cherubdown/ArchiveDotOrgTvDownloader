using System;
using System.Net;
using System.Web;

namespace ArchiveDotOrgTvDownloader
{
    class Program
    {
        // args[0] = url (example https://ia600203.us.archive.org/25/items/WTKR_20160304_040000_NewsChannel_3_News_at_11/WTKR_20160304_040000_NewsChannel_3_News_at_11.mp4?start=0&end=60&ignore=x.mp4)
        // args[1] = optional start time defaults to 0
        // args[2] = optional number of segments to download, defaults to 10 (10 minutes)
        static void Main(string[] args)
        {
            int numSegments = 10; // each segment is 60 seconds
            int segmentSeconds = 60;
            int startSeconds = 0;
            Uri uri = new Uri(args[0]);
            var qs = HttpUtility.ParseQueryString(uri.Query);

            bool interpretStartFromUrl = true;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-start")
                {
                    startSeconds = int.Parse(args[i + 1]);
                    interpretStartFromUrl = false;
                }
                if (args[i] == "-segments")
                {
                    numSegments = int.Parse(args[i + 1]);
                }
            }
            if (interpretStartFromUrl)
            {
                startSeconds = int.Parse(qs["start"]);
            }

            int end = startSeconds / 60 + numSegments;
            for (int i = startSeconds / 60; i < end; i++)
            {
                try
                {
                    qs.Set("start", (i * segmentSeconds).ToString());
                    qs.Set("end", (i * segmentSeconds + segmentSeconds).ToString());
                    var uriBuilder = new UriBuilder(uri);
                    uriBuilder.Query = qs.ToString();
                    var newUri = uriBuilder.Uri;
                    Console.WriteLine($"Downloading {newUri}");
                    new WebClient().DownloadFile(newUri, "download" + (i + 1).ToString() + ".mp4");
                }
                catch( Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.ReadKey();
                }
            }
        }
    }
}
