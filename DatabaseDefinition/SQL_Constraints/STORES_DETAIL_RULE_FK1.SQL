alter table "DETAIL_RULE"
	add constraint "STORES_DETAIL_RULE_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")  ON DELETE CASCADE

go
