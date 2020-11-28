/* Add foreign key constraints to table "BULK_COLOR_SIZE_ALLOCATION".                         */
alter table "BULK_COLOR_SIZE_ALLOCATION"
	add constraint "STR_BULK_COLOR_SIZE_DIST_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID") ON DELETE CASCADE

go
