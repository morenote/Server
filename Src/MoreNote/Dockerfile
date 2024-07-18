FROM mcr.microsoft.com/dotnet/aspnet:8.0  AS base
WORKDIR /app

#APP 应用
COPY ["bin/Release/net8.0/publish", "/app"]

EXPOSE 5000
ENTRYPOINT ["dotnet", "MoreNote.dll"]