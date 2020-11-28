alter table "STORE_GROUP_JOIN"
	add constraint "STORE_GROUP_JOIN_FK1" foreign key (
		"SGL_RID")
	 references "STORE_GROUP_LEVEL" (
		"SGL_RID")  

go
