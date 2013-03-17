name := Libraries.Extensions.dll
db_name := $(name).mdb
thisdir := .
cmd_library := -t:library
cmd_out := -out:$(name)
cmd_compiler := dmcs
sources := *.cs 
options := -define:STRING_EXTENSIONS,CURRYING,TESTING_EXTENSIONS,LINQ,MATH_FORMULA,POLYNOMIAL_APPROXIMATION 

build: 
	dmcs -optimize $(options) $(cmd_library) $(cmd_out) $(sources)
debug: 
	dmcs -debug $(options) $(cmd_library) $(cmd_out) $(sources)

install: build
	cp $(name) ../../../bin/

install-debug: debug
	cp $(name) ../../../bin/
	cp $(db_name) ../../../bin/

clean:
	-rm -f $(name) $(db_name)
