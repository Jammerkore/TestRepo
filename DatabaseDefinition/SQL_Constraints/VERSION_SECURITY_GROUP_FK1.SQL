alter table "SECURITY_GROUP_VERSION"
	add constraint "VERSION_SECURITY_GROUP_FK1" foreign key (
		"FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
