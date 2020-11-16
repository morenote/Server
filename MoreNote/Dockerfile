#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# docker pull mcr.microsoft.com/dotnet/sdk:5.0
# mcr.microsoft.com/dotnet/aspnet:5.0
FROM mcr.microsoft.com/dotnet/aspnet:5.0  AS base
WORKDIR /app
EXPOSE 80


FROM mcr.microsoft.com/dotnet/aspnet:5.0  AS build
WORKDIR /src
COPY ["MoreNote/MoreNote.csproj", "MoreNote/"]
COPY ["MoreNote.Common/MoreNote.Common.csproj", "MoreNote.Common/"]
COPY ["MoreNote.Logic/MoreNote.Logic.csproj", "MoreNote.Logic/"]
RUN dotnet restore "MoreNote/MoreNote.csproj"
COPY . .
WORKDIR "/src/MoreNote"
RUN dotnet build "MoreNote.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MoreNote.csproj" -c Release -o /app/publish
VOLUME ["/etc/morenote"]


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoreNote.dll"]