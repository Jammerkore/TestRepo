alter table "ASSORTMENT_PROPERTIES_BASIS"
	add constraint "FORECAST_VERSION_ASSORTMENT_PROP_BASIS_FK1" foreign key (
		"FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  

go
