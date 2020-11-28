alter table "DAILY_PERCENTAGES_DEFAULTS"
	add constraint "STRS_DLY_PCTS_DFLTS_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")  ON DELETE CASCADE

go
