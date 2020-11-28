/* Add foreign key constraints to table "WORKFLOW_HISTORY".                                   */
alter table "WORKFLOW_HISTORY"
	add constraint "WORKFLOW_HISTORY_FK1" foreign key (
		"WORKFLOW_RID")
	 references "WORKFLOW" (
		"WORKFLOW_RID")  

go
