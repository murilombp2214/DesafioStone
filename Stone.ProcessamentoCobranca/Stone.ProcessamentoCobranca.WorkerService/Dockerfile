#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Stone.ProcessamentoCobranca.WorkerService/Stone.ProcessamentoCobranca.WorkerService.csproj", "Stone.ProcessamentoCobranca.WorkerService/"]
RUN dotnet restore "Stone.ProcessamentoCobranca.WorkerService/Stone.ProcessamentoCobranca.WorkerService.csproj"
COPY . .
WORKDIR "/src/Stone.ProcessamentoCobranca.WorkerService"
RUN dotnet build "Stone.ProcessamentoCobranca.WorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Stone.ProcessamentoCobranca.WorkerService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stone.ProcessamentoCobranca.WorkerService.dll"]