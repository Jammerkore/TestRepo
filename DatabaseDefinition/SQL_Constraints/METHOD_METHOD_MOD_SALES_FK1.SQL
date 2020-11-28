/* Add foreign key constraints to table "METHOD_MOD_SALES".                                   */
alter table "METHOD_MOD_SALES"
	add constraint "METHOD_METHOD_MOD_SALES_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
