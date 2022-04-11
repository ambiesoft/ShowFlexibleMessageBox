
#include <string>
#include "../../lsMisc/SessionGlobalMemory/SessionGlobalMemory.h"
#include "../../lsMisc/stdosd/stdosd.h"
#include "../../lsMisc/UTF16toUTF8.h"
#include "../../lsMisc/OpenCommon.h"

using namespace Ambiesoft;
using namespace Ambiesoft::stdosd;
using namespace std;

int main()
{
	// あいうえお
	string str = "%E3%81%82%E3%81%84%E3%81%86%E3%81%88%E3%81%8A";
	CDynamicSessionGlobalMemory sgDyn("dyn", (size_t32)str.size());
	{
		sgDyn.set((const unsigned char*)str.data());
	}

	wstring exe = stdCombinePath(
		stdCombinePath(
			stdGetParentDirectory(stdGetModuleFileName()),
			L"../../../.."),
		stdFormat(L"ShowFlexibleMessageBox/bin/%s/ShowFlexibleMessageBox.exe",
#ifdef _DEBUG
			L"Debug"
#else
			L"Release"
#endif
		));

	wstring arg = stdFormat(L"-m \"%s\"", toStdWstringFromUtf8(sgDyn.getMapName()).c_str());

	HANDLE process = nullptr;
	OpenCommon(nullptr, exe.c_str(), arg.c_str(), nullptr, &process);
	WaitForInputIdle(process, INFINITE);
	return 0;
}
