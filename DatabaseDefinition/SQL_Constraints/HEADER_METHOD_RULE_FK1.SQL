alter table "METHOD_RULE"
	add constraint "HEADER_METHOD_RULE_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
