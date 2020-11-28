alter table "PACK_ALLOCATION"
	add constraint "STR_PACK_ALLOCATION_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")  ON DELETE CASCADE

go
