/* Add foreign key constraints to table "TOTAL_RULE_LAYER".                                   */
alter table "TOTAL_RULE_LAYER"
	add constraint "HEADER_TOTAL_RULE_LAYER_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
