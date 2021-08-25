#!/usr/bin/env bash
exec dotnet.exe tool run dotnet-format --check --no-restore -wsa info CentralizedClimateControl.sln "$@"
