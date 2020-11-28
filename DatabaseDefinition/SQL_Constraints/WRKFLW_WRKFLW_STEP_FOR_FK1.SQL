/* Add foreign key constraints to table "WORKFLOW_STEP_OTSPLAN".                              */
alter table "WORKFLOW_STEP_OTSPLAN"
	add constraint "WRKFLW_WRKFLW_STEP_FOR_FK1" foreign key (
		"WORKFLOW_RID")
	 references "WORKFLOW" (
		"WORKFLOW_RID")  

go
