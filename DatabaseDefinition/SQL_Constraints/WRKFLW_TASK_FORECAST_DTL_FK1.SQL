alter table "TASK_FORECAST_DETAIL"
	add constraint "WRKFLW_TASK_FORECAST_DTL_FK1" foreign key (
		"WORKFLOW_RID")
	 references "WORKFLOW" (
		"WORKFLOW_RID")  

go
