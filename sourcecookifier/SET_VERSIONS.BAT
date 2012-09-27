@ECHO OFF

SET MAJORVER=0
SET MINORVER=7
SET BUILDNUM=3
SET REVISION=0
SET CTAGSVER=5.8.7

REPLEX ".\ExuberantCtags\ctags.h" -e 1 ^
-s "PROGRAM_VERSION "[0-9]+[.][0-9]+[.][0-9]+[.]SC"" ^
-r "PROGRAM_VERSION "%CTAGSVER%.SC""
REPLEX ".\ExuberantCtags\ctags.h" -e 1 ^
-s "SOURCECOOKIFER_VERSION "[0-9]+[.][0-9]+[.][0-9]+[.][0-9]+"" ^
-r "SOURCECOOKIFER_VERSION "%MAJORVER%.%MINORVER%.%BUILDNUM%.%REVISION%""
REPLEX ".\SourceCookifier\CTagsExe.cs" -e 1 ^
-s "static string cTagsVersion = "[0-9]+[.][0-9]+[.][0-9]+[.]SC"" ^
-r "static string cTagsVersion = "%CTAGSVER%.SC""

REPLEX ".\SourceCookifier\Properties\AssemblyInfo.cs" -e 2 ^
-s "Version\("[0-9]+[.][0-9]+[.][0-9]+[.][0-9]+"\)\]" ^
-r "Version("%MAJORVER%.%MINORVER%.%BUILDNUM%.%REVISION%")]"

REPLEX ".\SourceCookifier\Properties\help.rtf" -e 1 ^
-s "v\}\{\\rtlch\\fcs1 \\af0\\afs28 \\ltrch\\fcs0 \\b\\f0\\fs28\\cf19\\insrsid14427415\\charrsid8879124 [0-9]+" ^
-r "v}{\rtlch\fcs1 \af0\afs28 \ltrch\fcs0 \b\f0\fs28\cf19\insrsid14427415\charrsid8879124 %MAJORVER%"
REPLEX ".\SourceCookifier\Properties\help.rtf" -e 1 ^
-s "\\b\\f0\\fs28\\cf21\\insrsid1335067 [0-9]+" ^
-r "\b\f0\fs28\cf21\insrsid1335067 %MINORVER%"
REPLEX ".\SourceCookifier\Properties\help.rtf" -e 1 ^
-s "\\b\\f0\\fs28\\cf26\\insrsid474794 [0-9]+" ^
-r "\b\f0\fs28\cf26\insrsid474794 %BUILDNUM%"
REPLEX ".\SourceCookifier\Properties\help.rtf" -e 1 ^
-s "\\b\\f0\\fs28\\cf14\\insrsid7933416\\charrsid7933416 [0-9]+" ^
-r "\b\f0\fs28\cf14\insrsid7933416\charrsid7933416 %REVISION%"

REPLEX ".\PACKAGE.BAT" -e 1 ^
-s "SET VERSION=v[0-9]+[.][0-9]+[.][0-9]+" ^
-r "SET VERSION=v%MAJORVER%.%MINORVER%.%BUILDNUM%"

PAUSE