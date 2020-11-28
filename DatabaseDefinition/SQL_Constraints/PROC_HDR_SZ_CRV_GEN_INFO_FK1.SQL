/* Add foreign key constraints to table "SIZE_CURVE_GENERATE_INFO".                           */
alter table "SIZE_CURVE_GENERATE_INFO"
	add constraint "PROC_HDR_SZ_CRV_GEN_INFO_FK1" foreign key (
		"PROCESS_RID")
	 references "PROC_HDR" (
		"PROCESS_RID")  

go
