alter table "METHOD_OVERRIDE"
	add constraint "HIER_NODE_MTHD_OVRD_FK2" foreign key (
		"ON_HAND_HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
