base_name := Frameworks.Plugin
lib_name := $(base_name).dll
db_name := $(base_name).mdb
name := Frameworks.Plugin.dll 
thisdir := .
cmd_library := -t:library
cmd_out := -out:$(name)
cmd_compiler := dmcs
sources := *.cs 
lib_dir := -lib:../../Libraries/Messaging
options := -r:Libraries.Messaging.dll
build: $(sources)
	dmcs -optimize $(options) $(lib_dir) $(cmd_library) $(cmd_out) $(sources)
debug: $(sources)
	dmcs -debug $(options) $(lib_dir) $(cmd_library) $(cmd_out) $(sources)

install: build
	cp $(lib_name) ../../../bin/	

install-debug: install-debug
	cp $(lib_name) ../../../bin/
	cp $(db_name) ../../../bin/

clean: 
	-rm -f *.dll *.mdb
