#!/bin/bash

nohup dotnet ~/work/Services/Reward/Reward.WebApi.dll --urls="http://*:11000" >> /dev/null