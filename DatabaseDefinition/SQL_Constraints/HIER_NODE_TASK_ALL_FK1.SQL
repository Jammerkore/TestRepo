alter table "TASK_ALLOCATE"
	add constraint "HIER_NODE_TASK_ALL_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
