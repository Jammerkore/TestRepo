alter table "BULK_COLOR_ALLOCATION"
	add constraint "STORES_BC_ALOCTN_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")  ON DELETE CASCADE

go
