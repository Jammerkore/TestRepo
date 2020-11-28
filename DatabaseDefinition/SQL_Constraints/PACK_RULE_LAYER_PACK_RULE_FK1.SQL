alter table "PACK_RULE"
	add constraint "PACK_RULE_LAYER_PACK_RULE_FK1" foreign key (
		"HDR_PACK_RID",
		"LAYER_ID")
	 references "PACK_RULE_LAYER" (
		"HDR_PACK_RID",
		"LAYER_ID")  

go
