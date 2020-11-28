alter table "METHOD_MOD_SALES_MATRIX"
	add constraint "STORE_GROUP_LEVEL_METHOD_MOD_SALES_MATRIX_FK1" foreign key (
		"SGL_RID")
	 references "STORE_GROUP_LEVEL" (
		"SGL_RID")  

go
