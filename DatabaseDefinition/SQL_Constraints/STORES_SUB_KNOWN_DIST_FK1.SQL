/* Add foreign key constraints to table "DETAIL_ALLOCATION".                                  */
alter table "DETAIL_ALLOCATION"
	add constraint "STORES_SUB_KNOWN_DIST_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")  ON DELETE CASCADE

go
