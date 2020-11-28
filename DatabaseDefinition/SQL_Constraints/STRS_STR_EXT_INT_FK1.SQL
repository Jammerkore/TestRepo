alter table "STORE_EXTERNAL_INTRANSIT"
	add constraint "STRS_STR_EXT_INT_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")   ON DELETE CASCADE

go
