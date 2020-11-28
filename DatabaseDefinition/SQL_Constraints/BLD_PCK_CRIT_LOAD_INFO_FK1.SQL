alter table "BUILD_PACK_CRITERIA_LOAD_INFO"
	add constraint "BLD_PCK_CRIT_LOAD_INFO_FK1" foreign key (
		"PROCESS_RID")
	 references "PROC_HDR" (
		"PROCESS_RID")  

go
