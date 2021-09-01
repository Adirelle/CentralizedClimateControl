local function noop(s)
  return s
end

local function const(s)
  return function()
    return s
  end
end

local function encloser(t)
  return function (s)
    return '[' .. t .. ']' .. s .. '[/' .. t .. ']';
  end
end

Blocksep = const("\n\n")
Space = const(" ")
SoftBreak = const("")
LineBrak = const("\n\n")
Emph = encloser("i")
Strong = encloser("b")
Strikeout = encloser("strike")
Code = encloser("code")
LineBlock = encloser("code")
CodeBlock = encloser("code")
BlockQuote = encloser("quote")

function RawInline(_, s)
  return s
end

function RawBlock(_, s)
  return s
end

function Link(s, src)
  return "[url=" .. src .. "]" .. s .. "[/url]"
end

function SingleQuoted(s)
  return "'" .. s .. "'"
end

function DoubleQuoted(s)
  return '"' .. s .. '"'
end

function Header(lev, s)
  return '[h' .. lev .. ']' .. s .. '[/h' .. lev .. ']'
end

function HorizontalRule()
  return "[hr][/hr]"
end

function BulletList(items)
  local buffer = {}
  for _, item in pairs(items) do
    table.insert(buffer, "[*]" .. item)
  end
  return "[list]\n" .. table.concat(buffer, "\n") .. "\n[/list]"
end

function OrderedList(items)
  local buffer = {}
  for _, item in pairs(items) do
    table.insert(buffer, "[*]" .. item)
  end
  return "[olist]\n" .. table.concat(buffer, "\n") .. "\n[/olist]"
end

function DefinitionList(items)
  local buffer = {}
  for _,item in pairs(items) do
    local k, v = next(item)
    table.insert(buffer, "[*][b]" .. k .. "[/b]: " .. table.concat(v, "\n"))
  end
  return "[list]\n" .. table.concat(buffer, "\n") .. "\n[/list]"
end

function Table(_, aligns, _, headers, rows)
  local buffer = {}
  local function add(s)
    table.insert(buffer, s)
  end
  add("[table]")

  local header_row = {}
  local empty_header = true
  for _, h in pairs(headers) do
    table.insert(header_row, '[th]' .. h .. '[/th]')
    empty_header = empty_header and h == ""
  end
  if not empty_header then
    add('[tr]')
    for _,cell in pairs(header_row) do
      add(cell)
    end
    add('[/tr]')
  end

  for _, row in pairs(rows) do
    add('[tr]')
    for _, cell in pairs(row) do
      add('[td]' .. cell .. '[/td]')
    end
    add('[/tr]')
  end

  add('[/table]')
  return table.concat(buffer,'\n')
end

setmetatable(_G, {
  __index = function(t, key)
    --io.stderr:write(string.format("WARNING: Undefined function '%s'\n",key))
    t[key] = noop
    return noop
  end
})
