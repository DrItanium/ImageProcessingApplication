name := Libraries.Tycho.dll
db_name := $(name).mdb
thisdir := .
cmd_library := -t:library
cmd_out := -out:$(name)
cmd_compiler := dmcs
sources := *.cs Metadata/*.cs
lib_dir := -lib:../LexicalAnalysis/ -lib:../Collections/ -lib:../Extensions/
options := -r:Libraries.Collections.dll -r:Libraries.LexicalAnalysis.dll -r:Libraries.Extensions.dll
result := $(name)

build: 
	dmcs -optimize $(options) $(lib_dir) $(cmd_library) $(cmd_out) $(sources)
debug: 
	dmcs -debug $(options) $(lib_dir) $(cmd_library) $(cmd_out) $(sources)

install: build
	cp $(name) ../../../../bin/

install-debug: debug
	cp $(name) ../../../../bin/
	cp $(db_name) ../../../../bin/

clean:
	-rm -f $(name) $(db_name)


