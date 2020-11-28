/* Add foreign key constraints to table "SECURITY_GROUP_VERSION".                             */
alter table "SECURITY_GROUP_VERSION"
	add constraint "GROUP_SECURITY_VERSION_FK1" foreign key (
		"GROUP_RID")
	 references "USER_GROUP" (
		"GROUP_RID")  

go
