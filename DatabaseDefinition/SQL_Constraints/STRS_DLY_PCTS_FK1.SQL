alter table "DAILY_PERCENTAGES"
	add constraint "STRS_DLY_PCTS_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")  ON DELETE CASCADE

go
