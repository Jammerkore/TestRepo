alter table "HEADER"
	add constraint "HEADER_HEADER_FK1" foreign key (
		"HDR_GROUP_RID")
	 references "HEADER" (
		"HDR_RID")  

go
