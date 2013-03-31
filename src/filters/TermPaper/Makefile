name := CS555.TermPaper.dll
db_name := $(name).mdb
thisdir := .
cmd_library := -t:library
root := $(CURDIR)
cmd_out := -out:$(name)
cmd_compiler := dmcs
sources := *.cs 
cmd_include := -r:
bin_dir := ../../../bin
lib_dir := ../../lib
libs := -lib:$(lib_dir)
cmd_libraries := -r:Cortex.dll \
					  -r:System.Windows.Forms.dll\
					  -r:System.Drawing.dll\
					  -r:System.Data.dll

build: 
	dmcs -optimize $(cmd_library) $(cmd_out) $(libs) $(cmd_libraries) $(sources)
debug: 
	dmcs -debug $(cmd_library) $(cmd_out) $(libs) $(cmd_libraries) $(sources)

install: build
	cp $(name) $(bin_dir)

install-debug: debug
	cp $(name) $(bin_dir)
	cp $(db_name) $(bin_dir) 

clean: 
	-rm -f $(name) $(db_name)
