FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["MoreNote/MoreNote.csproj", "MoreNote/"]
RUN dotnet restore "MoreNote/MoreNote.csproj"
COPY . .
WORKDIR "/src/MoreNote"
RUN dotnet build "MoreNote.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MoreNote.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MoreNote.dll"]
