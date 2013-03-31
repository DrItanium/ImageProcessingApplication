name = Cortex.dll
cmd_out := -out:$(name)
dname = $(name).mdb
cmd_library := -t:library
sources = Extensions/*.cs \
			 Collections/*.cs \
			 LexicalAnalysis/*.cs \
			 Messaging/*.cs \
			 Starlight/*.cs \
			 Imaging/*.cs \
			 Tycho/*.cs \
			 Tycho/Metadata/*.cs \
			 Parsing/*.cs \
			 Plugin/*.cs \
			 Filter/*.cs \
			 FileFormat/*.cs \
			 Operation/*.cs 
options := -define:STRING_EXTENSIONS,CURRYING,TESTING_EXTENSIONS,LINQ,MATH_FORMULA,POLYNOMIAL_APPROXIMATION \
           -r:System.Windows.Forms.dll \
			  -r:System.Drawing.dll \
			  -r:System.Data.dll \
			  -win32res:Filter/Resources/FilterdialogForm.resx \
			  -unsafe
compiler = dmcs

.PHONY: install install-debug clean 

build: 
	$(compiler) -optimize $(options) $(cmd_library) $(cmd_out) $(sources)

debug: 
	$(compiler) -debug $(options) $(cmd_library) $(cmd_out) $(sources)

stats: 
	$(compiler) -d:GATHERING_STATS $(options) $(cmd_library) $(cmd_out) $(sources)

stats-debug: 
	$(compiler) -debug -d:GATHERING_STATS $(options) $(cmd_library) $(cmd_out) $(sources)
	

install: build
	cp $(name) ../../bin/

install-debug: debug
	cp $(name) ../../bin/
	cp $(dname) ../../bin/

clean:
	-rm -f $(name) $(dname)
