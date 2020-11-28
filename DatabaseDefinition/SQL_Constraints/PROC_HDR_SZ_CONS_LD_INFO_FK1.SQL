/* Add foreign key constraints to table "SIZE_CONSTRAINT_LOAD_INFO".                          */
alter table "SIZE_CONSTRAINT_LOAD_INFO"
	add constraint "PROC_HDR_SZ_CONS_LD_INFO_FK1" foreign key (
		"PROCESS_RID")
	 references "PROC_HDR" (
		"PROCESS_RID")  

go
