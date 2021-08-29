# AnimeLion

## Build:

```cmd
cd AnimeLion
```

#### For Windows
```cmd
dotnet publish "AnimeLion.csproj" -c release -f net5.0 -r win-x64 --self-contained
```
#### For Mac
```cmd
dotnet publish "AnimeLion.csproj" -c release -f net5.0 -r osx-x64 --self-contained
```

#### For Linux
```cmd
dotnet publish "AnimeLion.csproj" -c release -f net5.0 -r linux-x64 --self-contained
```


### First you need to start the server:
```cmd
cd AnimeLion.Web
dotnet watch run
```
