alter table "TASK_FORECAST_BALANCE"
	add constraint "FOR_VER_TSK_FOR_BAL_FK1" foreign key (
		"FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
