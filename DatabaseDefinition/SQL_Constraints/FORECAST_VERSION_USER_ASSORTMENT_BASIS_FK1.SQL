alter table "USER_ASSORTMENT_BASIS"
	add constraint "FORECAST_VERSION_USER_ASSORTMENT_BASIS_FK1" foreign key (
		"FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
