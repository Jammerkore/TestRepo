alter table "SIZE_ALT_PRIMARY_SIZE"
	add constraint "SZS_SZ_ALT_PRI_SZ_FK1" foreign key (
		"SIZES_RID")
	 references "SIZES" (
		"SIZES_RID")  

go
