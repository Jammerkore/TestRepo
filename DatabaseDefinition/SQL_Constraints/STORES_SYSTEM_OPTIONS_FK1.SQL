alter table "SYSTEM_OPTIONS"
	add constraint "STORES_SYSTEM_OPTIONS_FK1" foreign key (
		"RESERVE_ST_RID")
	 references "STORES" (
		"ST_RID")   ON DELETE CASCADE

go
