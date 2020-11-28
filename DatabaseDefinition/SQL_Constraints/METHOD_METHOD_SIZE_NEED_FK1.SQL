/* Add foreign key constraints to table "METHOD_SIZE_NEED".                                   */
alter table "METHOD_SIZE_NEED"
	add constraint "METHOD_METHOD_SIZE_NEED_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
