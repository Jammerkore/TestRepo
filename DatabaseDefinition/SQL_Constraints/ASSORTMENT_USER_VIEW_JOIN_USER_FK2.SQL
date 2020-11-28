alter table "ASSORTMENT_USER_VIEW_JOIN"
	add constraint "ASSORTMENT_USER_VIEW_JOIN_USER_FK2" foreign key (
		"USER_RID")
	 references "APPLICATION_USER" (
		"USER_RID")  

go