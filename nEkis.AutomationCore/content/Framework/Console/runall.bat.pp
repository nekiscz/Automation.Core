set word=-
set d=%DATE%
call set d=%%d:.=%word%%%

set t=%TIME%
call set t=%%t::=%word%%%
call set t=%%t:,=%word%%%

"%~dp0nunit3-console.exe" --params:Browser=chrome "%~dp0..\..\{dllname}.dll"
"%~dp0NUnitOrange" "%~dp0TestResult.xml" "%~dp0..\..\Report\testreport.%d%_%t%.html"