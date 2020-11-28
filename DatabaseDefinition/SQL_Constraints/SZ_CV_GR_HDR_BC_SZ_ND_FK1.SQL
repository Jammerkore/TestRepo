/* Add foreign key constraints to table "HEADER_BULK_COLOR_SIZE_NEED".                        */
alter table "HEADER_BULK_COLOR_SIZE_NEED"
	add constraint "SZ_CV_GR_HDR_BC_SZ_ND_FK1" foreign key (
		"SIZE_CURVE_GROUP_RID")
	 references "SIZE_CURVE_GROUP" (
		"SIZE_CURVE_GROUP_RID")  
go
