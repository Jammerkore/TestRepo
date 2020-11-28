alter table "HEADER"
	add constraint "HIERARCHY_NODE_HEADER_FK2" foreign key (
		"PLAN_HNRID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
