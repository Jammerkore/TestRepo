alter table "STORE_ELIGIBILITY"
	add constraint "ELIG_MDL_STR_ELIG_FK1" foreign key (
		"EM_RID")
	 references "ELIGIBILITY_MODEL" (
		"EM_RID")  

go
