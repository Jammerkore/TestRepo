/* Add foreign key constraints to table "PACK_RULE".                                          */
alter table "PACK_RULE"
	add constraint "STORES_PACK_RULE_FK1" foreign key (
		"ST_RID")
	 references "STORES" (
		"ST_RID")  ON DELETE CASCADE

go
