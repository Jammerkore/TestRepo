alter table "MID_ENQUEUE"
	add constraint "FORECAST_VER_MID_ENQUEUE_FK1" foreign key (
		"FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
