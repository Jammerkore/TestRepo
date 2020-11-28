alter table "WORKFLOW_CHANGE_HISTORY"
	add constraint "APP_USER_WRKFLW_CHG_HIS_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
