FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

COPY . .
RUN dotnet restore

RUN dotnet publish src/Wipro.WebCrawler.Web -c Release -o out

FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app

COPY --from=build-env /app/src/Wipro.WebCrawler.Web/out .

ENV ASPNETCORE_URLS http://*:5000

ENTRYPOINT ["dotnet", "Wipro.WebCrawler.Web.dll"]
