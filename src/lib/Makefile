root:=$(CURDIR)
frameworks:=$(root)/Frameworks
libraries:=$(root)/Libraries

install:
	cd $(libraries) && $(MAKE) install
	cd $(frameworks) && $(MAKE) install

install-debug:
	cd $(libraries) && $(MAKE) install-debug
	cd $(frameworks) && $(MAKE) install-debug

clean:
	cd $(libraries) && $(MAKE) clean
	cd $(frameworks) && $(MAKE) clean
