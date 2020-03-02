#! /bin/bash
cd Services/Account
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true -o ./app --self-contained

cd ../../Services/Money
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true -o ./app --self-contained

cd ../../Services/Reward
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true -o ./app --self-contained

cd ../../Services/RoomMatch
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true -o ./app --self-contained

cd ../../Services/Game
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true -o ./app --self-contained

cd ../../Gateways/ApiGateway
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true -o ./app --self-contained

cd ../../Gateways/WSGateway
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true -o ./app --self-contained

cd ../..
mkdir -p /CleanGame/run/Services/Account
mkdir -p /CleanGame/run/Services/Money
mkdir -p /CleanGame/run/Services/Reward
mkdir -p /CleanGame/run/Services/RoomMatch
mkdir -p /CleanGame/run/Services/Game
mkdir -p /CleanGame/run/Gateways/ApiGateway
mkdir -p /CleanGame/run/Gateways/WSGateway
cp Services/Account/app/Account /CleanGame/run/Services/Account/
cp Services/Money/app/Money /CleanGame/run/Services/Money/
cp Services/Reward/app/Reward /CleanGame/run/Services/Reward/
cp Services/RoomMatch/app/RoomMatch /CleanGame/run/Services/RoomMatch/
cp Services/Game/app/Game /CleanGame/run/Services/Game/
cp Gateways/ApiGateway/app/ApiGateway /CleanGame/run/Gateways/ApiGateway/
cp Gateways/WSGateway/app/WSGateway /CleanGame/run/Gateways/WSGateway/
cp run/SwaggerInterface/*  /CleanGame/run/SwaggerInterface/