alter table "USER_ALLOCATION_BASIS"
	add constraint "CAL_DT_RANGE_USER_ALLOC_BAS_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  
go
