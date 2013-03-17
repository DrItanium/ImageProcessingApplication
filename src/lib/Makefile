SUBDIRS = Collections Extensions Imaging LexicalAnalysis Messaging Operation Parsing Starlight Tycho Plugin Filter FileFormat

.PHONY: install install-debug clean subdirs $(SUBDIRS)



install:
	for d in $(SUBDIRS); do($(MAKE) -C $$d install ); done

install-debug:
	for d in $(SUBDIRS); do($(MAKE) -C $$d install-debug ); done

clean:
	for d in $(SUBDIRS); do($(MAKE) -C $$d clean ); done

subdirs: $(SUBDIRS)

$(SUBDIRS):
	$(MAKE) -C $@
