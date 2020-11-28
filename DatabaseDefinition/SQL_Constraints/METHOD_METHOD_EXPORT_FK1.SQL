/* Add foreign key constraints to table "METHOD_EXPORT".                                      */
alter table "METHOD_EXPORT"
	add constraint "METHOD_METHOD_EXPORT_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
