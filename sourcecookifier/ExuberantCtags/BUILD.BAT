@ECHO OFF

IF "%SETUP_ENV_DONE%" == "" CALL ..\SETUP_ENV.BAT

SET TARGETS=args.c ant.c asm.c asp.c awk.c basic.c beta.c c.c cobol.c css.c dosbatch.c eiffel.c entry.c erlang.c fgl.c flex.c fortran.c get.c html.c jscript.c keyword.c lisp.c lregex.c lua.c main.c make.c matlab.c objc.c ocaml.c options.c parse.c pascal.c perl.c php.c python.c read.c rexx.c routines.c ruby.c scheme.c sh.c slang.c sml.c sort.c sql.c strlist.c tcl.c tex.c verilog.c vhdl.c vim.c yacc.c vstring.c gnu_regex/regex.c
SET COMMON_OPTS=-DWIN32 -DHAVE_REGCOMP -D__USE_GNU -Dbool=int -Dfalse=0 -Dtrue=1 -Dstrcasecmp=stricmp -I. -Ignu_regex

REM cl (Microsoft)
IF "%BUILD_ENV_C%" == "VC" (
  IF "%CTAGS_DEBUG%" == "1" (
    CL.EXE /Zi /Fectags.exe -DDEBUG %COMMON_OPTS% debug.c %TARGETS% /link setargv.obj
  ) ELSE (
    CL.EXE /O2 /Fectags.exe %COMMON_OPTS% %TARGETS% /link setargv.obj
  )
  DEL *.obj
)

REM gcc (Cygwin):
IF "%BUILD_ENV_C%" == "CYGWIN" (
  IF "%CTAGS_DEBUG%" == "1" (
    bash -c "gcc -O4 -o ctags.exe -save-temps -Wall -DDEBUG -mno-cygwin -DHAVE_FTRUNCATE %COMMON_OPTS% debug.c %TARGETS%"
  ) ELSE (
    bash -c "gcc -O4 -o ctags.exe -save-temps -Wall -mno-cygwin -DHAVE_FTRUNCATE %COMMON_OPTS% %TARGETS%"
  )
  rm *.i *.o *.s
)

REM gcc (MinGW):
IF "%BUILD_ENV_C%" == "MINGW" (
  IF "%CTAGS_DEBUG%" == "1" (
    sh -c "gcc -O4 -o ctags.exe -save-temps -Wall -DDEBUG %COMMON_OPTS% debug.c %TARGETS%"
  ) ELSE (
    sh -c "gcc -O4 -o ctags.exe -save-temps -Wall %COMMON_OPTS% %TARGETS%"
  )
  rm *.i *.o *.s
)

COPY ctags.exe "..\SourceCookifier\Setup\SourceCookifier\ctags.exe"
COPY ctags.exe "..\..\Notepad++\plugins\SourceCookifier\ctags.exe"
