alter table "NODE_STOCK_MIN_MAX"
	add constraint "CAL_DT_RGE_NODE_STK_MM_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
