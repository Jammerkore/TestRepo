/* Add foreign key constraints to table "METHOD_VELOCITY".                                    */
alter table "METHOD_VELOCITY"
	add constraint "METHOD_METHOD_VELOCITY_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
