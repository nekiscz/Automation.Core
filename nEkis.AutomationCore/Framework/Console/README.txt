You can use NUnit console to run these tests with console (Jenkins, TFS, Octopus some 
other CICD application that can execute commands)


"%~dp0nunit3-console.exe" --params:Browser=chrome "%~dp0..\..\{dllname}.dll"

I added .bat file with same line as above. So you need to change the {dllname} variable 
in that file if you want to use it. I also added NUnitOrange that will generate HTML
report for you from TestResult.xml file.

"%~dp0NUnitOrange" "%~dp0TestResult.xml" "%~dp0..\..\Report\testreport.%d%_%t%.html"

As you can see filename is specified with path so, if you change TestReslt.xml 
directory you need to cange it there too. Report directory is created in Log sontructor
so you need to call it before you will run this line of script. Why in Log? Don't 
know...

Rest of the scripts just gets current date for HTML result file.

DON'T FORGET TO SET ALL FILES IN THIS FOLDER TO "Copy if newer"!