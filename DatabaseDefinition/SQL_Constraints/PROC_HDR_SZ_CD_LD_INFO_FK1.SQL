/* Add foreign key constraints to table "SIZE_CODE_LOAD_INFO".                                */
alter table "SIZE_CODE_LOAD_INFO"
	add constraint "PROC_HDR_SZ_CD_LD_INFO_FK1" foreign key (
		"PROCESS_RID")
	 references "PROC_HDR" (
		"PROCESS_RID")  

go
