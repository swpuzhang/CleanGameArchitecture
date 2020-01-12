#!/bin/bash

nohup dotnet ~/work/Services/Sangong/SangongMatching/Sangong.Matching.WebApi.dll --urls="http://*:9000" --MatchingGroup="1" --Rabbitmq:Queue="SangongMatching-1" >> /dev/null