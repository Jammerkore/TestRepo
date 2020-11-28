alter table "HEADER"
	add constraint "METHOD_HEADER_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
