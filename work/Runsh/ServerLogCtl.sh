#!/bin/bash

nohup dotnet ~/work/Services/ServerLog/ServerLog.WebApi.dll --urls="http://*:14000" >> /dev/null