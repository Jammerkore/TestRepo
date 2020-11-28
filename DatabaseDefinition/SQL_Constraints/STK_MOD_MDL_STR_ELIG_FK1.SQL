alter table "STORE_ELIGIBILITY"
	add constraint "STK_MOD_MDL_STR_ELIG_FK1" foreign key (
		"STKMOD_RID")
	 references "STOCK_MODIFIER_MODEL" (
		"STKMOD_RID")  

go
