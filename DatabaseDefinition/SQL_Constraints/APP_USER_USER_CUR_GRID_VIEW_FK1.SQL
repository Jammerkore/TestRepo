alter table "USER_CURRENT_GRID_VIEW"
	add constraint "APP_USER_USER_CUR_GRID_VIEW_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
