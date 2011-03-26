#include "SourceCookifierLoader.h"

BOOL APIENTRY DllMain(HMODULE hModule, DWORD reason, LPVOID lpReserved)
{
	if (reason == DLL_PROCESS_ATTACH) {
		hMod = hModule;
		LPWSTR pFilename;
		DWORD len = GetModuleFileName(hModule, szPluginPath, MAX_PATH);
		if (len) {
			pFilename = PathFindFileName(szPluginPath);
			PathRemoveExtension(szPluginPath);
			lstrcpy(szPluginName, pFilename);
			wsprintf(szBuffer, L"\\%sWrapper.dll", pFilename);
			lstrcat(szPluginPath, szBuffer);
		}
	}
	else if (reason == DLL_PROCESS_DETACH) {
		for (int i = 0; i < nbFunc; i++) {
			if (funcItem[i]._pShKey) {
				delete funcItem[i]._pShKey;
			}
		}
		if (tbIcons.hToolbarBmp) {
			DeleteObject(tbIcons.hToolbarBmp);
		}
	}
    return TRUE;
}

BOOL GetPluginWrapper()
{
	if (pluginWrapperLoaded) {
		return TRUE;
	}

	if ((GetVersion() & 0xFF) < 6) {
		// OS earlier than Vista
		BOOL bNetInstalled = TRUE;
		HKEY hKey = NULL;
		LONG result = RegOpenKeyEx(HKEY_LOCAL_MACHINE, L"SOFTWARE\\Microsoft\\.NETFramework\\Policy\\v2.0", 0, KEY_READ, &hKey);
		if (result == ERROR_FILE_NOT_FOUND) {
			bNetInstalled = FALSE;
		} else if (result == ERROR_ACCESS_DENIED) {
			TCHAR szNetDir[MAX_PATH];
			ExpandEnvironmentStrings(L"%SYSTEMROOT%\\Microsoft.NET\\Framework\\v2.0.50727", szNetDir, sizeof szNetDir);
			if (GetFileAttributes(szNetDir) == -1) {
				bNetInstalled = FALSE;
			}
		} else {
			RegCloseKey(hKey);
		}
		if (!bNetInstalled) {
			MessageBox(NULL, L"Please install the .NET Runtime 2.0 in order to use this plugin", L"SourceCookifier", MB_OK);
			return FALSE;
		}
	}

	HMODULE hPluginWrapper = LoadLibrary(szPluginPath);
	if (!hPluginWrapper) {
		MessageBox(NULL, L"Failed loading SourceCookifierWrapper.dll", L"SourceCookifier", MB_OK);
		return FALSE;
	}

	W_Init = (void(__cdecl *)()) GetProcAddress(hPluginWrapper, "Init");
	W_setInfo = (void(__cdecl *)(NppData)) GetProcAddress(hPluginWrapper, "setInfo");
	W_getFuncsArray = (FuncItem *(__cdecl *)(int *)) GetProcAddress(hPluginWrapper, "getFuncsArray");
	W_messageProc = (LRESULT(__cdecl*)(UINT, WPARAM, LPARAM)) GetProcAddress(hPluginWrapper, "messageProc");
	W_beNotified = (void(__cdecl*)(SCNotification *)) GetProcAddress(hPluginWrapper, "beNotified");
	if (!W_Init || !W_setInfo || !W_getFuncsArray || !W_messageProc || !W_beNotified) {
		MessageBox(NULL, L"Missing exports in SourceCookifierWrapper.dll", L"SourceCookifier", MB_OK);
		return FALSE;
	}

	W_Init();
	W_setInfo(nppData);

	int nbF = 0;
	funcItemCore = W_getFuncsArray(&nbF);
	CORE_ShowFrmMain = funcItemCore[0]._pFunc;
	CORE_GoToDefinition = funcItemCore[2]._pFunc;
	CORE_NavigateBackward = funcItemCore[3]._pFunc;
	CORE_NavigateForward = funcItemCore[4]._pFunc;
#ifdef FIND_IN_SESSION
	CORE_FindInSession = funcItemCore[6]._pFunc;
	CORE_ShowSettings = funcItemCore[8]._pFunc;
	CORE_ShowOptions = funcItemCore[9]._pFunc;
	CORE_ShowHelp = funcItemCore[11]._pFunc;
#else
	CORE_ShowSettings = funcItemCore[6]._pFunc;
	CORE_ShowOptions = funcItemCore[7]._pFunc;
	CORE_ShowHelp = funcItemCore[9]._pFunc;
#endif

	funcItemCore[0]._cmdID = funcItem[0]._cmdID;
	funcItemCore[2]._cmdID = funcItem[2]._cmdID;
	funcItemCore[3]._cmdID = funcItem[3]._cmdID;
	funcItemCore[4]._cmdID = funcItem[4]._cmdID;
#ifdef FIND_IN_SESSION
	funcItemCore[6]._cmdID = funcItem[6]._cmdID;
	funcItemCore[8]._cmdID = funcItem[8]._cmdID;
	funcItemCore[9]._cmdID = funcItem[9]._cmdID;
	funcItemCore[11]._cmdID = funcItem[11]._cmdID;
#else
	funcItemCore[6]._cmdID = funcItem[6]._cmdID;
	funcItemCore[7]._cmdID = funcItem[7]._cmdID;
	funcItemCore[9]._cmdID = funcItem[9]._cmdID;
#endif

	SCNotification * notifyCode = new SCNotification();
	notifyCode->nmhdr.code = NPPN_TBMODIFICATION;
	W_beNotified(notifyCode);
	delete notifyCode;

	return (pluginWrapperLoaded = TRUE);
}

