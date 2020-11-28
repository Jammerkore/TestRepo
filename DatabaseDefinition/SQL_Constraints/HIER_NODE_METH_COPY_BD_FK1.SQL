alter table "METHOD_COPY_BASIS_DETAIL"
	add constraint "HIER_NODE_METH_COPY_BD_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
