/* Add foreign key constraints to table "METHOD_OVERRIDE".                                    */
alter table "METHOD_OVERRIDE"
	add constraint "METHOD_METHOD_OVERRIDE_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
