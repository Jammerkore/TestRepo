alter table "SIZE_CONSTRAINT_MODEL"
	add constraint "SZ_CRV_GR_SZ_CNST_MDL_FK1" foreign key (
		"SIZE_CURVE_GROUP_RID")
	 references "SIZE_CURVE_GROUP" (
		"SIZE_CURVE_GROUP_RID")  

go
