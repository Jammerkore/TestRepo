alter table "METHOD_GLOBAL_UNLOCK"
	add constraint "CAL_DT_RNG_MTHD_GLBL_UNLK_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")   

go
