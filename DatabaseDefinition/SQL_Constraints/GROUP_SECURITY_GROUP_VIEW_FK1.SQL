/* Add foreign key constraints to table "SECURITY_GROUP_VIEW".                                */
alter table "SECURITY_GROUP_VIEW"
	add constraint "GROUP_SECURITY_GROUP_VIEW_FK1" foreign key (
		"GROUP_RID")
	 references "USER_GROUP" (
		"GROUP_RID")  

go
