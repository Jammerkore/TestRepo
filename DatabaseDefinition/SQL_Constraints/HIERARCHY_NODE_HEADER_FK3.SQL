alter table "HEADER"
	add constraint "HIERARCHY_NODE_HEADER_FK3" foreign key (
		"ON_HAND_HNRID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
