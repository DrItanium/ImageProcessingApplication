name := Libraries.Messaging.dll
db_name := $(name).mdb
thisdir := .
cmd_library := -t:library
cmd_out := -out:$(name)
cmd_compiler := dmcs
sources := *.cs 

build: $(sources)
	$(cmd_compiler) -optimize $(cmd_library) $(cmd_out) $(sources)
debug: $(sources)
	$(cmd_compiler) -debug $(cmd_library) $(cmd_out) $(sources)

install: build
	cp $(name) ../../../bin/

install-debug: debug
	cp $(name) ../../../bin/
	cp $(db_name) ../../../bin/

clean:
	-rm -f $(name) $(db_name)

