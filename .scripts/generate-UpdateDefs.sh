#!/usr/bin/env bash
exec pandoc -t .pandoc/UpdateFeatureDefs.lua CHANGELOG.md -o 1.3/Defs/UpdateFeatureDefs/UpdateFeatures.xml
