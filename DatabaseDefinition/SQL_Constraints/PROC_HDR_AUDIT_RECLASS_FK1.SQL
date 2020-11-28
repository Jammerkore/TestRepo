/* Add foreign key constraints to table "AUDIT_RECLASS".                                      */
alter table "AUDIT_RECLASS"
	add constraint "PROC_HDR_AUDIT_RECLASS_FK1" foreign key (
		"PROCESS_RID")
	 references "PROC_HDR" (
		"PROCESS_RID")  

go
