alter table "METHOD_SIZE_NEED"
	add constraint "SZ_CON_MDL_MTHD_SZ_NEED_FK1" foreign key (
		"SIZE_CONSTRAINT_RID")
	 references "SIZE_CONSTRAINT_MODEL" (
		"SIZE_CONSTRAINT_RID")  

go
