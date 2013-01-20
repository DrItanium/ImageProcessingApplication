root := $(CURDIR)
lib:=$(root)/lib
main:=$(root)/main
plugins:=$(root)/plugins

install:
	cd $(lib) && $(MAKE) install
	cd $(main) && $(MAKE) install
	cd $(plugins) && $(MAKE) install

install-debug:
	cd $(lib) && $(MAKE) install-debug
	cd $(main) && $(MAKE) install-debug
	cd $(plugins) && $(MAKE) install-debug

clean: 
	cd $(lib) && $(MAKE) clean
	cd $(main) && $(MAKE) clean
	cd $(plugins) && $(MAKE) clean