void LoadConfigs()
{
	SendMessage(nppData._nppHandle, NPPM_GETPLUGINSCONFIGDIR, MAX_PATH, (LPARAM)szBuffer);
	wsprintf(szPluginConfig, L"%s\\SourceCookifier.config.xml", szBuffer);

	std::string line;
	std::ifstream sFile;

	sFile.open(szPluginConfig);
	if (sFile.is_open())
	{
		while (!sFile.eof())
		{
			getline(sFile, line);
			if (line.find("StartupShowMode>Hide<", 0) != std::string::npos) {
				HideAtStartup = TRUE;
			}
			else if (line.find("StartupShowMode>Show<", 0) != std::string::npos) {
				ShowAtStartup = TRUE;
			}
		}
		sFile.close();
	}
}

extern "C" __declspec(dllexport) BOOL isUnicode()
{
    return TRUE;
}
extern "C" __declspec(dllexport) void setInfo(NppData notepadPlusData)
{
	nppData = notepadPlusData;
	LoadConfigs();
}
extern "C" __declspec(dllexport) FuncItem * getFuncsArray(int *nbF)
{
    setCommand(0, TEXT("Toogle SourceCookifier"), ShowFrmMain,       false, 0x53/*S*/, true,  true,  true);
    setCommand(1, TEXT("---")                   , NULL,              false, NULL,      false, false, false);
    setCommand(2, TEXT("Go To Definition")      , GoToDefinition,    false, VK_RETURN, true,  true,  false);
    setCommand(3, TEXT("Navigate Backward")     , NavigateBackward,  false, VK_LEFT,   false, false, true);
    setCommand(4, TEXT("Navigate Forward")      , NavigateForward,   false, VK_RIGHT,  false, false, true);
    setCommand(5, TEXT("---")                   , NULL,              false, NULL,      false, false, false);
#ifdef FIND_IN_SESSION
    setCommand(6, TEXT("Find In Session")       , FindInSession,     false, VK_RETURN, true,  true,  true);
    setCommand(7, TEXT("---")                   , NULL,              false, NULL,      false, false, false);
    setCommand(8, TEXT("Language settings")     , ShowSettings,      false, NULL,      false, false, false);
    setCommand(9, TEXT("Options")               , ShowOptions,       false, NULL,      false, false, false);
    setCommand(10, TEXT("---")                  , NULL,              false, NULL,      false, false, false);
    setCommand(11, TEXT("Help && About")        , ShowHelp,          false, NULL,      false, false, false);
#else
    setCommand(6, TEXT("Language settings")     , ShowSettings,      false, NULL,      false, false, false);
    setCommand(7, TEXT("Options")               , ShowOptions,       false, NULL,      false, false, false);
    setCommand(8, TEXT("---")                  , NULL,              false, NULL,      false, false, false);
    setCommand(9, TEXT("Help && About")        , ShowHelp,          false, NULL,      false, false, false);
#endif
	*nbF = nbFunc;
	return funcItem;
}
extern "C" __declspec(dllexport) LRESULT messageProc(UINT Message, WPARAM wParam, LPARAM lParam)
{
	if (pluginWrapperLoaded) {
		return (LRESULT)W_messageProc(Message, wParam, lParam);
	}
	return TRUE;
}
extern "C" __declspec(dllexport) const TCHAR * getName()
{
	return szPluginName;
}
extern "C" __declspec(dllexport) void beNotified(SCNotification *notifyCode)
{
	if (notifyCode->nmhdr.code == NPPN_TBMODIFICATION) {
		tbIcons.hToolbarBmp = (HBITMAP)LoadImage(
			(HINSTANCE)hMod, MAKEINTRESOURCE(IDB_COOKIE_MONSTER), IMAGE_BITMAP, 0, 0, (LR_DEFAULTSIZE | LR_LOADMAP3DCOLORS | LR_LOADTRANSPARENT));
		SendMessage(nppData._nppHandle, NPPM_ADDTOOLBARICON, (WPARAM)funcItem[0]._cmdID, (LPARAM)&tbIcons);
		return;
	}
	else if (notifyCode->nmhdr.code == NPPN_READY)
	{
		if (!FormShown && ShowAtStartup)
			ShowFrmMain();
	}
	if (pluginWrapperLoaded) {
		W_beNotified(notifyCode);
	}
	else {
		if (notifyCode->nmhdr.code == NPPN_FILEBEFOREOPEN)
    	{
    		TCHAR szPath[MAX_PATH];
    		if (SendMessage(nppData._nppHandle, NPPM_GETFULLPATHFROMBUFFERID, notifyCode->nmhdr.idFrom, (LPARAM)szPath) != -1)
    		{
				LPWSTR pExt = PathFindExtension(szPath);
				if (!lstrcmp(pExt, L".c00k!e")) {
					if (GetPluginWrapper()) {
						FormShown = TRUE;
						W_beNotified(notifyCode);
					}
				}
    		}
    	}
	}
}

