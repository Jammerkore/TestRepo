/* Add foreign key constraints to table "COMPUTATION_PROCESS".                                */
alter table "COMPUTATION_PROCESS"
	add constraint "PROC_HDR_COMP_PROCESS_FK1" foreign key (
		"PROCESS_RID")
	 references "PROC_HDR" (
		"PROCESS_RID")  

go
