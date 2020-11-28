alter table "METHOD_SIZE_NEED"
	add constraint "METH_SIZE_NEED_NSCCD_FK1" foreign key (
		"GENCURVE_NSCCD_RID")
	 references "NODE_SIZE_CURVE_CRITERIA_DETAIL" (
		"NSCCD_RID") on update no action on delete no action  
go
