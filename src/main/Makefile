name := ImageProcessingApplication.exe
db_name := $(name).mdb
thisdir := .
cmd_library := -t:library
cmd_out := -out:$(name)
cmd_compiler := dmcs
sources := *.cs Properties/*.cs
def_mono := -d:MONO_NET
bin_dir := ../../bin
libs := -lib:../lib/Plugin/ \
        -lib:../lib/Collections/ \
        -lib:../lib/Extensions/ \
        -lib:../lib/Filter/ \
        -lib:../lib/Imaging/ \
        -lib:../lib/LexicalAnalysis/ \
        -lib:../lib/Messaging/ \
        -lib:../lib/Parsing/ \
        -lib:../lib/Starlight/ \
        -lib:../lib/Tycho/ \
		  -lib:../lib/FileFormat/
cmd_libraries := -r:Libraries.Collections.dll\
					  -r:Libraries.Extensions.dll\
					  -r:Libraries.Filter.dll\
					  -r:Libraries.Imaging.dll\
					  -r:Libraries.LexicalAnalysis.dll\
					  -r:Frameworks.Plugin.dll\
					  -r:Libraries.Messaging.dll\
					  -r:Libraries.Parsing.dll\
					  -r:Libraries.Starlight.dll\
					  -r:Libraries.Tycho.dll\
					  -r:Libraries.FileFormat.dll\
					  -r:System.Windows.Forms.dll\
					  -r:System.Drawing.dll\
					  -r:System.Data.dll
cmd_icons := -win32res:MainForm.resx -win32res:Properties/Resources.resx 

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
