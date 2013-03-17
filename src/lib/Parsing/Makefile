name := Libraries.Parsing.dll
db_name := $(name).mdb
thisdir := .
cmd_library := -t:library
cmd_out := -out:$(name)
cmd_compiler := dmcs
sources := *.cs 
lib_dir := -lib:../LexicalAnalysis/ \
           -lib:../Collections/ \
           -lib:../Extensions/ \
			  -lib:../Starlight/ \
			  -lib:../Tycho/ 
options := -r:Libraries.Collections.dll \
           -r:Libraries.LexicalAnalysis.dll \
			  -r:Libraries.Extensions.dll \
			  -r:Libraries.Starlight.dll \
			  -r:Libraries.Tycho.dll
result := $(name)

build: 
	$(cmd_compiler) -optimize $(options) $(lib_dir) $(cmd_library) $(cmd_out) $(sources)
debug: 
	$(cmd_compiler) -debug $(options) $(lib_dir) $(cmd_library) $(cmd_out) $(sources)

#stats: $(sources)
#	$(cmd_compiler) -D:GATHERING_STATS $(options) $(lib_dir) $(cmd_library) $(cmd_out) $(sources)

install: build
	cp $(name) ../../../bin/

install-debug: debug
	cp $(name) ../../../bin/
	cp $(db_name) ../../../bin/

clean:
	-rm -f $(name) $(db_name)


