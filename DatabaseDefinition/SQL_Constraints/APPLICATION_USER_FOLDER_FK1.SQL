/* Add foreign key constraints to table "FOLDER".                                             */
alter table "FOLDER"
	add constraint "APPLICATION_USER_FOLDER_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
