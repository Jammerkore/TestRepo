/* Add foreign key constraints to table "AUDIT_MODIFY_SALES".                                 */
alter table "AUDIT_MODIFY_SALES"
	add constraint "AUD_FOR_AUD_MOD_SALES_FK1" foreign key (
		"AUDIT_FORECAST_RID")
	 references "AUDIT_FORECAST" (
		"AUDIT_FORECAST_RID")  

go
