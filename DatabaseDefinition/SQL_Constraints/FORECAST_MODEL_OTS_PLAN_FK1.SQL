alter table "OTS_PLAN"
	add constraint "FORECAST_MODEL_OTS_PLAN_FK1" foreign key (
		"FORECAST_MOD_RID")
	 references "FORECAST_MODEL" (
		"FORECAST_MOD_RID")  

go
