/* Add foreign key constraints to table "METHOD_COPY_GROUP_LEVEL".                            */
alter table "METHOD_COPY_GROUP_LEVEL"
	add constraint "METH_COPY_FC_METH_COPY_GL_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD_COPY_FORECAST" (
		"METHOD_RID")  

go
