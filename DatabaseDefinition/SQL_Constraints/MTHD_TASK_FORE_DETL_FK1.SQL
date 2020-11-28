alter table "TASK_FORECAST_DETAIL"
	add constraint "MTHD_TASK_FORE_DETL_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
