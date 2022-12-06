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

COPY ctags.exe "..\SourceCookifier\Setup\bin\ctags.exe"
MD "..\..\Notepad++\plugins\SourceCookifier\bin"
COPY ctags.exe "..\..\Notepad++\plugins\SourceCookifier\bin\ctags.exe"
