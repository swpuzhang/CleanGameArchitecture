#!/bin/bash

nohup dotnet ~/work/Services/Account/Account.WebApi.dll --urls="http://*:7000" >> /dev/null