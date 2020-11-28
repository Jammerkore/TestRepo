/* Add foreign key constraints to table "METHOD_MOD_SALES_GRADE".                             */
alter table "METHOD_MOD_SALES_GRADE"
	add constraint "METHOD_MOD_SALES_METHOD_MOD_SALES_GRADE_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD_MOD_SALES" (
		"METHOD_RID")  

go
