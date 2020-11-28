/* Add foreign key constraints to table "TOTAL_RULE".                                         */
alter table "TOTAL_RULE"
	add constraint "TOT_RULE_LAYER_TOT_RULE_FK1" foreign key (
		"HDR_RID",
		"LAYER_ID")
	 references "TOTAL_RULE_LAYER" (
		"HDR_RID",
		"LAYER_ID")  

go
