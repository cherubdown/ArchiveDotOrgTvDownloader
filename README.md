ArchiveDotOrgTvDownloader

This downloads videos directly from https://archive.org, and puts the videos in sequence.

Usage:

Create a bat file like so:

set url="https://ia801407.us.archive.org/34/items/FOXNEWSW_20190704_080000_Fox_and_Friends_First/FOXNEWSW_20190704_080000_Fox_and_Friends_First.mp4?start=3480&end=3540&ignore=x.mp4"
ArchiveDotOrgTvDownloader.exe %url% -segments 2

where url is the mp4 link found on archive, and segments is the number of 3-minute segments to download thereafter.
