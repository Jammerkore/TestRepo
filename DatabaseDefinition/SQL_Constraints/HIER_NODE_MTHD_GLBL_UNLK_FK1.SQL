alter table "METHOD_GLOBAL_UNLOCK"
	add constraint "HIER_NODE_MTHD_GLBL_UNLK_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
