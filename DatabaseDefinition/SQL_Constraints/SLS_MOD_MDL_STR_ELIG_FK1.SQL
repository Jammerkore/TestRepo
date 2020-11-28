alter table "STORE_ELIGIBILITY"
	add constraint "SLS_MOD_MDL_STR_ELIG_FK1" foreign key (
		"SLSMOD_RID")
	 references "SALES_MODIFIER_MODEL" (
		"SLSMOD_RID")  

go
