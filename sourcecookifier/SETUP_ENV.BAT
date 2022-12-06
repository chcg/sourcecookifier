@ECHO OFF

SET CTAGS_DEBUG=0
SET FIND_IN_SESSION=0

IF EXIST "C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Auxiliary\Build\vcvarsall.bat" GOTO VS2022

ECHO No full build environment found!
EXIT 1

:VS2022
SET BUILD_ENV_NET=VS2022
SET BUILD_ENV_C=VC
CALL "C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Auxiliary\Build\vcvarsall.bat" x64
GOTO FINISH

:FINISH
SET SETUP_ENV_DONE=1
