/* Add foreign key constraints to table "USER_ALLOCATION_HEADERS".                            */
alter table "USER_ALLOCATION_HEADERS"
	add constraint "HEADER_USER_ALLOC_HEADERS_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
