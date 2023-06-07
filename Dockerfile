FROM mcr.microsoft.com/dotnet/nightly/sdk:7.0 AS build
WORKDIR whatsappnet

EXPOSE 80
EXPOSE 5024

#Copy ProjectFiles

COPY ./*.csproj ./
RUN dotnet restore

#Copy Everthing else
COPY . .
RUN dotnet publish -c Release -o out

#Build image
FROM mcr.microsoft.com/dotnet/nightly/sdk:7.0
WORKDIR whatsappnet
COPY --from=build /whatsappnet/out .
ENTRYPOINT ["dotnet" , "WhatsappNet.dll"]