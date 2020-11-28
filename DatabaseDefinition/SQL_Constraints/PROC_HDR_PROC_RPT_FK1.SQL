/* Add foreign key constraints to table "PROC_RPT".                                           */
alter table "PROC_RPT"
	add constraint "PROC_HDR_PROC_RPT_FK1" foreign key (
		"PROCESS_RID")
	 references "PROC_HDR" (
		"PROCESS_RID")  

go
