alter table "METHOD_MATRIX_BASIS_DETAILS"
	add constraint "FOR_VER_MTHD_MTRX_BAS_DETS_FK1" foreign key (
		"FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
