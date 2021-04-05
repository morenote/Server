#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# docker pull mcr.microsoft.com/dotnet/sdk:5.0
# mcr.microsoft.com/dotnet/aspnet:5.0
FROM mcr.microsoft.com/dotnet/aspnet:5.0  AS base
WORKDIR /app
EXPOSE 80
# use mirrors.aliyun.com/debian
COPY ["Src/MoreNote/sources.list","/app/sources.list"]
RUN  mv /etc/apt/sources.list /etc/apt/sources.list.bak &&cp sources.list /etc/apt/sources.list&&cat /etc/apt/sources.list 
# https://stackoverflow.com/questions/30451939/cant-install-libc6-package/30452149
RUN  apt-get autoclean&&apt-get update&&apt-get upgrade&& apt-get -u dist-upgrade&&dpkg --configure -a&&apt-get -f install&&apt-get install -y libgdiplus &&ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll








FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /code
#替换软件源
COPY ["Src/MoreNote/sources.list","/app/sources.list"]
RUN  mv /etc/apt/sources.list /etc/apt/sources.list.bak &&cp /app/sources.list /etc/apt/sources.list&&cat /etc/apt/sources.list 


#解决证书问题
#https://q.cnblogs.com/q/133315/
ADD http://ftp.us.debian.org/debian/pool/main/c/ca-certificates/ca-certificates_20210119_all.deb .
RUN dpkg -i ca-certificates_20210119_all.deb
RUN apt-get update -y && apt-get install -y ca-certificates && update-ca-certificates && rm -rf /var/lib/apt/lists/*


#APP 应用
COPY ["Src/MoreNote/MoreNote.csproj", "Src/MoreNote/"]
#通用层
COPY ["Src/MoreNote.Common/MoreNote.Common.csproj", "Src/MoreNote.Common/"]
#逻辑层
COPY ["Src/MoreNote.Logic/MoreNote.Logic.csproj", "Scrc/MoreNote.Logic/"]
#框架
COPY ["Src/03Framework/Morenote.Framework/Morenote.Framework.csproj", "Src/03Framework/Morenote.FrameworkMorenote/"]
#国际化
COPY ["Src/MoreNote.Language/MoreNote.Language.csproj", "Src/MoreNote.Language/"]
RUN  

RUN dotnet restore "Src/MoreNote/MoreNote.csproj"
COPY . .
WORKDIR "/code/Src/MoreNote"
RUN dotnet build "MoreNote.csproj" -c Release -o /app/build



FROM build AS publish
RUN dotnet publish "MoreNote.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoreNote.dll"]