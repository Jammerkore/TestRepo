alter table "SECURITY_USER_HIERARCHY_NODE"
	add constraint "USER_SECURITY_USER_HN_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
