alter table "USER_PLAN"
	add constraint "FV_USER_PLAN_SELECTED_FK2" foreign key (
		"CHAIN_FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
