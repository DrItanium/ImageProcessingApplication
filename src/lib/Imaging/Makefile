name := Libraries.Imaging.dll
db_name := $(name).mdb
thisdir := .
cmd_library := -t:library
cmd_out := -out:$(name)
cmd_compiler := dmcs
sources := *.cs 
libs := -r:System.Data.dll -r:System.Drawing.dll -r:System.Windows.Forms.dll

build: 
	dmcs -unsafe -optimize $(libs) $(cmd_library) $(cmd_out) $(sources)
debug:
	dmcs -unsafe -debug $(libs) $(cmd_library) $(cmd_out) $(sources)

install: build
	cp $(name) ../../../bin/

install-debug: debug
	cp $(name) ../../../bin/
	cp $(db_name) ../../../bin/

clean:
	-rm -f $(name) $(db_name)

