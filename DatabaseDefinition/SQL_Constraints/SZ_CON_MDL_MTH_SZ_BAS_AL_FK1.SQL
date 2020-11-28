alter table "METHOD_SIZE_BASIS_ALLOCATION"
	add constraint "SZ_CON_MDL_MTH_SZ_BAS_AL_FK1" foreign key (
		"SIZE_CONSTRAINT_RID")
	 references "SIZE_CONSTRAINT_MODEL" (
		"SIZE_CONSTRAINT_RID")  

go
