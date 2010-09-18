CALL "%VS90COMNTOOLS%vsvars32.bat"
CD ExuberantCtags
CALL VS_CL_CTAGS.BAT
CD ..
MSBUILD /Property:Configuration=Release SourceCookifier_SD.sln
VCBUILD /upgrade SourceCookifierLoader\SourceCookifierLoader.vcproj
VCBUILD /upgrade /overrideRefVer SourceCookifierWrapper\SourceCookifierWrapper.vcproj
MSBUILD /Property:Configuration=Release SourceCookifierLoader_VS2008.sln
CALL PACKAGE.BAT
PAUSE
