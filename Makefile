SHELL := $(shell which bash)
MAKEFLAGS += --no-builtin-rules --no-builtin-variables --no-print-directory

-include Makefile.local

BUILD_DIR ?= .
VERSION ?= $(shell git describe --always | sed -e 's/-g.*//;s/[^0-9][^0-9]*/./g')

export SHELL BUILD_DIR VERSION

.PHONY: all clean cleaner build package publish prepare-publish lint format quicktest release

all: build

build clean cleaner::
	$(MAKE) -f .build/Makefile.build $@

package clean::
	$(MAKE) -f .build/Makefile.package "BUILD_DIR=$(BUILD_DIR)/dist" $@

clean publish prepare-publish::
	$(MAKE) -f .build/Makefile.publish "BUILD_DIR=$(BUILD_DIR)/dist" $@

quicktest:: build
lint format quicktest cleaner release::
	$(MAKE) -f .build/Makefile.dev $@

clean::
	rm -rf "$(BUILD_DIR)/dist"
