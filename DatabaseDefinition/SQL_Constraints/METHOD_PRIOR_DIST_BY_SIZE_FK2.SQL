alter table "METHOD_SIZE_BASIS_ALLOCATION"
	add constraint "METHOD_PRIOR_DIST_BY_SIZE_FK2" foreign key (
		"SIZE_BASIS_HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
