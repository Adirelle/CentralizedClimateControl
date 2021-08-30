#!/usr/bin/env bash
set -eux

CHANGELOG=`pandoc -t .pandoc/SteamBBCode.lua "$4" | sed -z -e 's/"/\\"/g;s/\\n/\\\\n/g'`

cat << EOF
"workshopitem"
{
    "appid"            "$1"
    "publishedfileid"  "$2"
    "contentfolder"    "$3"
    "changenote"       "${CHANGELOG}"
}
EOF
