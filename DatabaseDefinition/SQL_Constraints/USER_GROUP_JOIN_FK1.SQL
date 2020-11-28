alter table "USER_GROUP_JOIN"
	add constraint "USER_GROUP_JOIN_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
