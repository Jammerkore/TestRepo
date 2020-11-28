/* Add foreign key constraints to table "ASSORTMENT_VIEW_DETAIL".                             */
alter table "ASSORTMENT_VIEW_DETAIL"
	add constraint "ASSRT_VIEW_ASSRT_VIEW_DTL_FK1" foreign key (
		"VIEW_RID")
	 references "ASSORTMENT_VIEW" (
		"VIEW_RID")  

go
