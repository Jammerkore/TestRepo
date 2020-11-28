alter table "METHOD_RULE"
	add constraint "HEADER_PACK_METHOD_RULE_FK1" foreign key (
		"HDR_PACK_RID")
	 references "HEADER_PACK" (
		"HDR_PACK_RID")  

go
