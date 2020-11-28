/* Add foreign key constraints to table "METHOD_MOD_SALES_MATRIX".                            */
alter table "METHOD_MOD_SALES_MATRIX"
	add constraint "METHOD_MOD_SALES_GRADE_METHOD_MOD_SALES_MATRIX_FK1" foreign key (
		"METHOD_RID",
		"BOUNDARY")
	 references "METHOD_MOD_SALES_GRADE" (
		"METHOD_RID",
		"BOUNDARY")  

go
