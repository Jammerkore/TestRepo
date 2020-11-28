/* Add foreign key constraints to table "SIZE_MTH_RULE_GRPLVL".                               */
alter table "SIZE_MTH_RULE_GRPLVL"
	add constraint "MTHD_SZ_MTH_CONST_GRPLVL_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
