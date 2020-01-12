#!/bin/bash

nohup dotnet ~/work/Services/Friend/Friend.WebApi.dll --urls="http://*:12000" >> /dev/null