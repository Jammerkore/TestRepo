/* Add foreign key constraints to table "POSTING_INFO".                                       */
alter table "POSTING_INFO"
	add constraint "PROC_HDR_POSTING_INFO_FK1" foreign key (
		"PROCESS_RID")
	 references "PROC_HDR" (
		"PROCESS_RID")  

go
