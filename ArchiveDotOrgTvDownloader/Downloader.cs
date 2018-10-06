using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveDotOrgTvDownloader
{
    public class Downloader
    {
        Uri uri;
        int numMinutes = 10;
        int segmentSeconds = 60;
        int startSeconds = 0;

        /// <summary>
        /// wip to object orient this project
        /// </summary>
        /// <param name="url"></param>
        public Downloader(string url)
        {
            uri = new Uri(url);
        }
    }
}
