alter table "HEADER"
	add constraint "HEADER_ASRT_FK1" foreign key (
		"ASRT_RID")
	 references "HEADER" (
		"HDR_RID")  

go
