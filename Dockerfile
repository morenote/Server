FROM mcr.microsoft.com/dotnet/aspnet:7.0  AS base
WORKDIR /app

#APP 应用
COPY ["linux64", "/app"]
EXPOSE 5000
ENTRYPOINT ["dotnet", "MoreNote.dll"]