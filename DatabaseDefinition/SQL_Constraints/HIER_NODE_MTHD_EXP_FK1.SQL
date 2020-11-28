alter table "METHOD_EXPORT"
	add constraint "HIER_NODE_MTHD_EXP_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
