 alter table "BULK_COLOR_RULE_LAYER"
	add constraint "HEADER_BULK_COLOR_RULE_LAYER_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go  
