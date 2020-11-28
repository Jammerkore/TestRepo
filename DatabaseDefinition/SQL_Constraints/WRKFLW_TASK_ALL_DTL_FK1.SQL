alter table "TASK_ALLOCATE_DETAIL"
	add constraint "WRKFLW_TASK_ALL_DTL_FK1" foreign key (
		"WORKFLOW_RID")
	 references "WORKFLOW" (
		"WORKFLOW_RID")  

go
