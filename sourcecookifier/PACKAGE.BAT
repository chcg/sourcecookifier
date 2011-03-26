SET VERSION=v0.6.1
SET TARGET=SourceCookifier.%VERSION%.bin
SET TRACEBUILD=SourceCookifier.%VERSION%.DEBUG_TRACE_BUILD
MD %TARGET%
MD %TARGET%\Config
MD %TARGET%\SourceCookifier
MD %TARGET%\SourceCookifier\icons
MD %TRACEBUILD%
COPY "..\Notepad++\plugins\SourceCookifier\icons" "%TARGET%\SourceCookifier\icons\"
COPY "..\Notepad++\plugins\SourceCookifier\ctags.exe" "%TARGET%\SourceCookifier\"
COPY "..\Notepad++\plugins\SourceCookifier\SourceCookifierCore.dll" "%TARGET%\SourceCookifier\"
COPY "..\Notepad++\plugins\SourceCookifier\SourceCookifierWrapper.dll" "%TARGET%\SourceCookifier\"
COPY "..\Notepad++\plugins\SourceCookifier\AddPolicy.bat" "%TARGET%\SourceCookifier\"
COPY "..\Notepad++\plugins\SourceCookifier\RemovePolicy.bat" "%TARGET%\SourceCookifier\"
COPY "..\Notepad++\plugins\Config\SourceCookifier.languages.model.xml" "%TARGET%\Config\"
COPY "..\Notepad++\plugins\SourceCookifier.dll" "%TARGET%"
COPY "SourceCookifier.README.TXT" "%TARGET%"
COPY "..\Notepad++\plugins\SourceCookifier\SourceCookifierCoreD.dll" "%TRACEBUILD%\SourceCookifierCore.dll"
COPY "TRACEME.TXT" "%TRACEBUILD%"
CD %TARGET%
"C:\Program Files\7-Zip\7z.exe" a -mx9 ..\%TARGET%.7z *
"C:\Program Files\7-Zip\7z.exe" a -mx9 ..\%TARGET%.zip *
CD ..\%TRACEBUILD%
"C:\Program Files\7-Zip\7z.exe" a -mx9 ..\%TRACEBUILD%.zip *
CD..