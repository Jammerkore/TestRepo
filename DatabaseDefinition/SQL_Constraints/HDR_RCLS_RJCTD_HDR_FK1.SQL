alter table "RECLASS_REJECTED_HEADER"
	add constraint "HDR_RCLS_RJCTD_HDR_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
