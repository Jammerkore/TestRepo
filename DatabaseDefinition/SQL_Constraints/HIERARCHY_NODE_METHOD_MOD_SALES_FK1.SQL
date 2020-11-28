alter table "METHOD_MOD_SALES"
	add constraint "HIERARCHY_NODE_METHOD_MOD_SALES_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
