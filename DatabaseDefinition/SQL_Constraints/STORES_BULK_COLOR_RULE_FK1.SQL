alter table "BULK_COLOR_RULE"
	add constraint "STORES_BULK_COLOR_RULE_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")  ON DELETE CASCADE

go
