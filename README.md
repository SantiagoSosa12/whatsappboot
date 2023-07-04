# WhatsApp Cloud Api in .Net 7.0

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
---------------------------------------
### To build image
```
docker build -t whatsappnetimg .
```
To Run Container
```
docker run -d -p 5024:5024 --name whatsNetCont --env-file ./.env whatsappnetimg
```
---------------------------------------
### To push image
```
docker login
```
```
docker tag whatsappnetimg YOURUSERNAME/whatsappnetimg
```
```
docker push YOURUSERNAME/whatsappnetimg
```

