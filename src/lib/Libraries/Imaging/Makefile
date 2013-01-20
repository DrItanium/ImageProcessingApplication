.RECIPEPREFIX = >
name := Libraries.Imaging.dll 
thisdir := .
cmd_library := -t:library
cmd_out := -out:$(name)
cmd_compiler := dmcs
sources := *.cs 
libs := -r:System.Data.dll -r:System.Drawing.dll -r:System.Windows.Forms.dll

build: $(sources)
> dmcs -unsafe -optimize $(libs) $(cmd_library) $(cmd_out) $(sources)
debug:
> dmcs -unsafe -debug $(libs) $(cmd_library) $(cmd_out) $(sources)
.PHONY : clean
clean: 
> -rm -f *.dll *.mdb
