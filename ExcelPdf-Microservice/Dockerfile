#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 57876
EXPOSE 5264

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ExcelPdf-Microservice/ExcelPdf-Microservice.csproj", "ExcelPdf-Microservice/"]
RUN dotnet restore "ExcelPdf-Microservice/ExcelPdf-Microservice.csproj"
COPY . .
WORKDIR "/src/ExcelPdf-Microservice"
RUN dotnet build "ExcelPdf-Microservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExcelPdf-Microservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExcelPdf-Microservice.dll"]