/* Add foreign key constraints to table "STOCK_ELIGIBILITY_ENTRY".                            */
alter table "STOCK_ELIGIBILITY_ENTRY"
	add constraint "ELIG_MDL_STK_ELIG_ENTRY_FK1" foreign key (
		"EM_RID")
	 references "ELIGIBILITY_MODEL" (
		"EM_RID")  

go
