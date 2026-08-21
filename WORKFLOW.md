# Core workflow

```bash
dotnet restore Core.csproj
dotnet build Core.csproj -c Release --no-restore \
  --disable-build-servers -m:1 /p:UseSharedCompilation=false
git diff --check
```
