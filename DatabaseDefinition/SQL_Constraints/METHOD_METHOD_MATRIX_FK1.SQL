/* Add foreign key constraints to table "METHOD_MATRIX".                                      */
alter table "METHOD_MATRIX"
	add constraint "METHOD_METHOD_MATRIX_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
