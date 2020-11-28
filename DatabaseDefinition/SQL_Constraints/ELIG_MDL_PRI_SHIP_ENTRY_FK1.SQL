/* Add foreign key constraints to table "PRIORITY_SHIPPING_ENTRY".                            */
alter table "PRIORITY_SHIPPING_ENTRY"
	add constraint "ELIG_MDL_PRI_SHIP_ENTRY_FK1" foreign key (
		"EM_RID")
	 references "ELIGIBILITY_MODEL" (
		"EM_RID")  

go
