/* Add foreign key constraints to table "SECURITY_USER_FUNCTION".                             */
alter table "SECURITY_USER_FUNCTION"
	add constraint "USER_SECURITY_FUNCTION_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
