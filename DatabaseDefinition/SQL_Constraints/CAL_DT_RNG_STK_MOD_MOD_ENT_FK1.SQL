alter table "STOCK_MODIFIER_MODEL_ENTRY"
	add constraint "CAL_DT_RNG_STK_MOD_MOD_ENT_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
