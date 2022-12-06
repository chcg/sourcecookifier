@ECHO OFF

IF "%SETUP_ENV_DONE%" == "" CALL ..\SETUP_ENV.BAT

IF "%FIND_IN_SESSION%" == "1" (
  MSBUILD /target:Rebuild /Property:Configuration=Debug;DefineConstants="TRACE_ALL;FIND_IN_SESSION" SourceCookifier_%BUILD_ENV_NET%.sln
  MOVE /Y ..\..\Notepad++\plugins\SourceCookifier\SourceCookifier.dll ..\..\Notepad++\plugins\SourceCookifier\bin\SourceCookifier.Debug.Build.dll
  MSBUILD /target:Rebuild /Property:Configuration=Release;DefineConstants=FIND_IN_SESSION;PostBuildEvent="" SourceCookifier_%BUILD_ENV_NET%.sln
) ELSE (
  MSBUILD /target:Rebuild /Property:Configuration=Debug;DefineConstants=TRACE_ALL SourceCookifier_%BUILD_ENV_NET%.sln
  MOVE /Y ..\..\Notepad++\plugins\SourceCookifier\SourceCookifier.dll ..\..\Notepad++\plugins\SourceCookifier\bin\SourceCookifier.Debug.Build.dll
  MSBUILD /target:Rebuild /Property:Configuration=Release;PostBuildEvent="" SourceCookifier_%BUILD_ENV_NET%.sln
)
