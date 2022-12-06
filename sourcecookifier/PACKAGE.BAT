@ECHO OFF
SET VERSION=v0.10.0

SET ZIP="C:\Program Files\7-Zip\7z.exe"

SET TARGET=SourceCookifier.%VERSION%.x64.bin

MD %TARGET%
MD %TARGET%\bin
MD %TARGET%\icons
MD %TARGET%\setup
COPY ".\SourceCookifier\Setup\bin\ctags.exe" "%TARGET%\bin\"
COPY ".\SourceCookifier\Setup\icons" "%TARGET%\icons\"
COPY ".\SourceCookifier\Setup\setup\AddPolicy.bat" "%TARGET%\setup\"
COPY ".\SourceCookifier\Setup\setup\RemovePolicy.bat" "%TARGET%\setup\"
COPY ".\SourceCookifier\Setup\setup\SourceCookifier.languages.model.xml" "%TARGET%\setup\"
COPY "..\Notepad++\plugins\SourceCookifier\SourceCookifier.dll" "%TARGET%\"
COPY "..\Notepad++\plugins\SourceCookifier\bin\SourceCookifier.Debug.Build.dll" "%TARGET%\bin\"
COPY "README.TXT" "%TARGET%\setup\"
COPY "DEBUGGING.TXT" "%TARGET%\setup\"

CD %TARGET%
%ZIP% a -mx9 ..\%TARGET%.zip *
CD..

certutil -hashfile %TARGET%.zip SHA256
