alter table "METHOD_VELOCITY"
	add constraint "CDR_MTHD_VEL_FK1" foreign key (
		"OTS_BEGIN_CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
