#!/bin/bash

nohup dotnet ~/work/Services/Sangong/SangongGame/Sangong.Game.WebApi.dll --urls="http://*:10000" --MatchingGroup="1" --Rabbitmq:Queue="SangongGame-1-1" --Rabbitmq:Mathcing="SangongMatching-1" >> /dev/null