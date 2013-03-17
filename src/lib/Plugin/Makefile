name := Frameworks.Plugin.dll
db_name := $(name).mdb
thisdir := .
cmd_library := -t:library
cmd_out := -out:$(name)
cmd_compiler := dmcs
sources := *.cs 
lib_dir := -lib:../Messaging
options := -r:Libraries.Messaging.dll
build: $(sources)
	dmcs -optimize $(options) $(lib_dir) $(cmd_library) $(cmd_out) $(sources)
debug: $(sources)
	dmcs -debug $(options) $(lib_dir) $(cmd_library) $(cmd_out) $(sources)

install: build
	cp $(name) ../../../bin/	

install-debug: debug
	cp $(name) ../../../bin/
	cp $(db_name) ../../../bin/

clean: 
	-rm -f $(name) $(db_name) 
