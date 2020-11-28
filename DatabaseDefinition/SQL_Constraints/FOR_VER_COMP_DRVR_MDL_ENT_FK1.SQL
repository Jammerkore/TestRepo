alter table "COMPUTATION_MODEL_ENTRY"
	add constraint "FOR_VER_COMP_DRVR_MDL_ENT_FK1" foreign key (
		"FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
