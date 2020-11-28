alter table "METHOD_MATRIX_BASIS_DETAILS"
	add constraint "CAL_MT_MTRX_BAS_DTLS_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
