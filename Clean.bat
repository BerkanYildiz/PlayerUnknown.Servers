for /d /r . %%d in (Builds,bin,obj,debug,release,x64,x86) do @if exist "%%d" rd /s/q "%%d"
RMDIR /S /Q "packages"