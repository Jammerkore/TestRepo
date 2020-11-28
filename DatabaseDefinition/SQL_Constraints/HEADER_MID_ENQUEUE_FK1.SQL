alter table "MID_ENQUEUE"
	add constraint "HEADER_MID_ENQUEUE_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
