/* Add foreign key constraints to table "USER_WORKFLOW_PREF".                                 */
alter table "USER_WORKFLOW_PREF"
	add constraint "APP_USER_WF_PREF_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
