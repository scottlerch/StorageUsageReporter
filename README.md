StorageUsageReporter
==================

Generate CSV report on contents of file system at specified location.

Usage
-----

Get help:

	> StorageUsageReporter.exe --help
	StorageUsageReporter 1.0.0.0
	Copyright Scott Lerch 2014
	
	  -p, --path        Required. Path to perform recursive analysis.
	
	  -o, --output      (Default: output.csv) Output file to store analysis.
	
	  -s, --sanitize    (Default: False) Randomize identifiable names.
	
	  -r, --rollup      (Default: False) Rollup metadata to directory level.
	
	  --help            Display this help screen.`                                               

Run:

	> StorageUsageReporter.exe --path=C:\
