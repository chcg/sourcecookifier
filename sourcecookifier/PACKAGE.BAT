SET VERSION=v0.7.3

SET ZIP="C:\Program Files\7-Zip\7z.exe"

SET TARGET=SourceCookifier.%VERSION%.bin
SET TRACEBUILD=SourceCookifier.%VERSION%.DEBUG_TRACE_BUILD

MD %TARGET%
MD %TARGET%\Config
MD %TARGET%\doc
MD %TARGET%\SourceCookifier
MD %TARGET%\SourceCookifier\icons
COPY ".\SourceCookifier\Setup\SourceCookifier\icons" "%TARGET%\SourceCookifier\icons\"
COPY ".\SourceCookifier\Setup\SourceCookifier\ctags.exe" "%TARGET%\SourceCookifier\"
COPY ".\SourceCookifier\Setup\SourceCookifier\AddPolicy.bat" "%TARGET%\SourceCookifier\"
COPY ".\SourceCookifier\Setup\SourceCookifier\RemovePolicy.bat" "%TARGET%\SourceCookifier\"
COPY ".\SourceCookifier\Setup\Config\SourceCookifier.languages.model.xml" "%TARGET%\Config\"
COPY "..\Notepad++\plugins\SourceCookifier.dll" "%TARGET%"
COPY "README.TXT" "%TARGET%\doc\SourceCookifier.README.TXT"

MD %TRACEBUILD%
COPY "..\Notepad++\plugins\SourceCookifier\SourceCookifierD.dll" "%TRACEBUILD%\SourceCookifier.dll"
COPY "TRACEME.TXT" "%TRACEBUILD%"

CD %TARGET%
%ZIP% a -mx9 ..\%TARGET%.7z *
%ZIP% a -mx9 ..\%TARGET%.zip *

CD ..\%TRACEBUILD%
%ZIP% a -mx9 ..\%TRACEBUILD%.zip *
CD..