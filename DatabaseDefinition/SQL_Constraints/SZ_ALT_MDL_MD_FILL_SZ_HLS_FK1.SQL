alter table "METHOD_FILL_SIZE_HOLES"
	add constraint "SZ_ALT_MDL_MD_FILL_SZ_HLS_FK1" foreign key (
		"SIZE_ALTERNATE_RID")
	 references "SIZE_ALTERNATE_MODEL" (
		"SIZE_ALTERNATE_RID")  

go
