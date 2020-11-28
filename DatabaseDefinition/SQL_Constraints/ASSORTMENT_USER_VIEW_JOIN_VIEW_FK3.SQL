alter table "ASSORTMENT_USER_VIEW_JOIN"
	add constraint "ASSORTMENT_USER_VIEW_JOIN_VIEW_FK3" foreign key (
		"VIEW_RID")
	 references "ASSORTMENT_VIEW" (
		"VIEW_RID")   

go
