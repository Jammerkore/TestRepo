/* Add foreign key constraints to table "WORKFLOW_CHANGE_HISTORY".                            */
alter table "WORKFLOW_CHANGE_HISTORY"
	add constraint "WRKFLW_WRKFLW_CHG_HIS_FK1" foreign key (
		"WORKFLOW_RID")
	 references "WORKFLOW" (
		"WORKFLOW_RID")  

go
