/* Add foreign key constraints to table "DETAIL_RULE_LAYER".                                  */
alter table "DETAIL_RULE_LAYER"
	add constraint "HEADER_DETAIL_RULE_LAYER_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
