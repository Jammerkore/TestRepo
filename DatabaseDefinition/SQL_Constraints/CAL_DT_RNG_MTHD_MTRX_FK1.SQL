alter table "METHOD_MATRIX"
	add constraint "CAL_DT_RNG_MTHD_MTRX_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
