alter table "METHOD_RULE"
	add constraint "HDR_BULK_CLR_MTHD_RULE_FK1" foreign key (
		"HDR_BC_RID")
	 references "HEADER_BULK_COLOR" (
		"HDR_BC_RID")  

go
