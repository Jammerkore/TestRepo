/* Add foreign key constraints to table "SECURITY_USER_VIEW".                                 */
alter table "SECURITY_USER_VIEW"
	add constraint "USER_SECURITY_USER_VIEW_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
