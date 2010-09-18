#pragma warning( disable : 4947 ) // 'AppendPrivatePath' has been deprecated..

using namespace System;
using namespace System::IO;
using namespace System::Reflection;
using namespace System::Windows::Forms;

namespace NppPluginNET
{
	public ref class WrapperManaged
	{
	public:
		static PluginBase^ _pluginBase;

		static void Init()
		{
			try
			{
				String^ pluginPath = Assembly::GetExecutingAssembly()->Location;
				String^ nppPluginsDir = Path::GetDirectoryName(pluginPath);
				AppDomain::CurrentDomain::get()->AppendPrivatePath(nppPluginsDir);
			}
			catch (Exception^ ex) { MessageBox::Show(ex->Message); }
		}
		static void CreatePluginInstance()
		{
			try
			{
				_pluginBase = gcnew PluginBase();
			}
			catch (Exception^ ex) { MessageBox::Show(ex->Message); }
		}
	};
}
