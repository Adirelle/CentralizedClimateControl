#!/usr/bin/env bash
set -eu
tmpFile=$(mktemp)
(
    sed -e '0,/<description>/'!d About/About.xml
    echo '<[CDATA['
    pandoc -t .pandoc/SteamBBCode.lua README.md
    echo ']]>'
    sed -e '\x</description>x,$'!d About/About.xml
) >"$tmpFile"

mv "$tmpFile" About/About.xml
