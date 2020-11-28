alter table "METHOD_OVERRIDE"
	add constraint "HIER_NODE_MTHD_OVRD_FK1" foreign key (
		"MERCH_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
