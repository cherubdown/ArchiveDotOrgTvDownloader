using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace ArchiveDotOrgTvDownloader
{
    class Program
    {
        // args[0] = url (example https://ia800506.us.archive.org/8/items/CSPAN2_20150525_220000_Book_Discussion_on_Preparing_for_Contact/CSPAN2_20150525_220000_Book_Discussion_on_Preparing_for_Contact.mp4?start=120&end=300&ignore=x.mp4)
        // args[1] = optional start time defaults to 0
        // args[2] = optional number of segments to download, defaults to 10 (10 minutes)
        static void Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = 30;
            int numSegments = 10; // each segment is 60 seconds
            int segmentSeconds = 180;
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

            int end = startSeconds / segmentSeconds + numSegments;
            List<Task> tasks = new List<Task>();
            for (int i = startSeconds / segmentSeconds; i < end; i++)
            {
                int index = i;
                var task = new Task(() => Download(uri.ToString(), index, segmentSeconds));
                tasks.Add(task);
                task.Start();
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Downloads complete!");
            Console.ReadKey();
        }

        private static void Download(string url, int i, int segmentSeconds)
        {
            try
            {
                Uri uri = new Uri(url);
                var qs = HttpUtility.ParseQueryString(uri.Query);
                string begin = (i * segmentSeconds).ToString();
                string end = (i * segmentSeconds + segmentSeconds).ToString();
                qs.Set("start", begin);
                qs.Set("end", end);
                var uriBuilder = new UriBuilder(uri);
                uriBuilder.Query = qs.ToString();
                Console.WriteLine($"Downloading {uriBuilder.Uri}");
                var fileName = "download_" + i + "_" + begin + "-" + end + ".mp4";
                new WebClient().DownloadFile(uriBuilder.Uri, fileName);
                Console.WriteLine($"Download Complete for {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }
    }
}
