/* Add foreign key constraints to table "USER_ASSORTMENT".                                    */
alter table "USER_ASSORTMENT"
	add constraint "USER_USER_ASSORTMENT_FK1" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go
