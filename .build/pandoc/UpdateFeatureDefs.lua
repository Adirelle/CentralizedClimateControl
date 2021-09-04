-- Preload UnityRichTextFormat
dofile ".build/pandoc/UnityRichTextFormat.lua"

local MOD_ID = "Adirelle.CentralizedClimateControl"
local MOD_NAME = "Centralized climate control"
local MOD_REPO = "https://github.com/Adirelle/CentralizedClimateControl"
local MOD_PREFIX = "CCC"

local function findVersions(body)
  local a, b, match = string.find(body, '%s*<<([^>]+)>>%s*')
  if a then
    return string.sub(body, 1, a), match, findVersions(string.sub(body, b))
  end
  return body
end

local function formatVersion(current, version, text)
  if version == "Unreleased" then
    if not current then
      return ""
    end
    version = current
  end
  return string.format([=[
  <HugsLib.UpdateFeatureDef ParentName="%s_News">
    <defName>%s_%s</defName>
    <assemblyVersion>%s</assemblyVersion>
    <linkUrl>%s/releases/tag/%s</linkUrl>
    <content><![CDATA[%s]]></content>
  </HugsLib.UpdateFeatureDef>
]=],
    MOD_PREFIX,
    MOD_PREFIX, version:gsub('%W', '_'),
    version,
    MOD_REPO, version,
    text:gsub("^[ \n\r\t]*(.-)[ \n\r\t]*$", "%1")
  )
end

local function formatVersions(current, version, text, ...)
  if not version then
    return ""
  end
  return formatVersion(current, version, text) .. formatVersions(current, ...)
end

local function dropFirst(_, ...)
  return ...
end

function Doc(body, _, variables)
  local versions = formatVersions(variables.VERSION, dropFirst(findVersions(body, 1)))

  return string.format([[
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Defs>
  <HugsLib.UpdateFeatureDef Abstract="true" Name="%s_News">
    <modNameReadable>%s</modNameReadable>
    <modIdentifier>%s</modIdentifier>
  </HugsLib.UpdateFeatureDef>
%s</Defs>
]],
    MOD_PREFIX,
    MOD_NAME,
    MOD_ID,
    versions
  )
end

function Header(lev, s, attr)
  if lev ~= 2 then
    return string.format('\n|<b><size=%d>%s</size></b>', 24 - 2 * lev, s)
  end

  return '<<' ..  s:match("^(%S+)") .. '>>'
end

function Image(_, src)
  return "\n|img:" .. src
end

function Para(s)
  return "|" .. s
end

function BulletList(items)
  local buffer = {}
  for _, item in pairs(items) do
    table.insert(buffer, "\n|* " .. item)
  end
  return table.concat(buffer, "")
end

function OrderedList(items)
  local buffer = {}
  for i, item in ipairs(items) do
    table.insert(buffer, "\n|" .. (i+1) .. ". " .. item)
  end
  return table.concat(buffer, "")
end

function DefinitionList(items)
  local buffer = {}
  for _,item in pairs(items) do
    local k, v = next(item)
    table.insert(buffer, "\n|" .. k .. ": " .. v)
  end
  return table.concat(buffer, "")
end

function CaptionedImage(src, _, caption)
  return Image("", src) .. "|caption:" .. caption
end
