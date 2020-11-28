/* Add foreign key constraints to table "ASSORTMENT_VIEW".                                    */
alter table "ASSORTMENT_VIEW"
	add constraint "APP_USER_ASSRT_VIEW_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
