alter table "METHOD_MATRIX"
	add constraint "FOR_VER_MTHD_MTRX_FK1" foreign key (
		"HIGH_LEVEL_FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
