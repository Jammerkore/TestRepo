alter table "METHOD_COPY_BASIS_DETAIL"
	add constraint "FV_METH_COPY_BASIS_DETAIL_FK1" foreign key (
		"FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
