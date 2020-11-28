/* Add foreign key constraints to table "METHOD_FILL_SIZE_HOLES".                             */
alter table "METHOD_FILL_SIZE_HOLES"
	add constraint "METHOD_SIZE_FILL_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
