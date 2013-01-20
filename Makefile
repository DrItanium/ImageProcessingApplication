#Written by Joshua Scoggins
root:=$(CURDIR)
src:=$(root)/src
bin:=$(root)/bin
lib:=$(src)/lib
main:=$(src)/main
plugins:=$(src)/plugins

all:
	cd $(lib) && $(MAKE) install
	cd $(main) && $(MAKE) install
	cd $(plugins) && $(MAKE) install

debug-all:
	cd $(lib) && $(MAKE) install-debug
	cd $(main) && $(MAKE) install-debug
	cd $(plugins) && $(MAKE) install-debug

clean: 
	cd $(lib) && $(MAKE) clean
	cd $(main) && $(MAKE) clean
	cd $(plugins) && $(MAKE) clean
	-rm -f bin/*.dll bin/*.mdb bin/*.exe
