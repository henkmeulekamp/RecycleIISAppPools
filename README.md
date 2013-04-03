RecycleIISAppPools
==================

Simple Command line app to recycle IIS application pools by name.  
  
Command line .net 4 application which will list all IIS applications pools. If filter is given as argument it recycles
application pools where name matches filter.  

## Recycle command line arguments ##

Recycle [/?]  [/f]  [/s]  [/u]  [/p] [/w]  

[/?]  Show Help  
[/f]  App Pool name filters, use keyword separated with spaces, Wildcard = *  
[/s]  Specifies remote server name, leave empty for localhost  
[/u]  Specifies username for server, leave empty when using current account  
[/p]  Specifies password for server, leave empty when using current account 
[/w]  Dont wait just before closing app, if not used it waits for key press or 3 seconds   
  
Notes on filters  
- Filters are case insensitive  
- \* Is wildcard (\*name\* or name\*)  
- Multiple filters can be used as argument, separated by space (/f:name1 name2\* name3)  
 
Useful for auto deploy scripts, sometimes deploying by just copying files into a IIS hosted application fails
due file or folder locking. Recycling the app pools for that web application seems to help. I wrote this app to
be able to quickly recycle certain applications within deploy script which will deploy new files.

## Examples ##
  
List app pools on current server 
- recycle
  
List app pools on server named webapp001 with current account  
- recycle /s:webapp001
  
List app pools on server named webapp001 with given account  
- recycle /s:webapp001 /u:myadminname /p:mypwd
  
Recycle app pools named webapp1 and webapp2  
- recycle /f:webapp1 webapp2
  
Recycle all app pools on server webapp001 with the word product in the name  
- recycle /s:webapp001 /f:\*product\*
  
Show help  
- recycle /?

## Requires ##

- .NET 4.0  
    - *I'm pretty sure a 2.0 and 3.5 version can be made by compiling it against those frameworks.*

## Questions ##

Ask: henk@meulekamp.net

## OSS Libraries used ##
The following libraries are used. Each library is released under its respective licence:

[Command Line parser, Ron Jacobs](http://code.msdn.microsoft.com/Command-Line-Parser-Library-a8ba828a)
