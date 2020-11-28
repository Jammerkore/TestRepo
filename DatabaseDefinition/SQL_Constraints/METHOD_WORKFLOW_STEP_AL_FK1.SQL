/* Add foreign key constraints to table "WORKFLOW_STEP_ALLOCATION".                           */
alter table "WORKFLOW_STEP_ALLOCATION"
	add constraint "METHOD_WORKFLOW_STEP_AL_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
