/* Add foreign key constraints to table "USER_ITEM".                                          */
alter table "USER_ITEM"
	add constraint "APP_USER_ITEM_FK1" foreign key (
		"OWNER_USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
