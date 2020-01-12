#!/bin/bash

nohup dotnet ~/work/GateWays/ApiGateWay/ApiGateWay.dll --urls="http://*:5000" >> /dev/null