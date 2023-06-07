#WhatsApp Cloud Api in .Net 7.0

To deploy and get .dll file execute: 

```
dotnet restore
```
```
dotnet publish -c Release -o out
```
```
dotnet out/WhatsappNet.dll
```

To Run Container
```
docker run -d -p 5024:5024 --name whatsNetCont --env-file ./.env whatsappnetimg
```