/* Add foreign key constraints to table "FWOS_MODIFIER_MODEL_ENTRY".                          */
alter table "FWOS_MODIFIER_MODEL_ENTRY"
	add constraint "FWOS_MOD_MDL_MDL_ENTRY_FK1" foreign key (
		"FWOSMOD_RID")
	 references "FWOS_MODIFIER_MODEL" (
		"FWOSMOD_RID")  

go
