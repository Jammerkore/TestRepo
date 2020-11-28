alter table "HEADER"
	add constraint "HEADER_HEADER_FK3" foreign key (
		"PLACEHOLDER_RID")
	 references "HEADER" (
		"HDR_RID")  

go
