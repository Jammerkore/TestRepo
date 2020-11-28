alter table "METHOD_VELOCITY"
	add constraint "CDR_MTHD_VEL_FK2" foreign key (
		"OTS_SHIP_TO_CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
