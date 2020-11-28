alter table "HEADER"
	add constraint "SIZE_GROUP_HEADER_FK1" foreign key (
		"SIZE_GROUP_RID")
	 references "SIZE_GROUP" (
		"SIZE_GROUP_RID")  

go
