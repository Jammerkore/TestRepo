alter table "STORE_ELIGIBILITY"
	add constraint "NODE_STORE_ELIGIBILITY_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
