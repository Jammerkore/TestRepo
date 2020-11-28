alter table "USER_PLAN"
	add constraint "FOR_VER_USER_PLAN_FK3" foreign key (
		"LOW_LEVEL_FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
