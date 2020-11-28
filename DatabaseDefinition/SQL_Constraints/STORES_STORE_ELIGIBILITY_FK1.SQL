/* Add foreign key constraints to table "STORE_ELIGIBILITY".                                  */
alter table "STORE_ELIGIBILITY"
	add constraint "STORES_STORE_ELIGIBILITY_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")   ON DELETE CASCADE

go
