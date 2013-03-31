name := ImageProcessingApplication.exe
db_name := $(name).mdb
thisdir := .
cmd_library := -t:library
cmd_out := -out:$(name)
cmd_compiler := dmcs
sources := *.cs Properties/*.cs
def_mono := -d:MONO_NET
bin_dir := ../../bin
libs := -lib:../lib/
cmd_libraries := -r:Cortex.dll \
					  -r:System.Windows.Forms.dll\
					  -r:System.Drawing.dll\
					  -r:System.Data.dll
cmd_icons := -win32res:MainForm.resx \
             -win32res:Properties/Resources.resx 

build: $(sources)
	$(cmd_compiler) -optimize $(def_mono) $(cmd_out) $(libs) $(cmd_libraries) $(cmd_icons) $(sources)
debug: $(sources)
	$(cmd_compiler) -debug $(def_mono) $(cmd_out) $(libs) $(cmd_libraries) $(cmd_icons) $(sources)

install: build
	cp $(name) $(bin_dir)

install-debug: debug
	cp $(name) $(bin_dir)
	cp $(db_name) $(bin_dir) 

clean: 
	-rm -f $(name) $(db_name) 
