/* Add foreign key constraints to table "SIZE_ALT_PRIMARY_SIZE".                              */
alter table "SIZE_ALT_PRIMARY_SIZE"
	add constraint "SZ_ALT_MDL_SZ_ALT_PRI_SZ_FK1" foreign key (
		"SIZE_ALTERNATE_RID")
	 references "SIZE_ALTERNATE_MODEL" (
		"SIZE_ALTERNATE_RID")  

go
