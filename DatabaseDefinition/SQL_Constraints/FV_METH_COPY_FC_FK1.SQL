alter table "METHOD_COPY_FORECAST"
	add constraint "FV_METH_COPY_FC_FK1" foreign key (
		"FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
