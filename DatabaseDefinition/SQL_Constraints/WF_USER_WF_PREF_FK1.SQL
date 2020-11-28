alter table "USER_WORKFLOW_PREF"
	add constraint "WF_USER_WF_PREF_FK1" foreign key (
		"WORKFLOW_RID")
	 references "WORKFLOW" (
		"WORKFLOW_RID")  

go
