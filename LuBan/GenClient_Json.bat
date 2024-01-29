set TOOL=dotnet .\Tools\Luban.ClientServer\Luban.ClientServer.dll
set CLIENT_CODE=..\Assets\Core\LFramework\Runtime\Generate\LuBan
set CLIENT_DATA=..\Assets\AssetsPackage\LuBan

%TOOL% -j cfg --^
 -d Defines\__root__.xml ^
 --input_data_dir Datas ^
 --output_data_dir %CLIENT_DATA% ^
 --output_code_dir %CLIENT_CODE% ^
 --gen_types code_cs_unity_json,data_json ^
 -s client
pause