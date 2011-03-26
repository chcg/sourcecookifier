CALL "%VS80COMNTOOLS%vsvars32.bat"

CD ExuberantCtags
CALL VS_CL_CTAGS.BAT
CD ..

MSBUILD /target:Rebuild /Property:Configuration=Debug;DefineConstants=TRACE_ALL;PostBuildEvent="" SourceCookifier_VS2005.sln
CD ..\Notepad++\plugins\SourceCookifier
RENAME SourceCookifierCore.dll SourceCookifierCoreD.dll
CD ..\..\..\sourcecookifier

MSBUILD /target:Rebuild /Property:Configuration=Release SourceCookifier_VS2005.sln

MSBUILD /target:Rebuild /Property:Configuration=Release SourceCookifierLoader_VS2005.sln

PAUSE
