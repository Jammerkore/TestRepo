/* Add foreign key constraints to table "MASTER_HEADER".                                      */
alter table "MASTER_HEADER"
	add constraint "HEADER_MASTER_HEADER_FK1" foreign key (
		"SUBORD_HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
