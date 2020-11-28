alter table "ASSORTMENT_STORE_SUMMARY"
	add constraint "STORES_ASSORTMENT_STORE_SUMMARY_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")  ON DELETE CASCADE

go
