/* Add foreign key constraints to table "AUDIT_FILTER".                                       */
alter table "AUDIT_FILTER"
	add constraint "APP_USER_AUDIT_FILTER_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
