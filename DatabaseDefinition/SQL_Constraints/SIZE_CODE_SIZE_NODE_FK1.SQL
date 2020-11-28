alter table "SIZE_NODE"
	add constraint "SIZE_CODE_SIZE_NODE_FK1" foreign key (
		"SIZE_CODE_RID")
	 references "SIZE_CODE" (
		"SIZE_CODE_RID")  

go
