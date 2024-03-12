FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

ARG CACHEBUST=1
RUN ls -r

COPY ./bin/Release/net8.0/publish/ .

ENTRYPOINT ["dotnet", "berry.dll"]