alter table "METHOD_VELOCITY_BASIS"
	add constraint "FOR_VER_MTHD_VEL_BAS_FK1" foreign key (
		"BASIS_FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
