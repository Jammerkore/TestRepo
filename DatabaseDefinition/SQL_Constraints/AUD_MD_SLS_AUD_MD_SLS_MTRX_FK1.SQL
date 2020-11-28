/* Add foreign key constraints to table "AUDIT_MODIFY_SALES_MATRIX".                          */
alter table "AUDIT_MODIFY_SALES_MATRIX"
	add constraint "AUD_MD_SLS_AUD_MD_SLS_MTRX_FK1" foreign key (
		"AUDIT_FORECAST_RID")
	 references "AUDIT_MODIFY_SALES" (
		"AUDIT_FORECAST_RID")  

go
