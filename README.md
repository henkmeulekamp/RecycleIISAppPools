RecycleIISAppPools
==================

Simple Command line app to recycle IIS application pools by name.  
  
Command line .net 4 application which will list all IIS applications pools. If filter is given as argument it recycles
application pools where name matches filter.  
  
- Filter is case insensitive
- * is wildcard (recycle *name* or recycle name*)
- multiple filters can be used as argument, separated by space (recycle name1 name2* name3)
  
Useful for auto deploy scripts, sometimes deploying by just copying files into a IIS hosted application fails
due file or folder locking. Recycling the app pools for that web application seems to help. I wrote this app to
be able to quickly recycle certain applications within deploy script which will deploy new files.

