/* Add foreign key constraints to table "SECURITY_USER_VERSION".                              */
alter table "SECURITY_USER_VERSION"
	add constraint "VERSION_SECURITY_USER_FK1" foreign key (
		"FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
