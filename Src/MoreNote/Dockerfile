#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# docker pull mcr.microsoft.com/dotnet/sdk:5.0
# mcr.microsoft.com/dotnet/aspnet:5.0
FROM mcr.microsoft.com/dotnet/aspnet:6.0  AS base
WORKDIR /app
EXPOSE 80


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /code
#APP 应用
COPY ["Src/MoreNote/MoreNote.csproj", "Src/MoreNote/"]
#通用层
COPY ["Src/MoreNote.Common/MoreNote.Common.csproj", "Src/MoreNote.Common/"]
#加密提供服务
COPY ["Src/MoreNote.CryptographyProvider/MoreNote.CryptographyProvider.csproj", "Src/MoreNote.CryptographyProvider/"]
#人脸识别服务
COPY ["Src/MoreNote.Face/MoreNote.Face.csproj", "Src/MoreNote.Face/"]
#逻辑层
COPY ["Src/MoreNote.Logic/MoreNote.Logic.csproj", "Src/MoreNote.Logic/"]
#模型层
COPY ["Src/MoreNote.Models/MoreNote.Models.csproj", "Src/MoreNote.Models/"]
#框架
COPY ["Src/03Framework/Morenote.Framework/Morenote.Framework.csproj", "Src/03Framework/Morenote.FrameworkMorenote/"]
#国际化
COPY ["Src/MoreNote.Language/MoreNote.Language.csproj", "Src/MoreNote.Language/"]
#签名服务
COPY ["Src/MoreNote.SignatureService/MoreNote.SignatureService.csproj", "Src/MoreNote.SignatureService/"]
 
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