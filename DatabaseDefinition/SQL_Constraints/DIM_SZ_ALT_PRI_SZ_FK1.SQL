alter table "SIZE_ALT_PRIMARY_SIZE"
	add constraint "DIM_SZ_ALT_PRI_SZ_FK1" foreign key (
		"DIMENSIONS_RID")
	 references "DIMENSIONS" (
		"DIMENSIONS_RID")  

go
