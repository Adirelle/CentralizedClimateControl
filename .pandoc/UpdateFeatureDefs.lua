-- Preload UnityRichTextFormat
dofile ".pandoc/UnityRichTextFormat.lua"

local MOD_ID = "CentralizedClimateControl"
local MOD_NAME = "Centralized climate control 2.0"
local MOD_REPO = "https://github.com/Adirelle/CentralizedClimateControl"
local MOD_PREFIX = "CCC"

local open = false

local function close()
  if open then
    open = false
    return "]]></content>\n  </HugsLib.UpdateFeatureDef>\n"
  end
  return ""
end

function Doc(body, metadata, variables)
  local buffer = {}
  local function add(s)
    table.insert(buffer, s)
  end
  add(
[[<?xml version="1.0" encoding="utf-8" ?>
<Defs>
  <HugsLib.UpdateFeatureDef Abstract="true" Name="]] .. MOD_PREFIX .. [[_UpdateFeatureBase">
    <modNameReadable>]] .. MOD_NAME .. [[</modNameReadable>
    <modIdentifier>]] .. MOD_ID .. [[</modIdentifier>
    <linkUrl>]] .. MOD_REPO .. [[</linkUrl>
    <trimWhitespace>true</trimWhitespace>
  </HugsLib.UpdateFeatureDef>]]
  )

  body = body .. close()
  local defs = body
    :match('(%s+%<HugsLib%.UpdateFeatureDef .+)$')
    :gsub('(<!%[CDATA%[%)%s+', '%1')
    :gsub('%s+(%]%]>)', '%1')
  add(defs)

  add('</Defs>')
  return table.concat(buffer,'')
end

function Image(_, src)
  return "\n|img:" .. src
end

function Para(s)
  return "|" .. s
end

-- lev is an integer, the header level.
function Header(lev, s, attr)
  if lev == 2 then
    local buffer = {}
    local function add(s)
      table.insert(buffer, s)
    end
      add(close())
    local version = s:match("^(%S+)")
    add('  <HugsLib.UpdateFeatureDef ParentName="' .. MOD_PREFIX .. '_UpdateFeatureBase">')
    add('    <defName>' .. MOD_PREFIX .. '_' .. version:gsub('%W', '_') .. '</defName>')
    if version == "Unreleased" then
      add('    <assemblyVersion>9.9.9</assemblyVersion>')
      add('    <linkUrl>' .. MOD_REPO .. '/releases/latest</linkUrl>')
    else
      add('    <assemblyVersion>' .. version .. '</assemblyVersion>')
      add('    <linkUrl>' .. MOD_REPO .. '/releases/tag/' .. version .. '</linkUrl>')
    end
    add('    <content><![CDATA[')
    open = true
    return table.concat(buffer, '\n')
  end

  return '\n|<b><size=' .. (24 - 2 * lev) .. '>' .. s .. '</size></b>'
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
