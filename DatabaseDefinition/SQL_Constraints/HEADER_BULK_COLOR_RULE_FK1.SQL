alter table "BULK_COLOR_RULE"
	add constraint "HEADER_BULK_COLOR_RULE_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
