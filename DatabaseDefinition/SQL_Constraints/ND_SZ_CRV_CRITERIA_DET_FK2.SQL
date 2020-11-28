alter table "NODE_SIZE_CURVE_CRITERIA_DETAIL" 
	add constraint "ND_SZ_CRV_CRITERIA_DET_FK2" foreign key ("CDR_RID")
	 references "CALENDAR_DATE_RANGE" ("CDR_RID")  

go
