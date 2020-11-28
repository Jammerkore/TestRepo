alter table "USER_ASSORTMENT_BASIS"
	add constraint "CALENDAR_DATE_RANGE_USER_ASSORTMENT_BASIS_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
