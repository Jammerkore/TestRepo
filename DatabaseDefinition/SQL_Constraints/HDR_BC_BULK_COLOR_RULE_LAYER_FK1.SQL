alter table "BULK_COLOR_RULE_LAYER"
	add constraint "HDR_BC_BULK_COLOR_RULE_LAYER_FK1" foreign key (
		"HDR_BC_RID")
	 references "HEADER_BULK_COLOR" (
		"HDR_BC_RID")  

go
