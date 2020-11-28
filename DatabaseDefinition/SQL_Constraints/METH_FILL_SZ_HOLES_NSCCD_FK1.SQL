alter table "METHOD_FILL_SIZE_HOLES"
	add constraint "METH_FILL_SZ_HOLES_NSCCD_FK1" foreign key (
		"GENCURVE_NSCCD_RID")
	 references "NODE_SIZE_CURVE_CRITERIA_DETAIL" (
		"NSCCD_RID") on update no action on delete no action  
go
