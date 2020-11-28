alter table "METHOD_SIZE_CURVE_MRCH_BAS_DET"
	add constraint "MTHD_SZ_CRV_MRCH_BAS_FV_FK1" foreign key ("FV_RID")
	 references "FORECAST_VERSION" ("FV_RID")  

go
