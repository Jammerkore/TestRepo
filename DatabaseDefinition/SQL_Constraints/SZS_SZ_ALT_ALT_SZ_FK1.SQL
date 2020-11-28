alter table "SIZE_ALT_ALTERNATE_SIZE"
	add constraint "SZS_SZ_ALT_ALT_SZ_FK1" foreign key (
		"SIZES_RID")
	 references "SIZES" (
		"SIZES_RID")  

go
