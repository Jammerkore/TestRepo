alter table "METHOD_COPY_GROUP_LEVEL"
	add constraint "SGL_METH_COPY_GL_FK1" foreign key (
		"SGL_RID")
	 references "STORE_GROUP_LEVEL" (
		"SGL_RID")  

go
