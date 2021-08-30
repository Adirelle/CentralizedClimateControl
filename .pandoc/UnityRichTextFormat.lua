local function encloser(tag)
  return function(s)
    return "<" .. tag .. ">" .. s .. "</" .. tag .. ">"
  end
end

local function empty()
  return ""
end

local function const(s)
  return function()
    return s
  end
end

local function id(s)
  return s
end

Blocksep = const("\n\n")
Space = const(" ")
SoftBreak = empty
LineBreak = const("\n")
HorizontalRule = empty
Emph = encloser("i")
Strong = encloser("b")

Str = id
Para = id
Plain = id
Doc = id

function SingleQuoted(s)
  return "'" .. s .. "'"
end

function DoubleQuoted(s)
  return '"' .. s .. '"'
end

function Link(s, src)
  return s .. " (" .. src .. ")"
end

function Header(lev, s)
    return '<b><size=' .. (24 - 2 * lev) .. '>' .. s .. '</size></b>'
end

function BulletList(items)
  local buffer = {}
  for _, item in pairs(items) do
    table.insert(buffer, "* " .. item)
  end
  return table.concat(buffer, "\n")
end

function OrderedList(items)
  local buffer = {}
  for i, item in ipairs(items) do
    table.insert(buffer, (i+1) .. ". " .. item)
  end
  return table.concat(buffer, "\n")
end

function DefinitionList(items)
  local buffer = {}
  for _,item in pairs(items) do
    local k, v = next(item)
    table.insert(buffer, k .. ": " .. v)
  end
  return table.concat(buffer, "\n")
end

function Table(caption, aligns, widths, headers, rows)
  io.stderr:write("Table are no supported")
end

local meta = {}
meta.__index =
  function(t, key)
    io.stderr:write(string.format("WARNING: Undefined function '%s'\n",key))
    t[key] = id
    return id
  end
setmetatable(_G, meta)
