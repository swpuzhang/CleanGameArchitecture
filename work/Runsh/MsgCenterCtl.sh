#!/bin/bash

nohup dotnet ~/work/Services/MsgCenter/MsgCenter.WebApi.dll --urls="http://*:13000" >> /dev/null