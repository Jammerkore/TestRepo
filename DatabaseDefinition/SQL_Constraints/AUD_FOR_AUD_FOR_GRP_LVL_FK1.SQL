/* Add foreign key constraints to table "AUDIT_OTS_FORECAST_SET".                             */
alter table "AUDIT_OTS_FORECAST_SET"
	add constraint "AUD_FOR_AUD_FOR_GRP_LVL_FK1" foreign key (
		"AUDIT_FORECAST_RID")
	 references "AUDIT_FORECAST" (
		"AUDIT_FORECAST_RID")  

go
