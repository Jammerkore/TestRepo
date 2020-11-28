alter table "STORE_SIZE_CURVE_BY_GROUP"
	add constraint "STORES_SIZE_SCALE_BY_GP_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")   ON DELETE CASCADE

go
