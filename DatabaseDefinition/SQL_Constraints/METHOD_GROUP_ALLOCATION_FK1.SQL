alter table "METHOD_GROUP_ALLOCATION"
	add constraint "METHOD_GROUP_ALLOCATION_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go