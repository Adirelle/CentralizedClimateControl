VERSION ?= $(shell git describe --always)
MOD_NAME ?= CentralizedClimateControl
RELEASE_TYPE ?= Release
OUTPUT_DIR ?= dist/$(MOD_NAME)

TXT_CHANGELOG = About/Changelog.txt
ABOUT = About/About.xml
MANIFEST = About/Manifest.xml
MODSYNC = About/ModSync.xml
UPDATEDEFS = 1.3/Defs/UpdateFeatureDefs/UpdateFeatures.xml

PACKAGE = $(MOD_NAME).zip

SLN_FILE = $(MOD_NAME).sln
CS_SOURCES = $(shell find Source -name "*.cs*")
ASSEMBLY = 1.3/Assemblies/CentralizedClimateControl.dll

DIST_SOURCES = $(sort \
	$(ASSEMBLY) $(TXT_CHANGELOG) $(UPDATEDEFS) \
	$(shell find 1.3 About Textures -type f) \
	$(wildcard *.md) \
)

DIST_DESTS = $(addprefix $(OUTPUT_DIR)/,$(DIST_SOURCES))
DIST_DIRS = $(sort $(foreach dest,$(DIST_DESTS),$(dir $(dest))))

DOTNET = dotnet.exe
DOTNET_BUILD_ARGS = --nologo --no-restore --configuration=$(RELEASE_TYPE) "-p:Version=$(VERSION)" "./$(SLN_FILE)"
DOTNET_FORMAT_ARGS = --no-restore -wsa info $(SLN_FILE)

PANDOC = pandoc

PRETTIER = node_modules/.bin/prettier
NPM = $(shell which npm)

-include Makefile.local

.PHONY: all clean cleaner package dist build version lint format

all: package

clean:
	rm -rf $(OUTPUT_DIR) $(ASSEMBLY) $(PACKAGE) $(TXT_CHANGELOG) $(UPDATEDEFS)

cleaner: clean
	rm -rf node_modules obj

package: dist $(PACKAGE)

dist: build $(DIST_DESTS)

build: $(MANIFEST) $(MODSYNC) $(ASSEMBLY) $(TXT_CHANGELOG) $(UPDATEDEFS) $(ABOUT)

$(MANIFEST) $(MODSYNC): .git/HEAD
	sed -i -e '/<[vV]ersion>/s/>.*</>$(VERSION)</' $@

$(ABOUT): README.md
	.scripts/generate-About.sh > $@

$(TXT_CHANGELOG): CHANGELOG.md
	sed -e '/^## $(VERSION)/,/^## /!d;/^## /,+3d' $< >$@

$(ASSEMBLY): $(SLN_FILE) $(CS_SOURCES) | obj
	mkdir -p $(@D)
	"$(DOTNET)" build $(DOTNET_BUILD_ARGS)

obj:
	"$(DOTNET)" restore --locked-mode

$(UPDATEDEFS): CHANGELOG.md .pandoc/UpdateFeatureDefs.lua
	mkdir -p $(@D)
	$(PANDOC) -t .pandoc/UpdateFeatureDefs.lua $< -o $@

$(PACKAGE): $(DIST_DESTS)
	cd $(dir $(OUTPUT_DIR)) ; zip -r -9 ../$(PACKAGE) $(patsubst dist/%,%,$?)

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

format: $(PRETTIER)
	$(PRETTIER) --write .
	"$(DOTNET)" tool run dotnet-format $(DOTNET_FORMAT_ARGS)

lint: $(PRETTIER)
	$(PRETTIER) --check .
	"$(DOTNET)" tool run dotnet-format --check $(DOTNET_FORMAT_ARGS)

$(PRETTIER): package-lock.json
	$(NPM) install
	touch -r $< $@
