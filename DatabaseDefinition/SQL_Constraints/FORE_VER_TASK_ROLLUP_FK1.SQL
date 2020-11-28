alter table "TASK_ROLLUP"
	add constraint "FORE_VER_TASK_ROLLUP_FK1" foreign key (
		"FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
