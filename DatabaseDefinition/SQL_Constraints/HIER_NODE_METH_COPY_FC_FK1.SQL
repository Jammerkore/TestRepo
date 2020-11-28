alter table "METHOD_COPY_FORECAST"
	add constraint "HIER_NODE_METH_COPY_FC_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
