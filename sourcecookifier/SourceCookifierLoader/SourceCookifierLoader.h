#include <windows.h>
#include <shlwapi.h>
#pragma comment(lib, "shlwapi.lib")
#include <fstream>
#include <string>
#include "resource.h"
#include "NppIncludes\PluginInterface.h"

// Don't enable following functionality, since it won't work with your Notepad++ anyway.
// I'm using a modded N++ version, because it didn't expose a function for plug-ins, which
// searches a given list of files for a text string...
// #define FIND_IN_SESSION

HMODULE hMod = NULL;

TCHAR szPluginPath[MAX_PATH];
TCHAR szPluginName[MAX_PATH];
TCHAR szPluginConfig[MAX_PATH];
TCHAR szBuffer[MAX_PATH];

NppData nppData;
#ifdef FIND_IN_SESSION
const int nbFunc = 12;
#else
const int nbFunc = 10;
#endif
FuncItem funcItem[nbFunc];
void setCommand(size_t index, TCHAR *cmdName, PFUNCPLUGINCMD pFunc, bool check0nInit, UCHAR key, bool ctrl, bool shift, bool alt);

toolbarIcons tbIcons;

FuncItem * funcItemCore = NULL;

BOOL pluginWrapperLoaded = FALSE;
BOOL GetPluginWrapper(BOOL showError);

void LoadConfigs();
BOOL ShowAtStartup = FALSE;
BOOL HideAtStartup = FALSE;
BOOL FormShown = FALSE;

void (__cdecl * W_Init)() = NULL;
void (__cdecl * W_setInfo)(NppData) = NULL;
FuncItem * (__cdecl * W_getFuncsArray)(int *) = NULL;
LRESULT (__cdecl * W_messageProc)(UINT, WPARAM, LPARAM) = NULL;
void (__cdecl * W_beNotified)(SCNotification *) = NULL;

void ShowFrmMain(); PFUNCPLUGINCMD CORE_ShowFrmMain = NULL;
void GoToDefinition(); PFUNCPLUGINCMD CORE_GoToDefinition = NULL;
void NavigateBackward(); PFUNCPLUGINCMD CORE_NavigateBackward = NULL;
void NavigateForward(); PFUNCPLUGINCMD CORE_NavigateForward = NULL;
void FindInSession(); PFUNCPLUGINCMD CORE_FindInSession = NULL;
void ShowSettings(); PFUNCPLUGINCMD CORE_ShowSettings = NULL;
void ShowOptions(); PFUNCPLUGINCMD CORE_ShowOptions = NULL;
void ShowHelp(); PFUNCPLUGINCMD CORE_ShowHelp = NULL;
