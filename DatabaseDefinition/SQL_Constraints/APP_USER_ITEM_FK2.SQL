alter table "USER_ITEM"
	add constraint "APP_USER_ITEM_FK2" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
