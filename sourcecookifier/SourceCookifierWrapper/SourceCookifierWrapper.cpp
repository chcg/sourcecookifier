#pragma unmanaged
#include "NppIncludes\PluginInterface.h"
#include "WrapperUnmanaged.h"

BOOL APIENTRY DllMain(HANDLE hModule, DWORD reasonForCall, LPVOID lpReserved)
{
    return TRUE;
}

extern "C" __declspec(dllexport) void Init()
{
	W_Init();
}

extern "C" __declspec(dllexport) void setInfo(NppData notepadPlusData)
{
	W_setInfo(notepadPlusData);
}
extern "C" __declspec(dllexport) FuncItem * getFuncsArray(int *nbF)
{
	return W_getFuncsArray(nbF);
}
extern "C" __declspec(dllexport) LRESULT messageProc(UINT Message, WPARAM wParam, LPARAM lParam)
{
	return (LRESULT)W_messageProc(Message, wParam, lParam);
}
extern "C" __declspec(dllexport) void beNotified(SCNotification *notifyCode)
{
	W_beNotified(notifyCode);
}
