alter table "METHOD_VELOCITY_BASIS"
	add constraint "CALENDAR_DATE_RANGE_METHOD_VELOCITY_BASIS_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
