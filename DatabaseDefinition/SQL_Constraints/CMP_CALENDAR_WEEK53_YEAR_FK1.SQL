/* Add foreign key constraints to table "CALENDAR_WEEK53_YEAR".                               */
alter table "CALENDAR_WEEK53_YEAR"
	add constraint "CMP_CALENDAR_WEEK53_YEAR_FK1" foreign key (
		"CM_RID",
		"CMP_TYPE",
		"CMP_SEQUENCE")
	 references "CALENDAR_MODEL_PERIODS" (
		"CM_RID",
		"CMP_TYPE",
		"CMP_SEQUENCE")  

go
