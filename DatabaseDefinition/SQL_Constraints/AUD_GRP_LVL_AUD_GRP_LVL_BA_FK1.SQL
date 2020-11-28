/* Add foreign key constraints to table "AUDIT_OTS_FORECAST_SET_BASIS".                       */
alter table "AUDIT_OTS_FORECAST_SET_BASIS"
	add constraint "AUD_GRP_LVL_AUD_GRP_LVL_BA_FK1" foreign key (
		"AUDIT_FORECAST_RID",
		"SGL_RID")
	 references "AUDIT_OTS_FORECAST_SET" (
		"AUDIT_FORECAST_RID",
		"SGL_RID")  

go
