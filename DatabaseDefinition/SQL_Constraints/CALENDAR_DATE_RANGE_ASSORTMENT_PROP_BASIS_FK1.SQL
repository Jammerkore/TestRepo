alter table "ASSORTMENT_PROPERTIES_BASIS"
	add constraint "CALENDAR_DATE_RANGE_ASSORTMENT_PROP_BASIS_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
