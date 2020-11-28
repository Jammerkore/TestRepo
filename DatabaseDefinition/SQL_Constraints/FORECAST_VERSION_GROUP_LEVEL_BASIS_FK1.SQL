alter table "GROUP_LEVEL_BASIS"
	add constraint "FORECAST_VERSION_GROUP_LEVEL_BASIS_FK1" foreign key (
		"FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
