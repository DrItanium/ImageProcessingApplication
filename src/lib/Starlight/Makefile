name := Libraries.Starlight.dll
db_name := $(name).mdb
thisdir := .
cmd_library := -t:library
cmd_out := -out:$(name)
cmd_compiler := dmcs
sources := *.cs 
#options := -d:GATHERING_STATS
result := $(name)

build: 
	$(cmd_compiler) -optimize $(cmd_library) $(cmd_out) $(sources)

debug:
	$(cmd_compiler) -debug $(cmd_library) $(cmd_out) $(sources)

#stats: 
#	$(cmd_compiler) $(options) $(cmd_library) $(cmd_out) $(sources)

install: build
	cp $(name) ../../../bin/

install-debug: debug
	cp $(name) ../../../bin/
	cp $(db_name) ../../../bin/

clean:
	-rm -f $(name) $(db_name)


