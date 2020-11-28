alter table "USER_ALLOCATION_HEADERS"
	add constraint "ALLOC_USER_ALLOC_HEADERS_FK1" foreign key (
		"USER_RID")
	 references "USER_ALLOCATION" (
		"USER_RID")  

go
