/* Add foreign key constraints to table "ROLLUP_PROCESS".                                     */
alter table "ROLLUP_PROCESS"
	add constraint "PROC_HDR_ROLLUP_PROCESS_FK1" foreign key (
		"PROCESS_RID")
	 references "PROC_HDR" (
		"PROCESS_RID")  

go
