alter table "METHOD_SIZE_BASIS_ALLOCATION"
	add constraint "METH_SZ_BASIS_ALLOC_NSCCD_FK1" foreign key (
		"GENCURVE_NSCCD_RID")
	 references "NODE_SIZE_CURVE_CRITERIA_DETAIL" (
		"NSCCD_RID") on update no action on delete no action  
go
