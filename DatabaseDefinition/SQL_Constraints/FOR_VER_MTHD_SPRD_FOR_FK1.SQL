alter table "METHOD_SPREAD_FORECAST"
	add constraint "FOR_VER_MTHD_SPRD_FOR_FK1" foreign key (
		"FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
