#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MoreNote/MoreNote.csproj", "MoreNote/"]
COPY ["MoreNote.Common/MoreNote.Common.csproj", "MoreNote.Common/"]
COPY ["UpYunLibrary/UpYunLibrary.csproj", "UpYunLibrary/"]
COPY ["PAYJS_CSharp_SDK/PAYJS_CSharp_SDK.csproj", "PAYJS_CSharp_SDK/"]
COPY ["MoreNote.Logic/MoreNote.Logic.csproj", "MoreNote.Logic/"]
RUN dotnet restore "MoreNote/MoreNote.csproj"
COPY . .
WORKDIR "/src/MoreNote"
RUN dotnet build "MoreNote.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MoreNote.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoreNote.dll"]