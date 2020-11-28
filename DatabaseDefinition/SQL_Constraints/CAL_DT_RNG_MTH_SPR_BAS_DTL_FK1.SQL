alter table "METHOD_SPREAD_BASIS_DETAIL"
	add constraint "CAL_DT_RNG_MTH_SPR_BAS_DTL_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
