alter table "STORE_INTRANSIT"
	add constraint "STORES_STORE_INTRANSIT_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")   

go
