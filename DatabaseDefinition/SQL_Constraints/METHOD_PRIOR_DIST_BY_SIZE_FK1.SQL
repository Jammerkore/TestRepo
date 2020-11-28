/* Add foreign key constraints to table "METHOD_SIZE_BASIS_ALLOCATION".                       */
alter table "METHOD_SIZE_BASIS_ALLOCATION"
	add constraint "METHOD_PRIOR_DIST_BY_SIZE_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
