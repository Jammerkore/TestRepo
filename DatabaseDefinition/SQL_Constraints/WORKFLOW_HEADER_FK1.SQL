alter table "HEADER"
	add constraint "WORKFLOW_HEADER_FK1" foreign key (
		"WORKFLOW_RID")
	 references "WORKFLOW" (
		"WORKFLOW_RID")  

go
