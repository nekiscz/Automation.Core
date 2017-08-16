You can use NUnit console to run these tests with console (Jenkins, TFS, Octopus some 
other CICD application that can execute commands)


"%~dp0nunit3-console.exe" --params:Browser=chrome "%~dp0..\..\{dllname}.dll"

I added .bat file with same line as above. So you need to change the {dllname} variable 
in that file if you want to use it.