/* Begin TT#707 - JScott - Size Curve process needs to multi-thread */
/* Add foreign key constraints to table "TASK_SIZE_CURVE_GENERATE_NODE".                      */
alter table "TASK_SIZE_CURVE_GENERATE_NODE"
	add constraint "TASK_TASK_SZ_CRV_GEN_ND_FK1" foreign key (
		"TASK_SEQUENCE",
		"TASKLIST_RID")
	 references "TASK" (
		"TASK_SEQUENCE",
		"TASKLIST_RID")  

go
