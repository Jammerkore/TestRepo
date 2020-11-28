/* Add foreign key constraints to table "FILTER_BASIS_ALLOCATION".                            */
alter table "FILTER_BASIS_ALLOCATION"
	add constraint "RCT_METHOD_PRIOR_DISTRO_FK1" foreign key (
		"HDR_RID")
	 references "HEADER" (
		"HDR_RID")  

go
