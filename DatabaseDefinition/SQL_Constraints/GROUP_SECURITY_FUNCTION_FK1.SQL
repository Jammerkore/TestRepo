/* Add foreign key constraints to table "SECURITY_GROUP_FUNCTION".                            */
alter table "SECURITY_GROUP_FUNCTION"
	add constraint "GROUP_SECURITY_FUNCTION_FK1" foreign key (
		"GROUP_RID")
	 references "USER_GROUP" (
		"GROUP_RID")  

go
