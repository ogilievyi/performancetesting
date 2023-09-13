set mypath=%cd%
call sql\1run_mssql.bat
cd %mypath%
call sql\2run_grafana.bat
cd %mypath%
call build.bat
cd %mypath%
call runpf3.bat


