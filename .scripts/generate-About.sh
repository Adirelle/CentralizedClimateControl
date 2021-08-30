#!/usr/bin/env bash
sed -e '0,/<description>/!d' About/About.xml
echo '<![CDATA['
pandoc -t .pandoc/SteamBBCode.lua README.md
echo ']]>'
sed -e '\x</description>x,$!d' About/About.xml
