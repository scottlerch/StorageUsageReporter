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

	DirectoryPath,FileName,FileSize,CreationTimeUtc,LastAccessTimeUtc,LastWriteTimeUtc
	5f5e5109-f31e-4cc5-8ac3-1541f96dd1d7,vvfidzzt.jqj,2063689,2013-08-26T23:58:06.8158449Z,2013-08-26T23:58:06.8158449Z,2006-09-01T18:25:00Z
	5f5e5109-f31e-4cc5-8ac3-1541f96dd1d7,dmco51lr.ye4,504,2013-08-25T03:49:49.9593375Z,2013-08-25T03:49:49.9593375Z,2013-11-18T03:37:37.1705881Z
	5f5e5109-f31e-4cc5-8ac3-1541f96dd1d7,lmmgdei1.jzp,1855603,2013-08-27T00:07:10.8733246Z,2013-08-27T00:07:10.8733246Z,2006-09-01T18:25:00Z
	5f5e5109-f31e-4cc5-8ac3-1541f96dd1d7,0lc32ulm.z2q,517254,2013-08-27T00:18:20.6841834Z,2013-08-27T00:18:20.6841834Z,2006-09-01T18:25:00Z
	5f5e5109-f31e-4cc5-8ac3-1541f96dd1d7,ueafviwg.ycc,120,2013-08-26T22:26:01.6325293Z,2013-08-26T22:26:01.6325293Z,2008-07-09T02:17:53Z
	5f5e5109-f31e-4cc5-8ac3-1541f96dd1d7,qwgm1h0v.elt,1349850,2013-08-27T01:18:16.3281475Z,2013-08-27T01:18:16.3281475Z,2006-09-01T18:25:00Z
	1fc36967-cd5c-4081-8217-d8870c8cf080,ykumvo0c.j40,368,2013-08-26T22:26:43.2705337Z,2013-08-26T22:26:43.2705337Z,2010-09-20T05:24:23Z
	1fc36967-cd5c-4081-8217-d8870c8cf080,znkjxb04.cm2,1577926,2013-08-27T01:18:17.8035685Z,2013-08-27T01:18:17.8035685Z,2010-06-28T23:33:25Z
	1fc36967-cd5c-4081-8217-d8870c8cf080,vlmq4ffk.hhq,2007963,2013-08-27T01:18:19.4202326Z,2013-08-27T01:18:19.4202326Z,2010-06-28T23:33:40Z
	1fc36967-cd5c-4081-8217-d8870c8cf080,lzxlpvmv.3gc,2547798,2013-08-27T01:18:25.8200909Z,2013-08-27T01:18:25.8200909Z,2010-06-28T23:35:43Z
	1fc36967-cd5c-4081-8217-d8870c8cf080,elyet4hk.rbz,2459945,2013-08-27T01:18:31.0220682Z,2013-08-27T01:18:31.0220682Z,2010-06-28T23:35:49Z
	466afdd6-d976-4ace-8e6f-4c904adfe931,3wunxddf.t2j,67,2013-08-26T22:26:43.0031203Z,2013-08-26T22:26:43.0031203Z,2009-08-29T20:29:44Z
	5d3ef560-a307-4fd5-a0f5-4adca1f455cd,q5a02xvh.zam,20,2013-08-26T22:26:42.8656881Z,2013-08-26T22:26:42.8656881Z,2009-08-29T20:29:44Z
	97278577-f403-44ac-b2e8-c0f4f0192155,epx0xdzd.vee,55730,2013-08-26T22:26:42.7062479Z,2013-08-26T22:26:42.7062479Z,2010-07-17T23:43:32Z
	97278577-f403-44ac-b2e8-c0f4f0192155,xazxghxw.05k,7367029,2013-08-27T02:29:24.6228195Z,2013-08-27T02:29:24.6228195Z,2008-07-23T18:47:56Z
	97278577-f403-44ac-b2e8-c0f4f0192155,rjvmagal.hzn,7428599,2013-08-27T02:29:28.560483Z,2013-08-27T02:29:28.560483Z,2008-07-23T18:48:10Z
