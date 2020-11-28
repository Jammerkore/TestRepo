/* Add foreign key constraints to table "BULK_ALLOCATION".                                    */
alter table "BULK_ALLOCATION"
	add constraint "STORES_BULK_DISTRIBUTION_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")  ON DELETE CASCADE

go
