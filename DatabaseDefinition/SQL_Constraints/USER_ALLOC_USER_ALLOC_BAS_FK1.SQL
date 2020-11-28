/* Add foreign key constraints to table "USER_ALLOCATION_BASIS".                              */
alter table "USER_ALLOCATION_BASIS"
	add constraint "USER_ALLOC_USER_ALLOC_BAS_FK1" foreign key (
		"USER_RID")
	 references "USER_ALLOCATION" (
		"USER_RID")  
go
