alter table "FWOS_MODIFIER_MODEL_ENTRY"
	add constraint "CDR_FWOS_MOD_MDL_ENTRY_FK1" foreign key (
		"CDR_RID")
	 references "CALENDAR_DATE_RANGE" (
		"CDR_RID")  

go
