alter table "METHOD_FILL_SIZE_HOLES"
	add constraint "SZ_CON_MDL_MTH_FILL_SZ_HLS_FK1" foreign key (
		"SIZE_CONSTRAINT_RID")
	 references "SIZE_CONSTRAINT_MODEL" (
		"SIZE_CONSTRAINT_RID")  

go
