alter table "METHOD_EXPORT"
	add constraint "FOR_VER_MTHD_EXP_FK1" foreign key (
		"FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
