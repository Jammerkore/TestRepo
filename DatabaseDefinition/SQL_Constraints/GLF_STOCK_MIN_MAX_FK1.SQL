/* Add foreign key constraints to table "STOCK_MIN_MAX".                                      */
alter table "STOCK_MIN_MAX"
	add constraint "GLF_STOCK_MIN_MAX_FK1" foreign key (
		"METHOD_RID",
		"SGL_RID")
	 references "GROUP_LEVEL_FUNCTION" (
		"METHOD_RID",
		"SGL_RID")  

go
