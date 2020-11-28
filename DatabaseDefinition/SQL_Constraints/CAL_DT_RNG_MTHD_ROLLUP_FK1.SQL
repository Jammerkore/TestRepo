alter table "METHOD_ROLLUP"
	add constraint "CAL_DT_RNG_MTHD_ROLLUP_FK1" foreign key ("CDR_RID")
	 references "CALENDAR_DATE_RANGE" ("CDR_RID") on update no action on delete no action  

go
