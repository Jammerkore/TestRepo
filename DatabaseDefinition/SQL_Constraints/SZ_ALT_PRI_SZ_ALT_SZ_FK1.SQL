alter table "SIZE_ALT_ALTERNATE_SIZE"
	add constraint "SZ_ALT_PRI_SZ_ALT_SZ_FK1" foreign key (
		"SIZE_ALTERNATE_RID",
		"PRIMARY_SIZE_SEQ")
	 references "SIZE_ALT_PRIMARY_SIZE" (
		"SIZE_ALTERNATE_RID",
		"PRIMARY_SIZE_SEQ")  

go
