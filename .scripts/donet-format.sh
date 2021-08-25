#!/usr/bin/env bash
exec dotnet.exe tool run dotnet-format --no-restore -wsa info CentralizedClimateControl.sln "$@"
