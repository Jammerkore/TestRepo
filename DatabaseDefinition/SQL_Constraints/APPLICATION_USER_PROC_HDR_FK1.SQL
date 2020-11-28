/* Add foreign key constraints to table "PROC_HDR".                                           */
alter table "PROC_HDR"
	add constraint "APPLICATION_USER_PROC_HDR_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
