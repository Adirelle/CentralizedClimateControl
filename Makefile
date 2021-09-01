-include Makefile.local

HERE ?= $(shell pwd -L)
SRC_DIR ?= .
BUILD_DIR ?= .
VERSION ?= $(shell git describe --always | sed -e 's/-g.*//')

STEAM_APP_ID ?= 294100
STEAM_FILE_ID ?= 2589526141

export SRC_DIR BUILD_DIR HERE VERSION

.PHONY: all clean cleaner package build lint format quicktest

all: build

build clean cleaner::
	$(MAKE) -f .build/Makefile.build $@

package clean::
	$(MAKE) -f .build/Makefile.package $@

quicktest:: build
lint format quicktest cleaner::
	$(MAKE) -f .build/Makefile.dev $@
