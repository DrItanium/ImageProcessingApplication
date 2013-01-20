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



#test: install
#.PHONY: test 
#test-debug: install-debug
#.PHONY: test-debug
#cortex: homeworkform 
#.PHONY: cortex
#cortex-debug: homeworkform-debug 
#.PHONY: cortex-debug
#install: cortex copy
#.PHONY: install
#install-debug: cortex-debug copy-debug
#.PHONY: install-debug
#
#collections: extensions 
#	cd $(root)/TheoreticalIndustries/Libraries/Collections && $(MAKE) 
#
#imaging: 
#	cd $(root)/TheoreticalIndustries/Libraries/Imaging && $(MAKE)
#
#starlight: 
#	cd $(root)/TheoreticalIndustries/Libraries/Starlight && $(MAKE) 
#
#tycho: lexical 
#	cd $(root)/TheoreticalIndustries/Libraries/Tycho && $(MAKE) 
#
#parsing: tycho starlight  
#	cd $(root)/TheoreticalIndustries/Libraries/Parsing && $(MAKE)
#
#messaging:
#	cd $(root)/TheoreticalIndustries/Libraries/Messaging && $(MAKE) 
#
#filter: collections extensions lexical starlight tycho parsing messaging plugin 
#	cd $(root)/TheoreticalIndustries/Libraries/Filter && $(MAKE) 
#
#plugin: messaging 
#	cd $(root)/TheoreticalIndustries/Frameworks/Plugin && $(MAKE) 
#
#homeworkform: filter imaging
#	cd $(root)/CS555.HomeworkForm/ && $(MAKE)
#
#extensions: 
#	cd $(root)/TheoreticalIndustries/Libraries/Extensions && $(MAKE)
#
#lexical: collections 
#	cd $(root)/TheoreticalIndustries/Libraries/LexicalAnalysis && $(MAKE)
#
#copy: 
#	cd $(root)/TheoreticalIndustries/Libraries/LexicalAnalysis/ && cp *.dll $(root)/bin/; \
#	cd $(root)/TheoreticalIndustries/Libraries/Extensions/ && cp *.dll $(root)/bin/; \
#	cd $(root)/TheoreticalIndustries/Frameworks/Plugin/ && cp *.dll $(root)/bin/; \
#	cd $(root)/TheoreticalIndustries/Libraries/Collections/ && cp *.dll $(root)/bin/; \
#	cd $(root)/TheoreticalIndustries/Libraries/Imaging/ && cp *.dll $(root)/bin/; \
#	cd $(root)/TheoreticalIndustries/Libraries/Starlight/ && cp *.dll $(root)/bin/; \
#	cd $(root)/TheoreticalIndustries/Libraries/Tycho/ && cp *.dll $(root)/bin/; \
#	cd $(root)/TheoreticalIndustries/Libraries/Messaging/ && cp *.dll $(root)/bin/; \
#	cd $(root)/TheoreticalIndustries/Libraries/Filter/ && cp *.dll $(root)/bin/; \
#	cd $(root)/TheoreticalIndustries/Libraries/Parsing/ && cp *.dll $(root)/bin/; \
#	cd $(root)/CS555.HomeworkForm/ && cp *.exe $(root)/bin/; 
#
## Begin Debug Functions
#collections-debug: extensions-debug 
#	cd $(root)/TheoreticalIndustries/Libraries/Collections; \
#	make debug; \
#	cp *.dll $(root)/bin/; \
#	cp *.mdb $(root)/bin/; \
#	cd $(root)
#
#imaging-debug: 
#	cd $(root)/TheoreticalIndustries/Libraries/Imaging; \
#	make debug; \
#	cp *.dll $(root)/bin/; \
#	cp *.mdb $(root)/bin/; \
#	cd $(root)
#
#starlight-debug:
#	cd $(root)/TheoreticalIndustries/Libraries/Starlight && $(MAKE) debug
#
#tycho-debug: collections-debug lexical-debug
#	cd $(root)/TheoreticalIndustries/Libraries/Tycho && $(MAKE) debug
#parsing-debug: tycho-debug starlight-debug 
#	cd $(root)/TheoreticalIndustries/Libraries/Parsing && $(MAKE) debug
#messaging-debug:
#	cd $(root)/TheoreticalIndustries/Libraries/Messaging && $(MAKE) debug
#filter-debug: parsing-debug messaging-debug plugin-debug
#	cd $(root)/TheoreticalIndustries/Libraries/Filter && $(MAKE) debug 
#plugin-debug: messaging-debug
#	cd $(root)/TheoreticalIndustries/Frameworks/Plugin && $(MAKE) debug 
#homeworkform-debug: filter-debug imaging-debug
#	cd $(root)/CS555.HomeworkForm/ && $(MAKE) debug
#extensions-debug: 
#	cd $(root)/TheoreticalIndustries/Libraries/Extensions && $(MAKE) debug
#lexical-debug: collections-debug
#	cd $(root)/TheoreticalIndustries/Libraries/LexicalAnalysis && $(MAKE) debug
#copy-debug: 
#	cd $(root)/TheoreticalIndustries/Libraries/LexicalAnalysis/ && cp *.dll *.mdb $(root)/bin/; \
#	cd $(root)/TheoreticalIndustries/Libraries/Extensions/ && cp *.dll *.mdb $(root)/bin/; \
#	cd $(root)/TheoreticalIndustries/Frameworks/Plugin/ && cp *.dll *.mdb $(root)/bin/; \
#	cd $(root)/TheoreticalIndustries/Libraries/Collections/ && cp *.dll *.mdb $(root)/bin/; \
#	cd $(root)/TheoreticalIndustries/Libraries/Imaging/ && cp *.dll *.mdb $(root)/bin/; \
#	cd $(root)/TheoreticalIndustries/Libraries/Starlight/ && cp *.dll *.mdb $(root)/bin/; \
#	cd $(root)/TheoreticalIndustries/Libraries/Tycho/ && cp *.dll *.mdb $(root)/bin/; \
#	cd $(root)/TheoreticalIndustries/Libraries/Messaging/ && cp *.dll *.mdb $(root)/bin/; \
#	cd $(root)/TheoreticalIndustries/Libraries/Filter/ && cp *.dll *.mdb $(root)/bin/; \
#	cd $(root)/TheoreticalIndustries/Libraries/Parsing/ && cp *.dll *.mdb $(root)/bin/; \
#	cd $(root)/CS555.HomeworkForm/ && cp *.exe *.mdb $(root)/bin/; 
#clean:
#	cd $(root)/bin; \
#	rm -f *.exe *.dll *.mdb; \
#	cd $(root)/TheoreticalIndustries/Libraries/LexicalAnalysis && $(MAKE) clean; \
#	cd $(root)/TheoreticalIndustries/Libraries/Extensions && $(MAKE) clean; \
#	cd $(root)/TheoreticalIndustries/Libraries/Collections && $(MAKE) clean; \
#	cd $(root)/TheoreticalIndustries/Libraries/Filter && $(MAKE) clean; \
#	cd $(root)/TheoreticalIndustries/Libraries/Tycho && $(MAKE) clean; \
#	cd $(root)/TheoreticalIndustries/Libraries/Starlight && $(MAKE) clean; \
#	cd $(root)/TheoreticalIndustries/Libraries/Messaging && $(MAKE) clean; \
#	cd $(root)/TheoreticalIndustries/Libraries/Parsing && $(MAKE) clean; \
#	cd $(root)/TheoreticalIndustries/Libraries/Imaging && $(MAKE) clean; \
#	cd $(root)/TheoreticalIndustries/Frameworks/Plugin && $(MAKE) clean; \
#	cd $(root)/CS555.HomeworkForm && $(MAKE) clean 
#
#homework1:
#	cd $(root)/Homework1 && $(MAKE);
#homework1-copy: homework1
#	cp $(root)/Homework1/*.dll $(root)/bin/
#
#homework1-debug: 
#	cd $(root)/Homework1 && $(MAKE) debug
#homework1-copy-debug: homework1-debug
#	cp $(root)/Homework1/*.dll $(root)/Homework1/*.mdb $(root)/bin/
#
#homework2:
#	cd $(root)/Homework2 && $(MAKE);
#homework2-copy: homework2
#	cp $(root)/Homework2/*.dll $(root)/bin/
#
#homework2-debug: 
#	cd $(root)/Homework2 && $(MAKE) debug
#homework2-copy-debug: homework2-debug
#	cp $(root)/Homework2/*.dll $(root)/Homework2/*.mdb $(root)/bin/
