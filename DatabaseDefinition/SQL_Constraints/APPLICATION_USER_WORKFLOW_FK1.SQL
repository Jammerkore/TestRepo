/* Add foreign key constraints to table "WORKFLOW".                                           */
alter table "WORKFLOW"
	add constraint "APPLICATION_USER_WORKFLOW_FK1" foreign key (
		"WORKFLOW_USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
