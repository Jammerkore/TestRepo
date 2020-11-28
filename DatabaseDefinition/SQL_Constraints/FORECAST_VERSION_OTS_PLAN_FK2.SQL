alter table "OTS_PLAN"
	add constraint "FORECAST_VERSION_OTS_PLAN_FK2" foreign key (
		"CHAIN_FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
