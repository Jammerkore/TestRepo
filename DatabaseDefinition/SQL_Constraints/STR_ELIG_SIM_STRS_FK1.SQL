alter table "SIMILAR_STORES"
	add constraint "STR_ELIG_SIM_STRS_FK1" foreign key (
		"HN_RID",
		"ST_RID")
	 references "STORE_ELIGIBILITY" (
		"HN_RID",
		"ST_RID")  

go
