alter table "TASK_FORECAST"
	add constraint "HIER_NODE_TASK_FOR_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
