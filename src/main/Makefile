name := CS555.HomeworkForm.exe
thisdir := .
cmd_library := -t:library
cmd_out := -out:$(name)
cmd_compiler := dmcs
sources := *.cs 
def_mono := -d:MONO_NET
libs := -lib:../lib/Frameworks/Plugin/ \
        -lib:../lib/Libraries/Collections/ \
        -lib:../lib/Libraries/Extensions/ \
        -lib:../lib/Libraries/Filter/ \
        -lib:../lib/Libraries/Imaging/ \
        -lib:../lib/Libraries/LexicalAnalysis/ \
        -lib:../lib/Libraries/Messaging/ \
        -lib:../lib/Libraries/Parsing/ \
        -lib:../lib/Libraries/Starlight/ \
        -lib:../lib/Libraries/Tycho/ 
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
					  -r:System.Windows.Forms.dll\
					  -r:System.Drawing.dll\
					  -r:System.Data.dll
cmd_icons := -win32res:Form1.resx -win32res:Properties/Resources.resx 

build: $(sources)
	dmcs -optimize $(def_mono) $(cmd_out) $(libs) $(cmd_libraries) $(cmd_icons) $(sources)
debug: $(sources)
	dmcs -debug $(def_mono) $(cmd_out) $(libs) $(cmd_libraries) $(cmd_icons) $(sources)
.PHONY: clean
clean:
	-rm -f *.dll *.mdb
