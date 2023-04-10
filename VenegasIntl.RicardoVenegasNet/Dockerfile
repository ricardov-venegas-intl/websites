#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-jammy AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt-get update 
RUN apt-get -y install sudo curl jq 
RUN sudo mkdir -p --mode=0755 /usr/share/keyrings
RUN curl -fsSL https://pkg.cloudflare.com/cloudflare-main.gpg | sudo tee /usr/share/keyrings/cloudflare-main.gpg >/dev/null
RUN echo 'deb [signed-by=/usr/share/keyrings/cloudflare-main.gpg] https://pkg.cloudflare.com/cloudflared jammy main' | sudo tee /etc/apt/sources.list.d/cloudflared.list
RUN sudo apt-get update && sudo apt-get install cloudflared

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["VenegasIntl.RicardoVenegasNet.csproj", "."]
RUN dotnet restore "./VenegasIntl.RicardoVenegasNet.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "VenegasIntl.RicardoVenegasNet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VenegasIntl.RicardoVenegasNet.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VenegasIntl.RicardoVenegasNet.dll"]