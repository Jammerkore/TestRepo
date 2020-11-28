alter table "GROUP_LEVEL_BASIS"
	add constraint "CALENDAR_DATE_RANGE_GROUP_LEVEL_BASIS_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
