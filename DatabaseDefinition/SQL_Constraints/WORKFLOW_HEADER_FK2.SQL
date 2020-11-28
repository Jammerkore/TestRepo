alter table "HEADER"
	add constraint "WORKFLOW_HEADER_FK2" foreign key (
		"API_WORKFLOW_RID")
	 references "WORKFLOW" (
		"WORKFLOW_RID")  

go
