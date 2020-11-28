/* Add foreign key constraints to table "BULK_COLOR_RULE_LAYER".                              */
alter table "BULK_COLOR_RULE_LAYER"
	add constraint "MTHD_BLK_CLR_RULE_LAYER_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
