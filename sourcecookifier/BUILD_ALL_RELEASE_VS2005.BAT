CALL "%VS80COMNTOOLS%vsvars32.bat"
CD ExuberantCtags
CALL VS_CL_CTAGS.BAT
CD ..
MSBUILD /Property:Configuration=Release SourceCookifier_SD.sln
MSBUILD /Property:Configuration=Release SourceCookifierLoader_VS2005.sln
CALL PACKAGE.BAT
PAUSE
