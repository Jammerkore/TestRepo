alter table "USER_PLAN"
	add constraint "FV_USER_PLAN_SELECTED_FK1" foreign key (
		"STORE_FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
