/* Add foreign key constraints to table "AUDIT_RELIEVE_INTRANSIT_INFO".                            */
alter table "AUDIT_RELIEVE_INTRANSIT_INFO"
	add constraint "PROC_HDR_RELV_INT_INFO_FK1" foreign key (
		"PROCESS_RID")
	 references "PROC_HDR" (
		"PROCESS_RID")  

go
