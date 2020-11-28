alter table "PACK_RULE_LAYER"
	add constraint "HDR_PK_PK_RULE_LAYER_FK1" foreign key (
		"HDR_PACK_RID")
	 references "HEADER_PACK" (
		"HDR_PACK_RID")  

go
