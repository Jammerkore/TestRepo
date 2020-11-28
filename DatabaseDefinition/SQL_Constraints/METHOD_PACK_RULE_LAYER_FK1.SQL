/* Add foreign key constraints to table "PACK_RULE_LAYER".                                    */
alter table "PACK_RULE_LAYER"
	add constraint "METHOD_PACK_RULE_LAYER_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
