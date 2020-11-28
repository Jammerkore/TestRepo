alter table "TASK_FORECAST"
	add constraint "FOR_VER_TASK_FOR_FK1" foreign key (
		"FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
