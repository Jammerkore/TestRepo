/* Add foreign key constraints to table "BULK_RULE_LAYER".                                    */
alter table "BULK_RULE_LAYER"
	add constraint "HEADER_BULK_RULE_LAYER_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
