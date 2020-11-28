/* Add foreign key constraints to table "COMPUTATION_DRIVER_INFO".                            */
alter table "COMPUTATION_DRIVER_INFO"
	add constraint "PROC_HDR_COMP_DRVR_INFO_FK1" foreign key (
		"PROCESS_RID")
	 references "PROC_HDR" (
		"PROCESS_RID")  

go
