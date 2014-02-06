StorageUsageReporter
====================

Generate usage statistics at specified storage location and output a detailed CSV file containing file metadata.

Usage
-----

Get help:

	> StorageUsageReporter.exe --help
	StorageUsageReporter 1.0.0.0
	Copyright Scott Lerch 2014
	
	  -p, --paths       Required. Paths to perform recursive analysis.
	
	  -o, --output      (Default: output.csv) Output file to store analysis.
	
	  -s, --sanitize    (Default: False) Randomize identifiable names.
	
	  -r, --rollup      (Default: False) Rollup metadata to directory level.
	
	  --help            Display this help screen.
	  
Local drive:

	> StorageUsageReporter.exe -p C:\
	
	Starting at 2/4/2014 11:15:14 PM...
	Completed in 00:00:30.8677660!
	
	Location analyzed: C:\
	Detailed output: output.csv
	
	Number: 492885  Size: 137.9 GB

Local drive with rollup:

	> StorageUsageReporter.exe -p C:\ -r
	
	Starting at 2/4/2014 11:12:40 PM...
	Completed in 00:01:20.2719613!
	
	Location analyzed: C:\
	Detailed output: output.csv
	
	Number: 64029  Size: 137.8 GB

File share:

	> StorageUsageReporter.exe -path \\filesrv1\Documents

	Starting at 2/4/2014 11:08:28 PM...
	Completed in 00:00:09.8168705!

	Location analyzed: \\filesrv1\Documents
	Detailed output: output.csv

	Number: 71671  Size: 8.4 GB

File shares with sanitized output:

	> StorageUsageReporter.exe -p \\filesrv1\Documents \\filesrv1\Pictures -s

	Starting at 2/4/2014 11:11:07 PM...
	Completed in 00:00:09.9287875!

	Location analyzed: \\filesrv1\Documents;\\filesrv1\Pictures
	Detailed output: output.csv

	Number: 76739  Size: 18.5 GB

Sample Output
-------------

Sample output.csv file with sanitized names:

    DirectoryPath,FileName,FileExtension,FileSize,CreationTimeUtc,LastAccessTimeUtc,LastWriteTimeUtc
    830A41F4,830DDBDE,gitconfig,57,2013-08-25T05:37:24.146531Z,2013-08-25T05:37:24.1645425Z,2013-08-25T05:37:24.1655432Z
    830A41F4,F7D86809,octave_hist,588,2014-01-23T05:07:39.08688Z,2014-01-23T05:07:39.08688Z,2014-01-23T05:07:39.08688Z
    830A41F4,ABA88AB9,mdf,3211264,2013-11-16T04:14:49.9477779Z,2013-11-16T04:14:49.9477779Z,2014-01-19T00:43:17.0684181Z
    830A41F4,E87C7477,ldf,802816,2013-11-16T04:14:49.9661037Z,2013-11-16T04:14:49.9661037Z,2014-01-19T00:43:17.069427Z
    830A41F4,43492A74,DAT,12845056,2013-10-21T02:29:10.6986923Z,2014-01-19T00:43:40.7219919Z,2014-01-19T00:43:40.7219919Z
    830A41F4,E0068FBB,LOG1,12201984,2013-10-21T02:29:10.7768018Z,2013-10-21T02:29:10.7768018Z,2013-10-21T02:29:10.7768018Z
    830A41F4,E0068FBE,LOG2,4657152,2013-10-21T02:29:10.7768018Z,2013-10-21T02:29:10.7768018Z,2013-10-21T02:29:10.7768018Z
    830A41F4,415CC0FD,blf,65536,2013-10-21T02:29:10.7768018Z,2013-10-21T02:29:10.7768018Z,2013-10-21T02:29:10.8705827Z
    830A41F4,AE594576,regtrans-ms,524288,2013-10-21T02:29:10.7768018Z,2013-10-21T02:29:10.7768018Z,2013-10-21T02:29:10.8705827Z
    830A41F4,E6C98C51,regtrans-ms,524288,2013-10-21T02:29:10.7768018Z,2013-10-21T02:29:10.7768018Z,2013-10-21T02:29:10.8705827Z
    830A41F4,422E1686,blf,65536,2013-10-25T15:55:40.9964471Z,2013-10-25T15:55:40.9964471Z,2013-11-02T05:53:05.8839978Z
    830A41F4,B4B49EED,regtrans-ms,524288,2013-10-25T15:55:40.9964471Z,2013-10-25T15:55:40.9964471Z,2013-11-02T05:53:05.8839978Z
    830A41F4,949980C8,regtrans-ms,524288,2013-10-25T15:55:41.0120719Z,2013-10-25T15:55:41.0120719Z,2013-11-02T05:53:05.8839978Z
    830A41F4,E8E6E2CB,ini,20,2013-10-23T16:01:17.6752736Z,2013-10-23T16:01:17.6752736Z,2013-10-23T16:01:17.6752736Z
    8C8B45E0,3C3CB9F1,zip,79304187,2013-08-25T23:26:54.6915246Z,2013-08-25T23:26:54.6915246Z,2008-11-23T00:39:29Z
    8C8B45E0,57987C48,m4v,260902030,2013-08-25T23:28:03.9472248Z,2013-08-25T23:28:03.9472248Z,2011-02-21T02:55:08Z
    8C8B45E0,FFE0EF6A,ini,504,2013-08-25T03:49:49.9593375Z,2013-08-25T03:49:49.9593375Z,2013-11-18T03:37:37.1705881Z


Acknowledgements
----------------

AlphaFS - Peter Palotas:  
http://alphafs.codeplex.com/

Command Line Parser - Giacomo Stelluti Scala:  
https://github.com/gsscoder/commandline

ILRepack - Francois Valdy:  
https://github.com/gluck/il-repack
