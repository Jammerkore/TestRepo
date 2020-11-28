/* Add foreign key constraints to table "CALENDAR_MODEL_PERIODS".                             */
alter table "CALENDAR_MODEL_PERIODS"
	add constraint "CALENDAR_MODEL_PERIODS_FK1" foreign key (
		"CM_RID")
	 references "CALENDAR_MODEL" (
		"CM_RID")  

go
