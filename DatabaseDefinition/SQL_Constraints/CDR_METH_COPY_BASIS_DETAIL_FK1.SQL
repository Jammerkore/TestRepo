alter table "METHOD_COPY_BASIS_DETAIL"
	add constraint "CDR_METH_COPY_BASIS_DETAIL_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
