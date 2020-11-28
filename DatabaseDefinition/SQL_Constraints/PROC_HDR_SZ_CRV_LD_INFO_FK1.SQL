alter table "SIZE_CURVE_LOAD_INFO"
	add constraint "PROC_HDR_SZ_CRV_LD_INFO_FK1" foreign key (
		"PROCESS_RID")
	 references "PROC_HDR" (
		"PROCESS_RID")  

go
