alter table "SALES_MODIFIER_MODEL_ENTRY"
	add constraint "CAL_DT_RNG_SLS_MOD_MOD_ENT_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
