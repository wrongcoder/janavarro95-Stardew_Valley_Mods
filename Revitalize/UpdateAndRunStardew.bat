call %windir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe "C:\Users\owner\Documents\Visual Studio 2015\Projects\github\Stardew_Valley_Mods\Revitalize\Revitalize\Revitalize.sln"
call xnb_node.cmd pack RevitalizeProjectDecompiled RevitalizeProjectCompiled
xcopy /e /v /y RevitalizeProjectCompiled "C:\Users\owner\Documents\Visual Studio 2015\Projects\github\Stardew_Valley_Mods\Revitalize\RevitalizeProjectCompiled\"
xcopy /e /v /y RevitalizeProjectDecompiled "C:\Users\owner\Documents\Visual Studio 2015\Projects\github\Stardew_Valley_Mods\Revitalize\RevitalizeProjectDecompiled\"
start "StardewModdingAPI" "C:\Program Files (x86)\Steam\steamapps\common\Stardew Valley\StardewModdingAPI.exe"