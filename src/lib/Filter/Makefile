name := Libraries.Filter.dll
db_name := $(name).mdb
thisdir := .
cmd_library := -t:library
cmd_out := -out:$(name)
cmd_compiler := dmcs
sources := *.cs 
lib_dir := -lib:../LexicalAnalysis/ \
           -lib:../Collections/ \
           -lib:../Extensions/ \
			  -lib:../Plugin/ \
			  -lib:../Starlight/ \
			  -lib:../Tycho/ \
			  -lib:../LexicalAnalysis/ \
			  -lib:../Parsing/ \
			  -lib:../Messaging/
options := -r:Libraries.Collections.dll \
           -r:Libraries.LexicalAnalysis.dll \
			  -r:Libraries.Extensions.dll \
			  -r:Libraries.Collections.dll \
			  -r:Frameworks.Plugin.dll \
			  -r:Libraries.Starlight.dll \
			  -r:Libraries.Tycho.dll \
			  -r:Libraries.Parsing.dll \
			  -r:Libraries.Messaging.dll \
			  -r:System.Windows.Forms.dll \
			  -r:System.Drawing.dll \
			  -r:System.Data.dll \
			  -win32res:Resources/FilterdialogForm.resx
result := $(name)

build: 
	$(cmd_compiler) -optimize $(options) $(lib_dir) $(cmd_library) $(cmd_out) $(sources)

debug: 
	$(cmd_compiler) -debug $(options) $(lib_dir) $(cmd_library) $(cmd_out) $(sources)

install: build
	cp $(name) ../../../bin/

install-debug: debug
	cp $(name) ../../../bin/
	cp $(db_name) ../../../bin/

clean:
	-rm -f $(name) $(db_name)

