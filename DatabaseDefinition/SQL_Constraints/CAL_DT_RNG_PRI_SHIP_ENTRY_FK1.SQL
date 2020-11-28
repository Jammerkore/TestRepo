alter table "PRIORITY_SHIPPING_ENTRY"
	add constraint "CAL_DT_RNG_PRI_SHIP_ENTRY_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
