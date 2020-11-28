alter table "STORE_GROUP_JOIN_HISTORY"
	add constraint "STORE_GROUP_JOIN_HISTORY_FK1" foreign key (
		"SGL_RID")
	 references "STORE_GROUP_LEVEL" (
		"SGL_RID")  

go
