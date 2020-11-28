/* Add foreign key constraints to table "SECURITY_USER_HIERARCHY_NODE".                       */
alter table "SECURITY_USER_HIERARCHY_NODE"
	add constraint "HN_SECURITY_USER_HN_FK1" foreign key (
		"HN_RID")
	 references "HIERARCHY_NODE" (
		"HN_RID")  

go
