/* Add foreign key constraints to table "BULK_RULE".                                          */
alter table "BULK_RULE"
	add constraint "BULK_RULE_LAYER_BULK_RULE_FK1" foreign key (
		"HDR_RID",
		"LAYER_ID")
	 references "BULK_RULE_LAYER" (
		"HDR_RID",
		"LAYER_ID")  

go
