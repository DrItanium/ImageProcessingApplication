#Written by Joshua Scoggins
root:=$(CURDIR)
src:=$(root)/src
bin:=$(root)/bin
mkdir_cmd = mkdir -p

.PHONY: directories

all: directories
	cd $(src) && $(MAKE) install

debug: directories
	cd $(src) && $(MAKE) install-debug

clean: 
	cd $(src) && $(MAKE) clean
	-rm -f $(bin)/*.dll $(bin)/*.mdb $(bin)/*.exe
	-rmdir $(bin)

directories:
	$(mkdir_cmd) $(bin) 
