FROM mcr.microsoft.com/dotnet/aspnet:8.0

RUN apt-get update \
    && apt-get install -y libgdiplus libc6-dev libx11-dev libicu-dev libharfbuzz0b libfontconfig1 libfreetype6 \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY ./bin/Release/net8.0/publish/ .

ENTRYPOINT ["dotnet", "berry.dll"]