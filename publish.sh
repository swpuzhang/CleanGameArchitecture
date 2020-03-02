#! /bin/bash
cd Services/Account
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true -o ./app

cd ../../Services/Money
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true -o ./app

cd ../../Services/Reward
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true -o ./app

cd ../../Services/RoomMatch
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true -o ./app

cd ../../Services/Game
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true -o ./app

cd ../../Gateways/ApiGateway
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true -o ./app

cd ../../Gateways/WSGateway
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true -o ./app

cd ../..
cp Services/Account/app/Account /CleanGame/run/Services/Account/
cp Services/Money/app/Money /CleanGame/run/Services/Money/
cp Services/Reward/app/Reward /CleanGame/run/Services/Reward/
cp Services/RoomMatch/app/RoomMatch /CleanGame/run/Services/RoomMatch/
cp Services/Game/app/Game /CleanGame/run/Services/Game/
cp Gateways/ApiGateway/app/ApiGateway /CleanGame/run/Gateways/ApiGateway/
cp Gateways/WSGateway/app/WSGateway /CleanGame/run/Gateways/WSGateway/
cp run/SwaggerInterface/*  /CleanGame/run/SwaggerInterface/