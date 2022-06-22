FROM mcr.microsoft.com/dotnet/aspnet:6.0.6-jammy-arm64v8  AS base
WORKDIR /app

#APP 应用
COPY ["app", "/app"]

EXPOSE 80
ENTRYPOINT ["dotnet", "MoreNote.dll"]