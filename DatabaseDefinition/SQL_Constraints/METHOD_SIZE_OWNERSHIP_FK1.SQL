/* Add foreign key constraints to table "METHOD_SIZE_NEED_ALLOCATION".                        */
alter table "METHOD_SIZE_NEED_ALLOCATION"
	add constraint "METHOD_SIZE_OWNERSHIP_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
