root:=$(CURDIR)
hw1:=$(root)/Homework1
hw2:=$(root)/Homework2
term_paper := $(root)/TermPaper

install:
	cd $(hw1) && $(MAKE) install
	cd $(hw2) && $(MAKE) install
	cd $(term_paper) && $(MAKE) install

install-debug:
	cd $(hw1) && $(MAKE) install-debug
	cd $(hw2) && $(MAKE) install-debug
	cd $(term_paper) && $(MAKE) install-debug

clean:
	cd $(hw1) && $(MAKE) clean
	cd $(hw2) && $(MAKE) clean
	cd $(term_paper) && $(MAKE) clean
