alter table "STORE_CAPACITY"
	add constraint "STORES_STORE_CAPACITY_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")   ON DELETE CASCADE

go
