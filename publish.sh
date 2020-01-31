#! /bin/bash
cd Services/Account
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true -o ./app
cd ../../Services/Money
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true -o ./app
cd ../../Services/Reward
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true -o ./app
cd ../../Gateways/ApiGateway
dotnet publish -c Release -r linux-x64 /p:PublishSingleFile=true -o ./app
cd ../..
cp Services/Account/app/Account /CleanGame/run/Services/Account
cp Services/Money/app/Money /CleanGame/run/Services/Money
cp Services/Reward/app/Reward /CleanGame/run/Services/Reward
cp Gateways/ApiGateway/app/ApiGateway /CleanGame/run/Gateways/ApiGateway