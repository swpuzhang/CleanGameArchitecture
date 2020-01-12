#!/bin/bash

nohup dotnet ~/work/GateWays/WSGateWay/WSGateWay.dll --urls="http://*:6000" --Rabbitmq:Queue="WSGateWay-1" --WSHost="http://39.100.129.247/App" >> /dev/null