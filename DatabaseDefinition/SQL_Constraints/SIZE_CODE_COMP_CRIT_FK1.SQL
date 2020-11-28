/* Add foreign key constraints to table "COMPONENT_CRITERIA".                                 */
alter table "COMPONENT_CRITERIA"
	add constraint "SIZE_CODE_COMP_CRIT_FK1" foreign key (
		"SIZE_CODE_RID")
	 references "SIZE_CODE" (
		"SIZE_CODE_RID")  

go
