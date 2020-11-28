alter table "METHOD_SPREAD_BASIS_DETAIL"
	add constraint "FOR_VER_MTHD_SPRD_BAS_DTL_FK1" foreign key (
		"FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
