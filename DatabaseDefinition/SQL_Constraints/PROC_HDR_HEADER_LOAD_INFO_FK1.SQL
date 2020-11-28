/* Add foreign key constraints to table "HEADER_LOAD_INFO".                                   */
alter table "HEADER_LOAD_INFO"
	add constraint "PROC_HDR_HEADER_LOAD_INFO_FK1" foreign key (
		"PROCESS_RID")
	 references "PROC_HDR" (
		"PROCESS_RID")  

go
