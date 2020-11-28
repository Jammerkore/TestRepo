alter table "WORKFLOW_HISTORY"
	add constraint "HEADER_WORKFLOW_HISTORY_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
