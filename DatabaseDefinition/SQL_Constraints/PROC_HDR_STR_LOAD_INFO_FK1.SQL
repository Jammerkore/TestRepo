/* Add foreign key constraints to table "STR_LOAD_INFO".                                      */
alter table "STR_LOAD_INFO"
	add constraint "PROC_HDR_STR_LOAD_INFO_FK1" foreign key (
		"PROCESS_RID")
	 references "PROC_HDR" (
		"PROCESS_RID")  

go
