/* Add foreign key constraints to table "ALLOCATION_STEP_COMPONENT".                          */
alter table "ALLOCATION_STEP_COMPONENT"
	add constraint "COMP_CRIT_WF_STEP_COMP_FK1" foreign key (
		"COMP_CRIT_RID")
	 references "COMPONENT_CRITERIA" (
		"COMP_CRIT_RID")  

go
