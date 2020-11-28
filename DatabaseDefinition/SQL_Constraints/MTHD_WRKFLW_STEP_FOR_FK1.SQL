alter table "WORKFLOW_STEP_OTSPLAN"
	add constraint "MTHD_WRKFLW_STEP_FOR_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
