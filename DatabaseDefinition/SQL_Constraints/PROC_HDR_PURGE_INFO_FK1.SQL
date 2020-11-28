/* Add foreign key constraints to table "PURGE_INFO".                                         */
alter table "PURGE_INFO"
	add constraint "PROC_HDR_PURGE_INFO_FK1" foreign key (
		"PROCESS_RID")
	 references "PROC_HDR" (
		"PROCESS_RID")  

go
