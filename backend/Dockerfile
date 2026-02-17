FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY FerHf.DAL/FerHf.DAL.csproj FerHf.DAL/
COPY FerHf.BLL/FerHf.BLL.csproj FerHf.BLL/
COPY FerHf.API/FerHf.API.csproj FerHf.API/
RUN dotnet restore FerHf.API/FerHf.API.csproj

COPY . .
RUN dotnet publish FerHf.API/FerHf.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "FerHf.API.dll"]
