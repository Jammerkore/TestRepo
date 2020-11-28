alter table "OTS_PLAN"
	add constraint "FV_METHOD_OTS_PLAN_FK1" foreign key (
		"PLAN_FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
