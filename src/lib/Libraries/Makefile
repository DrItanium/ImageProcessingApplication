root := $(CURDIR)
collections := $(root)/Collections
extensions := $(root)/Extensions
lib_filter := $(root)/Filter
imaging := $(root)/Imaging
lexical_analysis := $(root)/LexicalAnalysis
messaging := $(root)/Messaging
operation := $(root)/Operation
parsing := $(root)/Parsing
starlight := $(root)/Starlight
tycho := $(root)/Tycho
plugin := $(root)/../Frameworks/Plugin/

install:
	cd $(extensions) && $(MAKE) install
	cd $(collections) && $(MAKE) install
	cd $(lexical_analysis) && $(MAKE) install
	cd $(messaging) && $(MAKE) install
	cd $(starlight) && $(MAKE) install
	cd $(imaging) && $(MAKE) install
	cd $(tycho) && $(MAKE) install
	cd $(parsing) && $(MAKE) install
	cd $(plugin) && $(MAKE) install
	cd $(lib_filter) && $(MAKE) install

install-debug:
	cd $(extensions) && $(MAKE) install-debug
	cd $(collections) && $(MAKE) install-debug
	cd $(lexical_analysis) && $(MAKE) install-debug
	cd $(messaging) && $(MAKE) install-debug
	cd $(starlight) && $(MAKE) install-debug
	cd $(imaging) && $(MAKE) install-debug
	cd $(tycho) && $(MAKE) install-debug
	cd $(parsing) && $(MAKE) install-debug
	cd $(plugin) && $(MAKE) install-debug
	cd $(lib_filter) && $(MAKE) install-debug

clean:
	cd $(extensions) && $(MAKE) clean
	cd $(collections) && $(MAKE) clean
	cd $(lexical_analysis) && $(MAKE) clean
	cd $(messaging) && $(MAKE) clean
	cd $(starlight) && $(MAKE) clean
	cd $(imaging) && $(MAKE) clean
	cd $(tycho) && $(MAKE) clean
	cd $(parsing) && $(MAKE) clean
	cd $(plugin) && $(MAKE) clean
	cd $(lib_filter) && $(MAKE) clean
