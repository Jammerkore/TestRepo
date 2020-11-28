/* Add foreign key constraints to table "STOCK_MODIFIER_MODEL_ENTRY".                         */
alter table "STOCK_MODIFIER_MODEL_ENTRY"
	add constraint "STK_MOD_MDL_STK_MOD_ENTRY_FK1" foreign key (
		"STKMOD_RID")
	 references "STOCK_MODIFIER_MODEL" (
		"STKMOD_RID")  

go
