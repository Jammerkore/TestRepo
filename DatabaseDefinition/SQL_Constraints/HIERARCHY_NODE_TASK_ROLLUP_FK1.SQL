alter table "TASK_ROLLUP"
	add constraint "HIERARCHY_NODE_TASK_ROLLUP_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
