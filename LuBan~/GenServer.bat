set TOOL=dotnet .\Tools\Luban.ClientServer\Luban.ClientServer.dll
set SERVER_CODE=..\Server\Model\Generate\LuBan\Code
set SERVER_DATA=..\Server\Model\Generate\LuBan\Data

%TOOL% -j cfg --^
 -d Defines\__root__.xml ^
 --input_data_dir Datas ^
 --output_data_dir %SERVER_DATA% ^
 --output_code_dir %SERVER_CODE% ^
 --gen_types code_cs_dotnet_json,data_json ^
 -s server
pause