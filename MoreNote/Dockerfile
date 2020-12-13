#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# docker pull mcr.microsoft.com/dotnet/sdk:5.0
# mcr.microsoft.com/dotnet/aspnet:5.0
FROM mcr.microsoft.com/dotnet/aspnet:5.0  AS base
WORKDIR /app
EXPOSE 80
# use mirrors.aliyun.com/debian
COPY ["MoreNote/sources.list","/app/sources.list"]
RUN  mv /etc/apt/sources.list /etc/apt/sources.list.bak &&cp sources.list /etc/apt/sources.list&&cat /etc/apt/sources.list 
# https://stackoverflow.com/questions/30451939/cant-install-libc6-package/30452149
RUN  apt-get autoclean&&apt-get update&&apt-get upgrade&& apt-get -u dist-upgrade&&dpkg --configure -a&&apt-get -f install&&apt-get install -y libgdiplus &&ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll

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
VOLUME ["/morenote"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoreNote.dll"]