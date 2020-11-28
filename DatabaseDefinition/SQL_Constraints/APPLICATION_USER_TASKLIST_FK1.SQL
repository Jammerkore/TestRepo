/* Add foreign key constraints to table "TASKLIST".                                           */
alter table "TASKLIST"
	add constraint "APPLICATION_USER_TASKLIST_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
