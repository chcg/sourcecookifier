@ECHO OFF

SET CTAGS_DEBUG=0
SET FIND_IN_SESSION=0

IF NOT "%VS100COMNTOOLS%" == "" GOTO VS2010
IF NOT "%VS90COMNTOOLS%" == "" GOTO VS2008
IF NOT "%VS80COMNTOOLS%" == "" GOTO VS2005
SET CYGWIN_BIN=C:\cygwin\bin
IF EXIST %CYGWIN_BIN%\bash.exe GOTO CYGWIN
SET MINGW_MSYS_BIN=C:\MinGW\msys\1.0\bin
IF EXIST %MINGW_MSYS_BIN%\sh.exe GOTO MINGW

:CheckSharpDevelop
SET SharpDevelopBinPath=C:\Program Files (x86)\SharpDevelop\4.0\bin
SET DotNetFrameworkPath=C:\Windows\Microsoft.NET\Framework\v4.0.30319
IF EXIST "%SharpDevelopBinPath%\SharpDevelop.exe" GOTO SharpDevelop

ECHO No full build environment found!
EXIT 1

:VS2010
SET BUILD_ENV_NET=VS2010
SET BUILD_ENV_C=VC
CALL "%VS100COMNTOOLS%..\..\VC\vcvarsall.bat" x86
GOTO FINISH
:VS2008
SET BUILD_ENV_NET=VS2008
SET BUILD_ENV_C=VC
CALL "%VS90COMNTOOLS%..\..\VC\vcvarsall.bat" x86
GOTO FINISH
:VS2005
SET BUILD_ENV_NET=VS2005
SET BUILD_ENV_C=VC
CALL "%VS80COMNTOOLS%..\..\VC\vcvarsall.bat" x86
GOTO FINISH
:CYGWIN
SET BUILD_ENV_C=CYGWIN
SET PATH=%CYGWIN_BIN%;%PATH%
ECHO Setting environment for using Cygwin tools.
GOTO CheckSharpDevelop
:MINGW
SET BUILD_ENV_C=MINGW
SET MINGW_BIN=C:\MinGW\bin
SET PATH=%MINGW_BIN%;%MINGW_MSYS_BIN%;%PATH%
ECHO Setting environment for using MinGW tools.
GOTO CheckSharpDevelop
:SharpDevelop
SET BUILD_ENV_NET=SD
ECHO Setting environment for using SharpDevelop tools.
PATH=%DotNetFrameworkPath%;%PATH%

:FINISH
SET SETUP_ENV_DONE=1
