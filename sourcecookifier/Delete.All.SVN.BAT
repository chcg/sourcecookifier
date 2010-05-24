cmd /c "TITLE Removing SVN Folders in %~dp0 && COLOR 4F && FOR /r "%~dp0" %%f IN (.svn) DO RD /s /q "%%f" "
