-include Makefile.local

BUILD_DIR ?= .
VERSION ?= $(shell git describe --always | sed -e 's/-g.*//')

export BUILD_DIR VERSION

.PHONY: all clean cleaner build package publish prepare-publish lint format quicktest

all: build

build clean cleaner::
	$(MAKE) -f .build/Makefile.build $@

package clean::
	$(MAKE) -f .build/Makefile.package $@

clean publish prepare-publish::
	$(MAKE) -f .build/Makefile.publish $@

quicktest:: build
lint format quicktest cleaner::
	$(MAKE) -f .build/Makefile.dev $@
