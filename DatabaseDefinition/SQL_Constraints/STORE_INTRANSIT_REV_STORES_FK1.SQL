/* Add foreign key constraint to table "STORE_INTRANSIT_REV".  */
alter table "STORE_INTRANSIT_REV"
	add constraint "STORE_INTRANSIT_REV_STORES_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")
go
