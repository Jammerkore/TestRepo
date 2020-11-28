alter table "METHOD"
	add constraint "APPLICATION_USER_METHOD_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
