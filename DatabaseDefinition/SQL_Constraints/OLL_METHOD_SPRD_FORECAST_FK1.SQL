alter table "METHOD_SPREAD_FORECAST"
	add constraint "OLL_METHOD_SPRD_FORECAST_FK1" foreign key ("OLL_RID")
	 references "OVERRIDE_LL_MODEL_HEADER" ("OLL_RID") on update no action on delete no action  	
go
