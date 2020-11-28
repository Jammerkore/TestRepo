alter table "USER_ALLOCATION"
	add constraint "STORE_GROUP_USER_ALLOC_FK1" foreign key (
		"SG_RID")
	 references "STORE_GROUP" (
		"SG_RID")  

go
