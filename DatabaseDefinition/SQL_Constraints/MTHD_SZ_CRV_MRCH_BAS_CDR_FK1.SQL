alter table "METHOD_SIZE_CURVE_MRCH_BAS_DET" 
	add constraint "MTHD_SZ_CRV_MRCH_BAS_CDR_FK1" foreign key ("CDR_RID")
	 references "CALENDAR_DATE_RANGE" ("CDR_RID")  

go
