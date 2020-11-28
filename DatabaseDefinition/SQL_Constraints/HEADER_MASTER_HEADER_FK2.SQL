alter table "MASTER_HEADER"
	add constraint "HEADER_MASTER_HEADER_FK2" foreign key (
		"MASTER_HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
