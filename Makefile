HERE ?= $(shell pwd -L)
STEAM_APP_ID ?= 294100
STEAM_FILE_ID ?= 2589526141

MOD_NAME ?= CentralizedClimateControl
VERSION ?= $(shell git describe --always | sed -e 's/-g.*//')
RELEASE_TYPE ?= Release
DIST_DIR ?= dist
OUTPUT_DIR ?= $(DIST_DIR)/$(MOD_NAME)

MD_CHANGELOG = CHANGELOG.md
RELEASE_CHANGELOG = dist/CHANGELOG.md
ABOUT = About/About.xml
MANIFEST = About/Manifest.xml
MODSYNC = About/ModSync.xml
UPDATEDEFS = 1.3/Defs/UpdateFeatureDefs/UpdateFeatures.xml

PACKAGE = $(DIST_DIR)/$(MOD_NAME).zip
WORKSHOP_META = $(DIST_DIR)/metadata.vdf

SLN_FILE = $(MOD_NAME).sln
CS_SOURCES = $(shell find Source -name "*.cs*")
ASSEMBLY = 1.3/Assemblies/CentralizedClimateControl.dll

DIST_SOURCES = $(sort \
	$(ASSEMBLY) $(UPDATEDEFS) \
	$(shell find 1.3 About Textures -type f) \
	$(wildcard *.md) \
)

DIST_DESTS = $(addprefix $(OUTPUT_DIR)/,$(DIST_SOURCES))
DIST_DIRS = $(sort $(foreach dest,$(DIST_DESTS),$(dir $(dest))))

DOTNET = dotnet.exe
DOTNET_BUILD_ARGS = --nologo --no-restore --configuration=$(RELEASE_TYPE) "-p:Version=$(VERSION)" "./$(SLN_FILE)"
DOTNET_FORMAT_ARGS = --no-restore -wsa info $(SLN_FILE)

PANDOC = pandoc

ZIPTOOL := $(shell which zip 2>/dev/null)
ifeq "$(ZIPTOOL)" ""
override ZIPTOOL := 7z.exe
ZIPFLAGS = a -r -mx9
else
ZIPFLAGS = -r -9
endif

PRETTIER = npx -q prettier
NPM = npm

VERSION_MARKER=$(DIST_DIR)/.version

-include Makefile.local

.PRECIOUS: $(ABOUT) $(MANIFEST) $(MODSYNC)

.PHONY: all clean cleaner package distrib build version lint format release quicktest publish prepare-publish

all: build

clean:
	rm -rf $(DIST_DIR) $(ASSEMBLY) $(PACKAGE) $(TXT_CHANGELOG) $(UPDATEDEFS) $(WORKSHOP_META)

cleaner: clean
	rm -rf node_modules obj

publish: prepare-publish
	steamcmd '+login $(STEAM_USERNAME) $(STEAM_PASSWORD)' "+workshop_build_item `pwd -L`/$(WORKSHOP_META)" '+quit'

prepare-publish: distrib $(WORKSHOP_META)

package: distrib $(PACKAGE)

distrib: build $(DIST_DESTS)

build: version $(ASSEMBLY) $(ABOUT)

version: $(MANIFEST) $(MODSYNC) $(RELEASE_CHANGELOG) $(UPDATEDEFS) | $(VERSION_MARKER)

$(MANIFEST) $(MODSYNC): $(VERSION_MARKER) | node_modules
	sed -i -e '/<[vV]ersion>/s/>.*</>$(VERSION)</' $@
	$(PRETTIER) --write $@

$(WORKSHOP_META): $(VERSION_MARKER) .scripts/workshop-meta | $(DIST_DIR)
	.scripts/workshop-meta "$(STEAM_APP_ID)" "$(STEAM_FILE_ID)" "$(HERE)/$(OUTPUT_DIR)" "$(VERSION)" >$@

$(ASSEMBLY): $(SLN_FILE) $(CS_SOURCES) $(VERSION_MARKER) | obj
	mkdir -p $(@D)
	"$(DOTNET)" build $(DOTNET_BUILD_ARGS)

$(RELEASE_CHANGELOG): $(MD_CHANGELOG) $(VERSION_MARKER) .scripts/changelog | $(DIST_DIR)
	.scripts/changelog $(VERSION) > $@

ifneq "$(file <$(VERSION_MARKER))" "$(VERSION)"
.PHONY: $(VERSION_MARKER)
endif

$(VERSION_MARKER): | $(DIST_DIR)
	echo -n "$(VERSION)" > $@

$(ABOUT): docs/description.md .scripts/generate-about | node_modules
	.scripts/generate-about $< > $@.2 && mv $@.2 $@ || rm $@.2
	$(PRETTIER) --write $@

obj:
	"$(DOTNET)" restore --locked-mode

$(UPDATEDEFS): $(MD_CHANGELOG) .pandoc/UpdateFeatureDefs.lua $(VERSION_MARKER) | node_modules
	mkdir -p $(@D)
	$(PANDOC) -t .pandoc/UpdateFeatureDefs.lua $< -o $@

.scripts/generate-about: .scripts/unityrtf $(ABOUT)
.scripts/workshop-meta: .scripts/bbcode
.scripts/unityrtf: .pandoc/UnityRichTextFormat.lua
.scripts/bbcode: .pandoc/SteamBBCode.lua
	touch $@

$(PACKAGE): $(DIST_DESTS)
	cd $(DIST_DIR) ; $(ZIPTOOL) $(ZIPFLAGS) ../$(PACKAGE) $(patsubst dist/%,%,$?)

define CP_template =
$(2): $(1) | $(dir $(2))
	cp $$? $$@

endef

$(foreach src,$(DIST_DESTS),$(eval $(call CP_template,$(patsubst $(OUTPUT_DIR)/%,%,$(src)),$(src))))

define MKDIR_template =
$(1):
	mkdir -p $$@

endef

$(foreach dir,$(DIST_DIRS),$(eval $(call MKDIR_template,$(dir))))

$(DIST_DIR):
	mkdir -p $@

format: | node_modules
	$(PRETTIER) --write .
	"$(DOTNET)" tool run dotnet-format $(DOTNET_FORMAT_ARGS)

lint: | node_modules
	$(PRETTIER) --check .
	"$(DOTNET)" tool run dotnet-format --check $(DOTNET_FORMAT_ARGS)

node_modules: package-lock.json
	$(NPM) install
	@touch $@

release: VERSION = $(shell cl suggest)
release: | node_modules
	cl release --suggest --yes
	$(PRETTIER) --write $(MD_CHANGELOG)
	$(MAKE) version VERSION=$(VERSION)
	git add $(MD_CHANGELOG) $(MANIFEST) $(MODSYNC)
	git commit -m "Release $(VERSION)"
	git tag $(VERSION) -m "Release $(VERSION)"

quicktest: build
	cd "$(RIMWORLD_PATH)"; exec ./RimWorldWin64.exe -savedatafolder=QuickTestSaveData
