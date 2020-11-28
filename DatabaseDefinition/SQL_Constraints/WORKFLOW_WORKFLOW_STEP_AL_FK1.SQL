alter table "WORKFLOW_STEP_ALLOCATION"
	add constraint "WORKFLOW_WORKFLOW_STEP_AL_FK1" foreign key (
		"WORKFLOW_RID")
	 references "WORKFLOW" (
		"WORKFLOW_RID")  

go
