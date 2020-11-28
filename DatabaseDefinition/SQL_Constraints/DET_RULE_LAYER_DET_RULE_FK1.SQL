/* Add foreign key constraints to table "DETAIL_RULE".                                        */
alter table "DETAIL_RULE"
	add constraint "DET_RULE_LAYER_DET_RULE_FK1" foreign key (
		"HDR_RID",
		"LAYER_ID")
	 references "DETAIL_RULE_LAYER" (
		"HDR_RID",
		"LAYER_ID")  

go
