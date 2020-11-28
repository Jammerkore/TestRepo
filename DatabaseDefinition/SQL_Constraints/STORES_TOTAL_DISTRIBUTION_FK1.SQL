/* Add foreign key constraints to table "TOTAL_ALLOCATION".                                   */
alter table "TOTAL_ALLOCATION"
	add constraint "STORES_TOTAL_DISTRIBUTION_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")   ON DELETE CASCADE

go
