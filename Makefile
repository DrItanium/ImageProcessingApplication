#Written by Joshua Scoggins
root:=$(CURDIR)
src:=$(root)/src
bin:=$(root)/bin

all:
	cd $(src) && $(MAKE) install

debug-all:
	cd $(src) && $(MAKE) install-debug

clean: 
	cd $(src) && $(MAKE) clean
	-rm -f $(bin)/*.dll $(bin)/*.mdb $(bin)/*.exe
