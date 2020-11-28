alter table "STORE_ELIGIBILITY"
	add constraint "FWOS_MOD_MDL_STR_ELIG_FK1" foreign key (
		"FWOSMOD_RID")
	 references "FWOS_MODIFIER_MODEL" (
		"FWOSMOD_RID")  

go
