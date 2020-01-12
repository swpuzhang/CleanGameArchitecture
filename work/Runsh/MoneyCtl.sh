#!/bin/bash

nohup dotnet ~/work/Services/Money/Money.WebApi.dll --urls="http://*:8000"  >> /dev/null