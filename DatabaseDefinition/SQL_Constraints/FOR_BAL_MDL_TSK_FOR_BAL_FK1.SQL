alter table "TASK_FORECAST_BALANCE"
	add constraint "FOR_BAL_MDL_TSK_FOR_BAL_FK1" foreign key (
		"FBMOD_RID")
	 references "FORECAST_BAL_MODEL" (
		"FBMOD_RID")  

go
