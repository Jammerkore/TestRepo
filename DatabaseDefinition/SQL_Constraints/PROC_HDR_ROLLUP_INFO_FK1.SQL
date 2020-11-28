/* Add foreign key constraints to table "ROLLUP_INFO".                                        */
alter table "ROLLUP_INFO"
	add constraint "PROC_HDR_ROLLUP_INFO_FK1" foreign key (
		"PROCESS_RID")
	 references "PROC_HDR" (
		"PROCESS_RID")  

go
