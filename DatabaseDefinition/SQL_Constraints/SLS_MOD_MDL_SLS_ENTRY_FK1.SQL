/* Add foreign key constraints to table "SALES_MODIFIER_MODEL_ENTRY".                         */
alter table "SALES_MODIFIER_MODEL_ENTRY"
	add constraint "SLS_MOD_MDL_SLS_ENTRY_FK1" foreign key (
		"SLSMOD_RID")
	 references "SALES_MODIFIER_MODEL" (
		"SLSMOD_RID")  

go
