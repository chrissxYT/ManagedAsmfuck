#!/bin/sh
dotnet msbuild /m /p:Configuration=Release /p:Platform=x64 /p:OutputPath=_build/x64/
dotnet msbuild /m /p:Configuration=Release /p:Platform=x86 /p:OutputPath=_build/x86/
dotnet msbuild /m /p:Configuration=Release /p:Platform="Any CPU" /p:OutputPath=_build/any/
