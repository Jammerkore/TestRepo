alter table "METHOD_GENERAL_ALLOCATION"
	add constraint "CDR_DR_METH_GEN_AL_FK2" foreign key (
		"SHIP_TO_CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
