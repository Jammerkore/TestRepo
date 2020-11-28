alter table "USER_PLAN_BASIS_DETAILS"
	add constraint "FV_PLAN_BASIS_DETAILS_FK1" foreign key (
		"FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
