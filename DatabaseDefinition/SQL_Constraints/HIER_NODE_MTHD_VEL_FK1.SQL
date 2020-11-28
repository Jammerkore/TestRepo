alter table "METHOD_VELOCITY"
	add constraint "HIER_NODE_MTHD_VEL_FK1" foreign key (
		"OTS_PLAN_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
