alter table "METHOD_GENERAL_ALLOCATION"
	add constraint "CDR_DR_METH_GENL_AL_FK1" foreign key (
		"BEGIN_CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
