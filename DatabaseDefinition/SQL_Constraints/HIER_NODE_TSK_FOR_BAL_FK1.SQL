alter table "TASK_FORECAST_BALANCE"
	add constraint "HIER_NODE_TSK_FOR_BAL_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
