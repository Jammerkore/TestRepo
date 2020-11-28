/* Add foreign key constraints to table "METHOD_COPY_BASIS_DETAIL".                           */
alter table "METHOD_COPY_BASIS_DETAIL"
	add constraint "METH_COPY_GL_METH_CBD_FK1" foreign key (
		"METHOD_RID",
		"SGL_RID")
	 references "METHOD_COPY_GROUP_LEVEL" (
		"METHOD_RID",
		"SGL_RID")  

go
