alter table "METHOD_SIZE_NEED"
	add constraint "SZ_ALT_MDL_MTHD_SZ_NEED_FK1" foreign key (
		"SIZE_ALTERNATE_RID")
	 references "SIZE_ALTERNATE_MODEL" (
		"SIZE_ALTERNATE_RID")  

go
