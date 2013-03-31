name := FileFormats.Divide.dll
db_name := $(name).mdb
bin_dir := ../../../bin
lib_dir := ../../lib
thisdir := .
cmd_library := -t:library
cmd_out := -out:$(name)
cmd_compiler := dmcs
sources := *.cs 
cmd_include := -r:
cmd_libraries := -r:Libraries.FileFormat.dll\
					  -r:Libraries.Imaging.dll\
					  -r:Frameworks.Plugin.dll\
					  -r:Libraries.Messaging.dll\
					  -r:System.Windows.Forms.dll\
					  -r:System.Drawing.dll\
					  -r:System.Data.dll
options := -lib:$(lib_dir)/FileFormat/ \
			  -lib:$(lib_dir)/Imaging/ \
			  -lib:$(lib_dir)/Messaging/ \
			  -lib:$(lib_dir)/Plugin/ 
build: 
	dmcs -optimize $(cmd_library) $(cmd_out) $(options) $(cmd_libraries) $(sources)
debug: 
	dmcs -debug $(cmd_library) $(cmd_out) $(options) $(cmd_libraries) $(sources)

install: build
	cp $(name) $(bin_dir)

install-debug: debug
	cp $(name) $(bin_dir)
	cp $(db_name) $(bin_dir) 

clean: 
	-rm -f $(name) $(db_name) 
