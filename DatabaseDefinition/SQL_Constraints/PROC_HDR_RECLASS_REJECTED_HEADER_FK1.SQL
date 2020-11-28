/* Add foreign key constraints to table "RECLASS_REJECTED_HEADER".                            */
alter table "RECLASS_REJECTED_HEADER"
	add constraint "PROC_HDR_RECLASS_REJECTED_HEADER_FK1" foreign key (
		"PROCESS_RID")
	 references "PROC_HDR" (
		"PROCESS_RID")  

go
