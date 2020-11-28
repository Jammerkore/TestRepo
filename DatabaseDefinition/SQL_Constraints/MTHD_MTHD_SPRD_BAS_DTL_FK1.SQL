/* Add foreign key constraints to table "METHOD_SPREAD_BASIS_DETAIL".                         */
alter table "METHOD_SPREAD_BASIS_DETAIL"
	add constraint "MTHD_MTHD_SPRD_BAS_DTL_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
