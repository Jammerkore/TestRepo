alter table "JOB_TASKLIST_JOIN"
	add constraint "TASKLIST_JOB_TASKLIST_JOIN_FK1" foreign key (
		"TASKLIST_RID")
	 references "TASKLIST" (
		"TASKLIST_RID")  

go
