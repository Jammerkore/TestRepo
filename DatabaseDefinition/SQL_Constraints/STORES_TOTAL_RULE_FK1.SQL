alter table "TOTAL_RULE"
	add constraint "STORES_TOTAL_RULE_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")   ON DELETE CASCADE

go
