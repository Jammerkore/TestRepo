/* Add foreign key constraints to table "BULK_COLOR_RULE".                                    */
alter table "BULK_COLOR_RULE"
	add constraint "BLK_CLR_RL_LAY_BLK_CLR_RL_FK1" foreign key (
		"HDR_BC_RID",
		"LAYER_ID")
	 references "BULK_COLOR_RULE_LAYER" (
		"HDR_BC_RID",
		"LAYER_ID")  

go
