alter table "METHOD_MATRIX"
	add constraint "HIER_NODE_MTHD_MTRX_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
