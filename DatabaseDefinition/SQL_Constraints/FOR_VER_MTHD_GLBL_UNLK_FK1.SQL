alter table "METHOD_GLOBAL_UNLOCK"
	add constraint "FOR_VER_MTHD_GLBL_UNLK_FK1" foreign key (
		"FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")   

go
