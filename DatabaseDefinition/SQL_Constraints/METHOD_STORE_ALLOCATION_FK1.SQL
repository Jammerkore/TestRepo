/* Add foreign key constraints to table "METHOD_GENERAL_ALLOCATION".                          */
alter table "METHOD_GENERAL_ALLOCATION"
	add constraint "METHOD_STORE_ALLOCATION_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
