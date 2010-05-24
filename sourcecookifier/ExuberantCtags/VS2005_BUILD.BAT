REM * Assuming we got VS2005 installed *
CALL "%VS80COMNTOOLS%vsvars32.bat"

CL.EXE /O2 /Fectags.exe -DWIN32 -DHAVE_REGCOMP -D__USE_GNU -Dbool=int -Dfalse=0 -Dtrue=1 -Dstrcasecmp=stricmp -I. -Ignu_regex args.c ant.c asm.c asp.c awk.c basic.c beta.c c.c cobol.c dosbatch.c eiffel.c entry.c erlang.c flex.c fortran.c get.c html.c jscript.c keyword.c lisp.c lregex.c lua.c main.c make.c matlab.c ocaml.c options.c parse.c pascal.c perl.c php.c python.c read.c rexx.c routines.c ruby.c scheme.c sh.c slang.c sml.c sort.c sql.c strlist.c tcl.c tex.c verilog.c vhdl.c vim.c yacc.c vstring.c gnu_regex/regex.c /link setargv.obj
COPY ctags.exe ..\SourceCookifier\Setup\SourceCookifier\ctags.exe

DEL *.obj
PAUSE