void setCommand(size_t index, TCHAR *cmdName, PFUNCPLUGINCMD pFunc, bool check0nInit, UCHAR key, bool ctrl, bool shift, bool alt) 
{
	ShortcutKey *pShKey = NULL;
	if (key) {
		pShKey = new ShortcutKey;
		pShKey->_isCtrl = ctrl;
		pShKey->_isShift = shift;
		pShKey->_isAlt = alt;
		pShKey->_key = key;
	}
    lstrcpy(funcItem[index]._itemName, cmdName);
    funcItem[index]._pFunc = pFunc;
    funcItem[index]._init2Check = check0nInit;
    funcItem[index]._pShKey = pShKey;
}

void ShowFrmMain()
{
	if (HideAtStartup)
	{
		HideAtStartup = FALSE;
		return;
	}
	if (GetPluginWrapper() && CORE_ShowFrmMain)
	{
		ShowAtStartup = FALSE;
		FormShown = TRUE;
		CORE_ShowFrmMain();
	}
}
void GoToDefinition() { if (GetPluginWrapper() && CORE_GoToDefinition) CORE_GoToDefinition(); }
void NavigateBackward() { if (GetPluginWrapper() && CORE_NavigateBackward) CORE_NavigateBackward(); }
void NavigateForward() { if (GetPluginWrapper() && CORE_NavigateForward) CORE_NavigateForward(); }
void FindInSession() { if (GetPluginWrapper() && CORE_FindInSession) CORE_FindInSession(); }
void ShowSettings() { if (GetPluginWrapper() && CORE_ShowSettings) CORE_ShowSettings(); }
void ShowOptions() { if (GetPluginWrapper() && CORE_ShowOptions) CORE_ShowOptions(); }
void ShowHelp() { if (GetPluginWrapper() && CORE_ShowHelp) CORE_ShowHelp(); }
